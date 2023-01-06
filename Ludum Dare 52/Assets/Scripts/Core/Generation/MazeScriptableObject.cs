using UnityEngine;

[CreateAssetMenu(fileName = "Empty Maze part", menuName = "MazePart")]
public class MazeScriptableObject : ScriptableObject
{
    public Texture2D StructureMap;                                          //Structure map
    public Texture2D EntityMap;                                             //Entity map
}