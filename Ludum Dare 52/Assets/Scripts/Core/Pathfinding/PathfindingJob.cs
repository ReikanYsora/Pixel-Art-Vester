using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

[BurstCompile]
public struct PathfindingJob : IJob
{
    #region ATTRIBUTES
    public int StartNodeIndex;                                                                              //Start node index
    public int TargetNodeIndex;                                                                             //Target node index
    public float2 Offsets;                                                                                  //Hex data map offsets
    [DeallocateOnJobCompletion] public NativeArray<HexPathData> CreatedTiles;                               //Created tiles
    public NativeList<Guid> PathResult;                                                                     //Pathfinding result
    #endregion

    #region METHODS
    /// <summary>
    /// METHOD : Execute JOB in BurstCompile mod
    /// </summary>
    public void Execute()
    {
        NativeList<int> openedList = new NativeList<int>(Allocator.Temp);
        NativeList<int> closedList = new NativeList<int>(Allocator.Temp);
        HexPathData startNode = CreatedTiles[StartNodeIndex];
        HexPathData targetNode = CreatedTiles[TargetNodeIndex];

        for (int i = 0; i < CreatedTiles.Length; i++)
        {
            HexPathData hexData = CreatedTiles[i];
            hexData.HCost = GetDistance(startNode, targetNode);
            hexData.CalculateFCost();
            CreatedTiles[i] = hexData;
        }

        startNode = CreatedTiles[StartNodeIndex];
        startNode.GCost = 0;
        startNode.CalculateFCost();
        CreatedTiles[StartNodeIndex] = startNode;
        openedList.Add(StartNodeIndex);

        while (openedList.Length > 0)
        {
            int currentNodeIndex = GetLowestCostFNodeIndex(openedList, CreatedTiles);
            HexPathData currentNode = CreatedTiles[currentNodeIndex];

            if (currentNodeIndex == TargetNodeIndex)
            {
                break;
            }

            for (int i = 0; i < openedList.Length; i++)
            {
                if (openedList[i] == currentNodeIndex)
                {
                    openedList.RemoveAtSwapBack(i);
                    break;
                }
            }

            closedList.Add(currentNodeIndex);

            NativeArray<int2> directions = GetDirectionsList(currentNode.CoordinateY);

            for (int d = 0; d < directions.Length; d++)
            {
                int2 tempDirection = directions[d];

                for (int i = 0; i < CreatedTiles.Length; i++)
                {
                    HexPathData tempTile = CreatedTiles[i];

                    if ((tempTile.CoordinateX == currentNode.CoordinateX + tempDirection.x) && (tempTile.CoordinateY == currentNode.CoordinateY + tempDirection.y))
                    {
                        if (closedList.Contains(tempTile.Index))
                        {
                            continue;
                        }

                        float tentativeGCost = currentNode.GCost + GetDistance(currentNode, tempTile);

                        if (tentativeGCost < tempTile.GCost)
                        {
                            tempTile.Connection = currentNode.Index;
                            tempTile.GCost = tentativeGCost;
                            tempTile.CalculateFCost();
                            CreatedTiles[tempTile.Index] = tempTile;

                            if (!openedList.Contains(tempTile.Index))
                            {
                                openedList.Add(tempTile.Index);
                            }
                        }
                    }
                }
            }

            directions.Dispose();
        }

        HexPathData endNode = CreatedTiles[TargetNodeIndex];

        if (endNode.Connection != -1)
        {
            PathResult.Add(endNode.GUID);

            HexPathData currentNode = endNode;

            while (currentNode.Connection != -1)
            {
                HexPathData connectionNode = CreatedTiles[currentNode.Connection];
                PathResult.Add(connectionNode.GUID);
                currentNode = connectionNode;
            }
        }

        openedList.Dispose();
        closedList.Dispose();
    }

    /// <summary>
    /// METHOD : Get lowest cost FNode
    /// </summary>
    /// <param name="openList">Open List</param>
    /// <param name="pathNodeArray">Path node</param>
    /// <returns>Lowest cost</returns>
    private int GetLowestCostFNodeIndex(NativeList<int> openList, NativeArray<HexPathData> pathNodeArray)
    {
        HexPathData lowestCostPathNode = pathNodeArray[openList[0]];

        for (int i = 0; i < openList.Length; i++)
        {
            HexPathData testPathNode = pathNodeArray[openList[i]];

            if (testPathNode.FCost < lowestCostPathNode.FCost)
            {
                lowestCostPathNode = testPathNode;
            }
        }

        return lowestCostPathNode.Index;
    }

    /// <summary>
    /// METHOD : Get distance between two hexPathData
    /// </summary>
    /// <param name="from">Position 1</param>
    /// <param name="to">Position 2</param>
    /// <returns>Distance</returns>
    private float GetDistance(HexPathData from, HexPathData to)
    {
        float X = Mathf.Abs(from.Position.x - to.Position.x) / Offsets.x;
        float Y = Mathf.Abs(from.Position.y - to.Position.y) / Offsets.y;

        return X + Y;
    }

    /// <summary>
    /// METHOD : Get directions list by row
    /// </summary>
    /// <param name="row">Row</param>
    /// <returns>List of directions</returns>
    public NativeArray<int2> GetDirectionsList(int row)
    {
        NativeArray<int2> evenList = new NativeArray<int2>(6, Allocator.Temp);
        evenList[0] = new int2(-1, 1);
        evenList[1] = new int2(0, 1);
        evenList[2] = new int2(-1, 0);
        evenList[3] = new int2(1, 0);
        evenList[4] = new int2(-1, -1);
        evenList[5] = new int2(0, -1);

        NativeArray<int2> oddList = new NativeArray<int2>(6, Allocator.Temp);
        oddList[0] = new int2(-1, 0);
        oddList[1] = new int2(1, 0);
        oddList[2] = new int2(0, 1);
        oddList[3] = new int2(1, 1);
        oddList[4] = new int2(0, -1);
        oddList[5] = new int2(1, -1);

        return (row % 2 == 0) ? evenList : oddList;
    }
    #endregion
}
