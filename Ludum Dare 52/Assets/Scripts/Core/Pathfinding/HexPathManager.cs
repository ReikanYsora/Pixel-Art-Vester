using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public class HexPathManager : MonoBehaviour
{
    #region ATTRIBUTES
    private float2 _hexOffset;                                          //Hex position offset X and Y
    #endregion

    #region PROPERTIES
    /// <summary>
    /// PROPERTY : Manager instance for direct access
    /// </summary>
    public static HexPathManager Instance { get; private set; }
    #endregion

    #region UNITY METHODS
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        _hexOffset = new float2(HexGridManager.Instance.Offsets.x, HexGridManager.Instance.Offsets.y);
    }
    #endregion

    #region METHODS
    /// <summary>
    /// METHOD : Find path newtween two position defined into a PathfindingOperation
    /// </summary>
    /// <param name="operation">PathfindingOperation</param>
    /// <returns>Queue of GUID to go from Start to End</returns>
    public Queue<Guid> FindPath(PathfindingOperation operation)
    {
        List<HexData> createdTiles = HexGridManager.Instance.HexTiles.Where(x => x.Created && x.Walkable).ToList();
        NativeArray<HexPathData> createdTilesNativeList = new NativeArray<HexPathData>(createdTiles.Count, Allocator.TempJob);
        NativeList<Guid> result = new NativeList<Guid>(Allocator.TempJob);
        int startNoteIndex = -1;
        int targetNodeNoteIndex = -1;

        for (int i = 0; i < createdTiles.Count; i++)
        {
            HexData hexData = createdTiles[i];

            createdTilesNativeList[i] = new HexPathData
            {
                Index = i,
                Position = new float2((float)Math.Round(hexData.Position.x, 2), (float)Math.Round(hexData.Position.z, 2)),
                GUID = hexData.GUID,
                GCost = int.MaxValue,
                HCost = 0,
                Connection = -1,
                CoordinateX = hexData.Coordinates.x,
                CoordinateY = hexData.Coordinates.y
            };

            if (hexData.GUID == operation.StartNode)
            {
                startNoteIndex = i;
            }

            if (hexData.GUID == operation.TargetNode)
            {
                targetNodeNoteIndex = i;
            }
        }

        Queue<Guid> pathResult = new Queue<Guid>();

        if ((startNoteIndex != -1) && (targetNodeNoteIndex != -1))
        {
            PathfindingJob findPathJob = new PathfindingJob
            {
                PathResult = result,
                CreatedTiles = createdTilesNativeList,
                StartNodeIndex = startNoteIndex,
                TargetNodeIndex = targetNodeNoteIndex,
                Offsets = new float2(_hexOffset.x, _hexOffset.y)
            };

            JobHandle jobHandle = findPathJob.Schedule();
            jobHandle.Complete();

            for (int i = 0; i < findPathJob.PathResult.Length; i++)
            {
                pathResult.Enqueue(findPathJob.PathResult[i]);
            }
        }
        else
        {
            createdTilesNativeList.Dispose();
        }

        result.Dispose();

        return pathResult;
    }
    #endregion
}
