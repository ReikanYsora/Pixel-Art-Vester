using UnityEngine;

public class SprayManager : MonoBehaviour
{
    #region ATTRIBUTES
    [SerializeField] private float _sprayCooldown;                                      //Spray cooldown
    private float _sprayTempCooldown;                                                   //Spray temp cooldown
    private bool _isReady;                                                              //If spray is ready to be applyed
    #endregion

    #region PROPERTIES
    /// <summary>
    /// PROPERTY : Manager instance for direct access
    /// </summary>
    public static SprayManager Instance { get; private set; }

    /// <summary>
    /// PROPERTY : Spray current cooldown
    /// </summary>
    public float Cooldown
    {
        get
        {
            return _sprayTempCooldown / _sprayCooldown;
        }
    }

    /// <summary>
    /// PROPERTY :Get id spray cooldown is ready
    /// </summary>
    public bool IsReady
    {
        get
        {
            return _isReady;
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
        _sprayTempCooldown = 0f;
        _isReady = true;
    }

    private void Update()
    {
        //Manage spray cooldown
        ManageCooldown();
    }
    #endregion

    #region METHODS
    /// <summary>
    /// METHOD : Manage spray cooldown
    /// </summary>
    private void ManageCooldown()
    {
        if ((_sprayTempCooldown <= _sprayCooldown) && (!_isReady))
        {
            _sprayTempCooldown += Time.deltaTime;
        }
        else
        {
            _sprayTempCooldown = 0.0f;
            _isReady = true;
        }
    }

    /// <summary>
    /// METHOD : Create spray at position
    /// </summary>
    /// <param name="position">Position</param>
    public void CreateSpray(Vector3 position)
    {
        Vector3 tempPosition = position;
        tempPosition.y = 0f;

        if (_isReady)
        {
            _isReady = false;

            DecalManager.Instance.CreateSprayAtPosition(tempPosition);
        }
    }
    #endregion
}
