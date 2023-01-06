using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    #region ATTRIBUTES
    [SerializeField] private GameObject[] _entityPrefabs;                                       //Actual entities gameobjects in scene  
    private List<EntityBase> _entities;                                                         //Complete entities list
    [SerializeField] private int _maxEntities = 1;                                              //Max entity allowed in scene
    #endregion

    #region EVENTS
    public delegate void EntityCreated(EntityBase entity);
    public event EntityCreated OnEntityCreated;                                                 //Raise when entity is created
    public delegate void EntityDestroyed(Guid entity);
    public event EntityDestroyed OnEntityDestroyed;                                             //Raise when entity is destroyed
    public delegate void EntityHit(Guid entity);
    public event EntityHit OnEntityHit;                                                         //Raise when entity take damages
    #endregion

    #region PROPERTIES
    /// <summary>
    /// PROPERTY : Manager instance for direct access
    /// </summary>
    public static EntityManager Instance { get; private set; }

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

        _entities = new List<EntityBase>();
    }

    private void Update()
    {
        CreateEntity();
    }
    #endregion

    #region METHODS
    /// <summary>
    /// METHOD : Get gameobjects entities
    /// </summary>
    /// <returns>List of all entities</returns>
    public List<GameObject> GetEntities()
    {
        return _entities.Select(x => x.gameObject).ToList();
    }

    /// <summary>
    /// METHOD : Get entity with GUID
    /// </summary>
    /// <param name="guid">GUID to find</param>
    /// <returns>EntityBase found</returns>
    public EntityBase GetEntity(Guid guid)
    {
        return _entities.Where(x => x.GUID == guid).FirstOrDefault();
    }

    /// <summary>
    /// METHOD : Create a new entity
    /// </summary>
    public void CreateEntity()
    {
        if (_entities.Count >= _maxEntities)
        {
            return;
        }

        Vector3 entityPosition = GetRandomAvailableTile().Position;

        entityPosition.y = 1.0f;

        int index = UnityEngine.Random.Range(0, _entityPrefabs.Length);

        GameObject tempEntity =  GameObject.Instantiate(_entityPrefabs[index], entityPosition, Quaternion.identity, transform);
        EntityBase tempEntityBase = tempEntity.GetComponent<EntityBase>();

        if (tempEntityBase != null)
        {
            tempEntityBase.OnEntityDied += CBOnEntityDied;

            _entities.Add(tempEntityBase);

            if (OnEntityCreated != null)
            {
                OnEntityCreated(tempEntityBase);
            }

            tempEntityBase.OnDamageReceived += CBOnDamageReceived;
        }
    }

    /// <summary>
    /// METHOD : Apply when entity is destroyed
    /// </summary>
    /// <param name="enityBase">DEstroyed entity</param>
    public void DestroyEntity(EntityBase enityBase)
    {

    }

    /// <summary>
    /// METHOD : Get random available hex when no entities on it
    /// </summary>
    /// <returns>Available hex data</returns>
    private HexData GetRandomAvailableTile()
    {
        List<HexData> unavailableHexDatas = new List<HexData>();
        List<HexData> allHexDatas = new List<HexData>();
        List<HexData> availableHexDatas = new List<HexData>();

        foreach (EntityBase tempEntity in _entities)
        {
            unavailableHexDatas.Add(HexGridManager.Instance.GetNearestTile(tempEntity.transform.position));
        }

        allHexDatas = HexGridManager.Instance.HexTiles.Where(x => x.StructureType == HexTileStructure.Ground && x.Created == true).ToList();
        availableHexDatas = allHexDatas.Except(unavailableHexDatas).ToList();

        return availableHexDatas[UnityEngine.Random.Range(0, availableHexDatas.Count)];
    }
    #endregion

    #region CALLBACKS
    /// <summary>
    /// CALLBACK : When entity died
    /// </summary>
    /// <param name="entityBase">Destroyed entity</param>
    /// <param name="entityDied">DEstroyed entity gameobject</param>
    private void CBOnEntityDied(EntityBase entityBase, GameObject entityDied)
    {
        if (OnEntityDestroyed != null)
        {
            OnEntityDestroyed(entityBase.GetID());
        }

        entityBase.OnDamageReceived -= CBOnDamageReceived;
        _entities.Remove(entityBase);
    }

    /// <summary>
    /// CALLBACK : When an entity take damages
    /// </summary>
    /// <param name="guid">GUID entity</param>
    /// <param name="lifePercent">Life value (percent)</param>
    private void CBOnDamageReceived(Guid guid, float lifePercent)
    {
        OnEntityHit?.Invoke(guid);
    }
    #endregion
}
