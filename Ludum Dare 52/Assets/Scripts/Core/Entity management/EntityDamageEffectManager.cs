using UnityEngine;

[RequireComponent(typeof(EntityBase))]
public class EntityDamageEffectManager : MonoBehaviour
{
    #region ATTRIBUTES
    [SerializeField] private Transform[] _damagePositions;
    private GameObject _lightSmokeTrail;
    private GameObject _heavySmokeTrail;
    private GameObject _debrisTrail;
    private EntityBase _entity;
    #endregion

    #region UNITY METHODS
    private void Start()
    {
        _entity = GetComponent<EntityBase>();
        _entity.OnDamageStateChanged += CBOnDamageStateChanged;
        _entity.OnEntityDied += CBOnEntityDied;
        _lightSmokeTrail = CreateDamageTrailEffectAndStop(FXManager.Instance.LightSmokeTrailPrefab);
        _debrisTrail = CreateDamageTrailEffectAndStop(FXManager.Instance.DebrisTrailPrefab);
        _heavySmokeTrail = CreateDamageTrailEffectAndStop(FXManager.Instance.HeavySmokeTrailPrefab);
    }

    private void OnDestroy()
    {
        _entity.OnDamageStateChanged -= CBOnDamageStateChanged;
        _entity.OnEntityDied -= CBOnEntityDied;
    }

    /// <summary>
    /// METHOD : Create damage trail effect
    /// </summary>
    /// <param name="prefab">Original prefab</param>
    /// <returns>Instantiate prefab</returns>
    private GameObject CreateDamageTrailEffectAndStop(GameObject prefab)
    {
        int position = Random.Range(0, _damagePositions.Length - 1);
        GameObject tempEffect = GameObject.Instantiate(prefab);
        tempEffect.GetComponent<ParticleSystem>().Stop();
        tempEffect.transform.position = _damagePositions[position].position;
        tempEffect.transform.SetParent(transform);

        return tempEffect;
    }

    /// <summary>
    /// METHOD : Create explosion text effect
    /// </summary>
    /// <param name="state">Current entity damage state</param>
    private void CreateExplosionTextEffect(EntityDamageState state)
    {
        switch (state)
        {
            default:
            case EntityDamageState.Intact:
                return;
            case EntityDamageState.Damaged:
            case EntityDamageState.HeavyDamaged:
                break;
        }
    }
    #endregion

    #region CALLBACKS
    /// <summary>
    /// CALLBACK : When entity damage state changed
    /// </summary>
    /// <param name="damageState">Damage state</param>
    private void CBOnDamageStateChanged(EntityDamageState damageState)
    {
        CreateExplosionTextEffect(damageState);
        DecalManager.Instance.CreateDecalAtPosition(transform.position);

        switch (damageState)
        {
            default:
            case EntityDamageState.Intact:
                _lightSmokeTrail.GetComponent<ParticleSystem>().Stop();
                _debrisTrail.GetComponent<ParticleSystem>().Stop();
                _heavySmokeTrail.GetComponent<ParticleSystem>().Stop();
                break;
            case EntityDamageState.Damaged:
                FXManager.Instance.CreateExplosionEffect(transform.position);
                _lightSmokeTrail.GetComponent<ParticleSystem>().Play();
                _debrisTrail.GetComponent<ParticleSystem>().Stop();
                _heavySmokeTrail.GetComponent<ParticleSystem>().Stop();
                break;
            case EntityDamageState.HeavyDamaged:
                FXManager.Instance.CreateExplosionEffect(transform.position);

                _lightSmokeTrail.GetComponent<ParticleSystem>().Play();
                _debrisTrail.GetComponent<ParticleSystem>().Play();
                _heavySmokeTrail.GetComponent<ParticleSystem>().Play();
                break;
        }
    }

    /// <summary>
    /// CALLBACK : When entity died
    /// </summary>
    private void CBOnEntityDied(EntityBase origin, GameObject entityGameObject)
    {
        FXManager.Instance.CreateHugeExplosionEffect(transform.position, transform);
        GameObject heavyFire = FXManager.Instance.CreateHeavyFireEffect(transform.position);

        if (heavyFire != null)
        {
            heavyFire.transform.SetParent(transform);
        }
    }
    #endregion
}
public enum EntityDamageState
{
    Intact, Damaged, HeavyDamaged
}