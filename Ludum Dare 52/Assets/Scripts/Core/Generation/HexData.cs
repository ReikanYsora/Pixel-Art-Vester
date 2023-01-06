using System;
using UnityEngine;

[Serializable]
public class HexData
{
    #region PROPERTIES
    /// <summary>
    /// PROPERTY : Hex GUID
    /// </summary>
    public Guid GUID { private set; get; }

    /// <summary>
    /// PROPERTY : Hex structure type
    /// </summary>
    public HexTileStructure StructureType { private set; get; }

    /// <summary>
    /// PROPERTY : Hex entity type
    /// </summary>
    public HexTileEntity EntityType { private set; get; }

    /// <summary>
    /// PROPERTY : Hex created status
    /// </summary>
    public bool Created { set; get; }

    /// <summary>
    /// PROPERTY : Hex vector position
    /// </summary>
    public Vector3 Position { private set; get; }

    /// <summary>
    /// PROPERTY : Hex map position
    /// </summary>
    public Vector2Int Coordinates { private set; get; }

    /// <summary>
    /// PROPERTY : linked gameobject
    /// </summary>
    public GameObject GameObject { set; get; }

    /// <summary>
    ///PROPERTY : If hex is walkable or not
    /// </summary>
    public bool Walkable { get; private set; }
    #endregion

    #region CONSTRUCTOR
    /// <summary>
    /// CONSTRUCTOR : Create hex part
    /// </summary>
    /// <param name="structureType">Structure type</param>
    /// <param name="entityType">Entity type</param>
    /// <param name="position">Position</param>
    /// <param name="x">Map coordinate X</param>
    /// <param name="y">Map coordinate Y</param>
    public HexData(HexTileStructure structureType, HexTileEntity entityType, Vector3 position, int x, int y)
    {
        Created = false;
        StructureType = structureType;
        EntityType = entityType;
        Position = position;
        Walkable = (structureType == HexTileStructure.Ground || structureType == HexTileStructure.Door);
        Coordinates = new Vector2Int(x, y);
        GUID = Guid.NewGuid();
    }
    #endregion
}

/// <summary>
/// ENUM : Structure
/// </summary>
public enum HexTileStructure
{
    None,
    Ground, 
    Wall,
    Door
}

/// <summary>
/// ENUM : Entity
/// </summary>
public enum HexTileEntity
{
    None,
    Start,
    Enemy
}

/// <summary>
/// ENUM : Hex rotation
/// </summary>
public enum HexTileRotation
{
    None,
    RotateRight,
    RotateLeft
}
