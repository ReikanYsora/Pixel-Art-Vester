using UnityEngine;
using System;

public class HitDamageManager : MonoBehaviour
{
    #region ATTRIBUTES
    [Header("Damage effect")]
    [SerializeField] private Material _damageMaterial;                                                          //Damage material
    #endregion

    #region PROPERTIES
    /// <summary>
    /// PROPERTY : Manager instance for direct access
    /// </summary>
    public static HitDamageManager Instance { get; private set; }
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
        EntityManager.Instance.OnEntityHit += CBOnEntityHit;
        PlayerManager.Instance.OnDamageReceived += CBOnPlayerDamageReceived;
    }

    private void OnDestroy()
    {
        EntityManager.Instance.OnEntityHit -= CBOnEntityHit;
        PlayerManager.Instance.OnDamageReceived -= CBOnPlayerDamageReceived;
    }
    #endregion

    #region CALLBACKS
    /// <summary>
    /// CALLBACK : When entity take damages
    /// </summary>
    /// <param name="entity">Entity GUID</param>
    private void CBOnEntityHit(Guid guid)
    {
        EntityBase _tempEntity = EntityManager.Instance.GetEntity(guid);

        if ((_tempEntity != null) && (_tempEntity.gameObject.GetComponent<DamageEffect>() == null))
        {
            DamageEffect damageEffect = _tempEntity.gameObject.AddComponent<DamageEffect>();
            damageEffect.InitializeEffect(_damageMaterial, 0.05f);
        }
    }

    /// <summary>
    /// CALLBACK : When player take damages
    /// </summary>
    private void CBOnPlayerDamageReceived()
    {
        if (PlayerManager.Instance.Player.GetComponent<DamageEffect>() == null)
        {
            DamageEffect damageEffect = PlayerManager.Instance.Player.AddComponent<DamageEffect>();
            damageEffect.InitializeEffect(_damageMaterial, 0.05f);
        }
    }
    #endregion
}
