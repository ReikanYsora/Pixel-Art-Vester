using System;
using UnityEngine;

public class EntityBase : MonoBehaviour
{
    #region ATTRIBUTES
    [Header("Entity name")]
    [SerializeField] private string _entityName;                                                                //Entity name                             
    private Guid _guid;                                                                                         //Entity GUID
    private EntityDamageState _damageState;                                                                     //Entity damage state

    [Header("Stats settings")]
    [SerializeField] private float _maxHealth;                                                                   //Max entity health
    private float _currentHealth;                                                                                //Actual entity health
    #endregion

    #region EVENTS
    public delegate void DamageEventHandler(Guid guid, float lifePercent);
    public event DamageEventHandler OnDamageReceived;
    public delegate void CurrentHealthChanged(float actualLife);
    public event CurrentHealthChanged OnCurrentHealthChanged;
    public delegate void DamageStateChanged(EntityDamageState damageState);
    public event DamageStateChanged OnDamageStateChanged;
    public delegate void EntityDied(EntityBase origin, GameObject entityGameObject);
    public event EntityDied OnEntityDied;                                                           //IEntity event : On entity died
    #endregion

    #region PROPERTIES
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

                if (OnCurrentHealthChanged != null)
                {
                    OnCurrentHealthChanged(_currentHealth);
                }

                if ((GetPercentLife() > 0.75f) && (GetPercentLife() <= 1f))
                {
                    DamageState = EntityDamageState.Intact;
                }
                else if ((GetPercentLife() > 0.5f) && (GetPercentLife() < 0.75f))
                {
                    DamageState = EntityDamageState.Damaged;
                }
                else if ((GetPercentLife() > 0.25f) && (GetPercentLife() < 0.5f))
                {
                    DamageState = EntityDamageState.HeavyDamaged;
                }
            }
        }
        get
        {
            return _currentHealth;
        }
    }

    /// <summary>
    /// PROPERTY : Get entity name
    /// </summary>
    public string EntityName
    {
        get
        {
            return _entityName;
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
    /// PROPERTY : Get entity GUID
    /// </summary>
    public Guid GUID
    {
        get
        {
            return _guid;
        }
    }
    #endregion

    #region METHODS
    /// <summary>
    /// METHOD : Intialize entity states
    /// </summary>
    public void Initialize()
    {
        _currentHealth = _maxHealth;
        _guid = Guid.NewGuid();
    }

    /// <summary>
    /// METHOD : Return entity GUID
    /// </summary>
    /// <returns>Entity GUID</returns>
    public Guid GetID()
    {
        return _guid;
    }

    /// <summary>
    /// METHOD : Return if entity is alive
    /// </summary>
    /// <returns>TRUE or FALSE</returns>
    public bool IsAlive()
    {
        return CurrentHealth > 0;
    }

    /// <summary>
    /// METHOD : Return max health
    /// </summary>
    /// <returns></returns>
    public float GetMaxHealth()
    {
        return _maxHealth;
    }

    /// <summary>
    /// METHOD : Return current life between 0 and 1
    /// </summary>
    /// <returns>Current life between 0 and 1</returns>
    public float GetPercentLife()
    {
        return _currentHealth / _maxHealth;
    }

    /// <summary>
    /// METHOD : Change entity layer
    /// </summary>
    /// <param name="layerMask">Layer name</param>
    /// <param name="childs">Change childs layer</param>
    protected void SetLayerMask(string layerMask, bool childs)
    {
        int layer = LayerMask.NameToLayer(layerMask);
        gameObject.layer = layer;

        if (childs)
        {
            foreach (Transform trans in gameObject.GetComponentsInChildren<Transform>(true))
            {
                trans.gameObject.layer = layer;
            }
        }
    }

    /// <summary>
    /// METHOD : Add damage on certain type on actual entity
    /// </summary>
    /// <param name="damage">Damage value</param>
    /// <param name="hitPosition">Damage position</param>
    public void AddDamage(float damage, Vector3 hitPosition)
    {
        if (IsAlive())
        {
            if (CurrentHealth <= damage)
            {
                CurrentHealth = 0;
                KillEntity();
            }
            else
            {
                float offsetValue = UnityEngine.Random.Range(-0.1f, 0.1f);
                float sizeValue = UnityEngine.Random.Range(20f, 30f);

                CurrentHealth -= damage;
            }

            OnDamageReceived?.Invoke(_guid, CurrentHealth / _maxHealth);
        }
    }

    /// <summary>
    /// METHOD : Kill entity and raise link events
    /// </summary>
    private void KillEntity()
    {
        if (OnEntityDied  != null)
        {
            OnEntityDied(this, gameObject);
        }
    }
    #endregion
}
