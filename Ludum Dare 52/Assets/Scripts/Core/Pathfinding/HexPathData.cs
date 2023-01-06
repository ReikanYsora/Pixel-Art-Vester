using System;
using Unity.Mathematics;

public struct HexPathData
{
    #region ATTRIBUTES
    public int Index;                                                       //Index of hex path data
    public Guid GUID;                                                       //Hex path GUID
    public float2 Position;                                                 //Hex part position
    public int CoordinateX;                                                 //Hex X coordinate on map
    public int CoordinateY;                                                 //Hex Y coordinate on map
    public int Connection;                                                  //Connection with another hex
    public float GCost;                                                     //G Cost
    public float HCost;                                                     //H Cost
    public float FCost;                                                     //F Cost
    #endregion

    #region METHODS
    /// <summary>
    /// METHOD : Calculate cost
    /// </summary>
    public void CalculateFCost()
    {
        FCost = GCost + HCost;
    }
    #endregion
}
