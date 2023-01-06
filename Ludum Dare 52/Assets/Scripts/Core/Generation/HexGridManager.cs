using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HexGridManager : MonoBehaviour
{
    #region ATTRIBUTES
    [Header("Prefab library")]
    [SerializeField] private GameObject _tilePrefab;
    [SerializeField] private GameObject _wallTilePrefab;
    [SerializeField] private GameObject _wallDoorTilePrefab;

    [Header("Tile generation")]
    [SerializeField] private float _xOffset = 8.66f;
    [SerializeField] private float _yOffset = 7.5f;
    [SerializeField] private float _spawnDelay = 0.01f;

    [Header("Image parsing settings")]
    [SerializeField] private int _filePixelOffsetX = 18;
    [SerializeField] private int _filePixelOffsetY = 15;

    [Header("Maze parts image library")]
    [SerializeField] private MazeScriptableObject[] _mazeParts;

    [Header("Generated grid")]
    [SerializeField] private bool _displayAtStart;
    private HexData _actualTile;

    [Header("Optimization")]
    [SerializeField] private bool _hideNotVisible;
    [SerializeField] private float _detectionThreshold = -7f;
    private List<GameObject> _tiles;
    #endregion

    #region PROPERTIES
    /// <summary>
    /// PROPERTY : Manager instance for direct access
    /// </summary>
    public static HexGridManager Instance { get; private set; }

    /// <summary>
    /// PROPERTY : List of all hex data
    /// </summary>
    public List<HexData> HexTiles { set; get; }

    /// <summary>
    /// PROPERTY : Get or set actual player tile
    /// </summary>
    public HexData ActualTile
    {
        get
        {
            return _actualTile;
        }
        private set
        {
            if (_actualTile != value)
            {
                _actualTile = value;

                if (_actualTile != null)
                {
                    OnActualTileChanged?.Invoke(PlayerManager.Instance.GetPlayerTransform(), _actualTile);
                }
            }
        }
    }

    /// <summary>
    /// METHOD : Get hex data offset
    /// </summary>
    public Vector2 Offsets
    {
        get
        {
            return new Vector2(_xOffset, _yOffset);
        }
    }
    #endregion

    #region EVENTS
    public delegate void ActualTileChanged(Transform playerTransform, HexData actual);
    public event ActualTileChanged OnActualTileChanged;                                                                 //Raise when player hex tile changed
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

        _tiles = new List<GameObject>();

        //Generate grid
        GenerateHexGrid();
    }

    private void Start()
    {
        //Create start tile & player
        HexData startPoint = GetStartPosition();
        CreateHexTile(startPoint);
        PlayerManager.Instance.CreatePlayer(startPoint.Position, Quaternion.identity);

        if (_displayAtStart)
        {
            StartCoroutine(CreateAllHexParts());
        }
    }

    private void Update()
    {
        ChangeActualTile(GetNearestTile(PlayerManager.Instance.GetPlayerPosition()));

        //Manage optimization
        ManageOptimization();
    }
    #endregion

    #region METHODS
    #region FILE IMPORTS
    /// <summary>
    /// METHOD : Import hexa map texture file
    /// </summary>
    public void ImportHexMap()
    {
        HexTiles = new List<HexData>();

        int X;
        float positionX;
        int Y = _filePixelOffsetY;
        float positionY = _yOffset;

        MazeScriptableObject mazePart = _mazeParts[0];
        Texture2D structureMap = mazePart.StructureMap;
        Texture2D entityMap = mazePart.EntityMap;

        int nbColumn = structureMap.height / _filePixelOffsetY;

        for (int column = 0; column < nbColumn; column++)
        {
            bool pair = (column % 2 == 0);
            X = pair ? _filePixelOffsetX / 2 : _filePixelOffsetX;
            positionX = pair ? _xOffset / 2 : _xOffset;
            int nbRow = pair ? structureMap.width / _filePixelOffsetX : structureMap.width / _filePixelOffsetX - 1;

            for (int row = 0; row < nbRow; row++)
            {
                Color pixelStructure = structureMap.GetPixel(X, Y);
                Color pixelEntity = entityMap.GetPixel(X, Y);
                Vector3 position = new Vector3((float) Math.Round(positionX, 2), 0, (float) Math.Round(positionY, 2));
                HexTileStructure mazeStructureType = GetStructureTypeFromColor(pixelStructure);
                HexTileEntity mazeEntityType = GetEntityTypeFromColor(pixelEntity);

                if (mazeStructureType != HexTileStructure.None)
                {
                    HexData tempHexData = new HexData(mazeStructureType, mazeEntityType, position, row, column);
                    HexTiles.Add(tempHexData);
                }

                X += _filePixelOffsetX;
                positionX += _xOffset;
            }

            Y += _filePixelOffsetY;
            positionY += _yOffset;
        }
    }

    /// <summary>
    /// METHOD : Transform color into an hexa tile structure
    /// </summary>
    /// <param name="color">Color</param>
    /// <returns>Hexa tile structure</returns>
    private HexTileStructure GetStructureTypeFromColor(Color color)
    {
        HexTileStructure result = HexTileStructure.None;
        string hexaColor = ColorUtility.ToHtmlStringRGB(color);

        switch (hexaColor.ToUpper())
        {
            case "FFFF00":
                result = HexTileStructure.Door;
                break;
            case "00FF00":
                result = HexTileStructure.Wall;
                break;
            case "FFFFFF":
                result = HexTileStructure.Ground;
                break;
        }

        return result;
    }

    /// <summary>
    /// METHOD : Transform color into an hexa entity type
    /// </summary>
    /// <param name="color">Color</param>
    /// <returns>Hexa entity type</returns>
    private HexTileEntity GetEntityTypeFromColor(Color color)
    {
        HexTileEntity result = HexTileEntity.None;
        string hexaColor = ColorUtility.ToHtmlStringRGB(color);

        switch (hexaColor.ToUpper())
        {
            case "0000FF":
                result = HexTileEntity.Enemy;
                break;
            case "FF0000":
                result = HexTileEntity.Start;
                break;
        }

        return result;
    }
    #endregion

    /// <summary>
    /// METHOD : If option is enabled, hide all not visible parts
    /// </summary>
    private void ManageOptimization()
    {
        if ((_hideNotVisible) && (CameraManager.Instance.Camera != null))
        {
            foreach (GameObject tile in _tiles)
            {
                Renderer[] tempRenderers = tile.GetComponentsInChildren<Renderer>();

                if ((tempRenderers != null) && (tempRenderers.Length > 0))
                {
                    tempRenderers.ToList().ForEach(x => x.enabled = IsVisible(tile));
                }
            }
        }
    }

    /// <summary>
    /// METHOD : Get if maze had uncreated parts
    /// </summary>
    /// <returns></returns>
    public bool HasUncreatedPart()
    {
        return HexTiles.Where(x => x.Created == false).Any();
    }
     
    /// <summary>
    /// METHOD : Generate hex grid
    /// </summary>
    public void GenerateHexGrid()
    {
        ImportHexMap();
    }

    /// <summary>
    /// METHOD : Get random start position
    /// </summary>
    /// <returns>Start position</returns>
    public HexData GetStartPosition()
    {
        List<HexData> startPoints = HexTiles.Where(x => x.EntityType == HexTileEntity.Start).ToList();
        return startPoints[UnityEngine.Random.Range(0, startPoints.Count)];
    }
    
    /// <summary>
    /// METHOD : Get list of hexa maze created in a specific detection range from a specific hexa tile
    /// </summary>
    /// <param name="tile">Center tile</param>
    /// <param name="detectionRange">Detection range</param>
    /// <returns>List of hexa parts in range</returns>
    public List<HexData> GetHexTilesInRange(HexData tile, int detectionRange)
    {
        List<HexData> result = new List<HexData>();

        for (int i = 1; i <= detectionRange; i++)
        {
            result.AddRange(HexTiles.Where(x => Vector3.Distance(tile.Position, x.Position) <= i * (_xOffset + 0.1f) && (Vector3.Distance(tile.Position, x.Position) > (i - 1) * (_xOffset + 0.1f)) && x.Created == false).ToList());
        }

        foreach (HexData item in result)
        {
            item.Created = true;
        }

        return result;
    }

    /// <summary>
    /// METHOD : Get tile in front of a transform
    /// </summary>
    /// <param name="transform">Transform</param>
    /// <returns>Hexa tile in front of transform</returns>
    public HexData GetTileInFrontOf(Transform transform)
    {
        List<HexData> nearestTiles = HexTiles.Where(x => Vector3.Distance(transform.position, x.Position) <= _xOffset + 0.1f && x.Created == true).ToList();
        float minDot = -1.0f;
        HexData result = null;

        foreach (HexData tempTile in nearestTiles)
        {
            float tempDot = Vector3.Dot(tempTile.Position - transform.position, transform.forward);

            if (tempDot > minDot)
            {
                minDot = tempDot;
                result = tempTile;
            }
        }

        return result;
    }

    /// <summary>
    /// METHOD : Get neareset hexa part of position
    /// </summary>
    /// <param name="position">Position</param>
    /// <returns>Neareset hexa part</returns>
    public HexData GetNearestTile(Vector3 position)
    {
        HexData nearestTile = null;
        float distance = Mathf.Infinity;

        foreach (HexData mazeTile in HexTiles)
        {
            float tempDistance = Vector3.Distance(mazeTile.Position, position);

            if (tempDistance < distance)
            {
                distance = tempDistance;
                nearestTile = mazeTile;
            }
        }

        return nearestTile;
    }
    
    /// <summary>
    /// METHOD : Get uncreated next part
    /// </summary>
    /// <returns>Hex data uncreated</returns>
    public HexData GetNextUncreatedPart()
    {
        return HexTiles.Where(x => x.Created == false).FirstOrDefault();
    }

    /// <summary>
    /// METHOD : Create hexa part
    /// </summary>
    /// <param name="tileToCreate">HexData to transform into hexa part</param>
    public void CreateHexTile(HexData tileToCreate)
    {
        if (tileToCreate != null)
        {
            GameObject tempTile = null;
            GameObject toInstantiate = null;

            switch (tileToCreate.StructureType)
            {
                case HexTileStructure.Ground:
                    toInstantiate = _tilePrefab;
                    break;
                case HexTileStructure.Wall:
                    toInstantiate = _wallTilePrefab;
                    break;
                case HexTileStructure.Door:
                    toInstantiate = _wallDoorTilePrefab;
                    break;
            }

            if (toInstantiate != null)
            {
                tempTile = GameObject.Instantiate(toInstantiate, tileToCreate.Position, Quaternion.identity, transform);      
                string name = string.Format("{0:D3}_{1:D3}", tileToCreate.Coordinates.x, tileToCreate.Coordinates.y);
                tempTile.name = name;
                tileToCreate.GameObject = tempTile;
                tileToCreate.Created = true;

                _tiles.Add(tempTile);
            }
        }
    }
    /// <summary>
    /// METHOD : Define is gameobject is visible by the camera
    /// </summary>
    /// <param name="gameObject">GameObject to check</param>
    /// <returns>TRUE or FALSE</returns>
    private bool IsVisible(GameObject gameObject)
    {
        var planes = GeometryUtility.CalculateFrustumPlanes(CameraManager.Instance.Camera);
        var point = gameObject.transform.position;

        foreach (var plane in planes)
        {
            if (plane.GetDistanceToPoint(point) < _detectionThreshold)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// COROUTINE : Create all hex parts
    /// </summary>
    /// <returns>COROUTINE return</returns>
    IEnumerator CreateAllHexParts()
    {
        while (HasUncreatedPart())
        {
            HexData tileToCreate = GetNextUncreatedPart();

            if (tileToCreate != null)
            {
                CreateHexTile(tileToCreate);
                yield return new WaitForSeconds(_spawnDelay);
            }

            yield return new WaitForSeconds(0);
        }

        yield return true;
    }

    /// <summary>
    /// COROUTINE : Create all hex in specific range of another hex part
    /// </summary>
    /// <param name="tile"></param>
    /// <returns>COROUTINE return</returns>
    IEnumerator CreateHexTilesInRange(HexData tile)
    {
        List<HexData> tilesToCreate = GetHexTilesInRange(tile, PlayerManager.Instance.GetDetectionRange());

        foreach (HexData tileToCreate in tilesToCreate)
        {
            if (tileToCreate != null)
            {
                CreateHexTile(tileToCreate);
                yield return new WaitForSeconds(_spawnDelay);
            }
            else
            {
                yield return new WaitForSeconds(0);
            }
        }

        yield return true;
    }

    /// <summary>
    /// METHOD : Define actual tile
    /// </summary>
    /// <param name="tile">Actual tile</param>
    public void ChangeActualTile(HexData tile)
    {
        if ((tile != ActualTile) && (tile != null))
        {
            ActualTile = tile;
            StartCoroutine(CreateHexTilesInRange(ActualTile));
        }
    }

    /// <summary>
    /// METHOD : Get a specific hexa part by GUID
    /// </summary>
    /// <param name="guid">GUID</param>
    /// <returns>Hex data</returns>
    public HexData GetHexTile(Guid guid)
    {
        return HexTiles.Where(x => x.GUID == guid).FirstOrDefault();
    }

    /// <summary>
    /// METHOD : Get a random created and walkable tile
    /// </summary>
    /// <returns>Random created and walkable tile</returns>
    public HexData GetRandomTile()
    {
        var t = HexTiles.Where(x => x.Created && x.Walkable && x != null).ToList();
        HexData d = t[UnityEngine.Random.Range(0, t.Count)];
        return d;
    }
    #endregion
}
