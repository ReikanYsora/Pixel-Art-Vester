using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region ATTRIBUTES
    [Header("Player instance management")]
    [SerializeField] private GameObject _playerPrefab;                                          //Player prefab
    private GameObject _playerInstance;                                                         //Player gameobject instance

    [Header("Player maze management")]
    [SerializeField][Range(1, 5)] private int _detectionRange = 1;                              //Player detection range
    //private GameObject _nearestDebug = null;                                                  //Nearest debug position

    [Header("Player health and damage management")]
    [SerializeField] private float _maxHealth;                                                  //Max player health
    private float _currentHealth;                                                               //Max current health
    private bool _isAlive;                                                                      //Player is alive or not
    private EntityDamageState _damageState;                                                     //Entity damage state
    #endregion

    #region EVENTS
    public delegate void PlayerDied();                                                          
    public event PlayerDied OnPlayerDied;                                                       //Event : when player died
    public delegate void DamageReceived();
    public event DamageReceived OnDamageReceived;                                               //Event : On damage received
    public delegate void CurrentHealthChanged(float currentHealth);
    public event CurrentHealthChanged OnCurrentHealthChanged;                                   //Event : On current health changed
    public delegate void DamageStateChanged(EntityDamageState damageState);
    public event DamageStateChanged OnDamageStateChanged;                                       //Event : On entity damage changed    
    #endregion

    #region PROPERTIES
    /// <summary>
    /// PROPERTY : Manager instance for direct access
    /// </summary>
    public static PlayerManager Instance { get; private set; }

    /// <summary>
    /// PROPERTY : Get current player health percent
    /// </summary>
    public float CurrentHealthPercent { get { return _currentHealth / _maxHealth; } }

    /// <summary>
    /// METHOD : Set or get entity actual life value
    /// </summary>
    public float CurrentHealth
    {
        set
        {
            if (_currentHealth != value)
            {
                _currentHealth = value;
                OnCurrentHealthChanged?.Invoke(_currentHealth);

                if ((CurrentHealthPercent > 0.75f) && (CurrentHealthPercent <= 1f))
                {
                    DamageState = EntityDamageState.Intact;
                }
                else if ((CurrentHealthPercent > 0.5f) && (CurrentHealthPercent < 0.75f))
                {
                    DamageState = EntityDamageState.Damaged;
                }
                else if ((CurrentHealthPercent > 0.25f) && (CurrentHealthPercent < 0.5f))
                {
                    DamageState = EntityDamageState.HeavyDamaged;
                }
            }
            
            _isAlive = (_currentHealth > 0);
        }
        get
        {
            return _currentHealth;
        }
    }

    /// <summary>
    /// PROPERTY : Set / Get entity damage state
    /// </summary>
    public EntityDamageState DamageState
    {
        set
        {
            if (value != _damageState)
            {
                _damageState = value;
                OnDamageStateChanged?.Invoke(_damageState);
            }
        }
        get
        {
            return _damageState;
        }
    }

    /// <summary>
    /// PROPERTY : Get player instance
    /// </summary>
    public GameObject Player
    {
        get
        {
            return _playerInstance;
        }
    }
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
        _currentHealth = _maxHealth;

        //_nearestDebug = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        //_nearestDebug.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        //Destroy(_nearestDebug.GetComponent<Collider>());
    }

    private void Update()
    {
        //manage available nearest slot position
        //ManageItemSlotPosition();
    }
    #endregion

    #region METHODS
    /// <summary>
    /// METHOD : Manage neareset slot position
    /// </summary>
    private void ManageItemSlotPosition()
    {
        HexData nearestTile = HexGridManager.Instance.GetTileInFrontOf(_playerInstance.transform);

        if (nearestTile != null)
        {
            Vector3 position = nearestTile.Position;
            position.y += 0.5f;
            //_nearestDebug.transform.position = position;
        }
    }

    /// <summary>
    /// METHOD : Create player at position
    /// </summary>
    /// <param name="position">Instantiation position</param>
    /// <param name="rotation">Instantiation rotation</param>
    /// <param name="height">Height from ground</param>
    public void CreatePlayer(Vector3 position, Quaternion rotation, float height = 1.0f)
    {
        _playerInstance = Instantiate(_playerPrefab, position, Quaternion.identity, transform);

        Vector3 playerPosition = position;
        playerPosition.y += height;
        _playerInstance.transform.position = playerPosition;

        CameraManager.Instance.Target = _playerInstance.transform;
    }

    /// <summary>
    /// METHOD : Get player position
    /// </summary>
    /// <returns>Player transform position</returns>
    public Vector3 GetPlayerPosition()
    {
        return _playerInstance.transform.position;
    }

    /// <summary>
    /// METHOD : Get detection range
    /// </summary>
    /// <returns>Player detection range</returns>
    public int GetDetectionRange()
    {
        return _detectionRange;
    }

    /// <summary>
    /// METHOD : Get player transform
    /// </summary>
    /// <returns>Player transform</returns>
    public Transform GetPlayerTransform()
    {
        return _playerInstance.transform;
    }

    /// <summary>
    /// METHOD : Add damage to player
    /// </summary>
    /// <param name="damage">Damage</param>
    public void AddDamage(float damage)
    {
        if (CurrentHealth - damage > 0)
        {
            CurrentHealth -= damage;
        }
        else if (_isAlive)
        {
            CurrentHealth = 0f;
            OnPlayerDied?.Invoke();

            //Play explosion FX
            FXManager.Instance.CreateNukeExplosionEffect(GetPlayerPosition(), GetPlayerTransform());
        }

        OnDamageReceived?.Invoke();
    }
    #endregion
}
