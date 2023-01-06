using UnityEngine;

public class FXManager : MonoBehaviour
{
    #region ATTRIBUTES
    [Header("Public trails effects")]
    [SerializeField] private GameObject _lightSmokeTrailPrefab;                                     //Light smoke trail prefab
    [SerializeField] private GameObject _heavySmokeTrailPrefab;                                     //Heavy smoke trail prefab
    [SerializeField] private GameObject _fireTrailPrefab;                                           //Fire trail prefab
    [SerializeField] private GameObject _debrisTrailPrefab;                                         //Debris trail prefab

    [Header("Static loop effects")]
    [SerializeField] private GameObject _heavyFirePrefab;                                           //Heavy fire static prefab
    [SerializeField] private GameObject _twirlingEffect;                                            //Static twirling effect prefab

    [Header("One shot managed effects")]
    [SerializeField] private GameObject _bulletLineRendererPrefab;                                  //Bullet line renderer prefab
    [SerializeField] private float _bulletLineRendererLifeTime;                                     //Bullet line renderer life time
    [SerializeField] private GameObject _muzzleFlashPrefab;                                         //Bullet muzzle flash prefab
    [SerializeField] private float _muzzleFlashLifeTime;                                            //Bullet muzzle flash life time
    [SerializeField] private GameObject _shellPrefab;                                               //Bullet shell prefab
    [SerializeField] private float _shellLifeTime;                                                  //Bullet shell life time
    [SerializeField] private GameObject _minigunImpact;                                             //Minigun shoot impact prefab
    [SerializeField] private float _minigunImpactLifeTime;                                          //Bullet impact life time
    [SerializeField] private GameObject _sprayEffectPrefab;                                         //Spray effect prefab
    [SerializeField] private float _sprayEffectLifeTime;                                            //Spray effect life time

    [SerializeField] private GameObject _explosionSmall;                                            //Explosion small prefab
    [SerializeField] private float _explosionSmallLifeTime;                                         //Explosion small life time
    [SerializeField] private GameObject _explosionHuge;                                             //Explosion huge prefab
    [SerializeField] private float _explosionHugeLifeTime;                                          //Explosion huge life time
    [SerializeField] private GameObject _iceExplosionEffect;                                        //Ice explosion effect
    [SerializeField] private float _iceExplosionEffectLifeTime;                                     //Ice explosion effect life time
    [SerializeField] private GameObject _magneticExplosionEffect;                                   //Magnetic explosion effect
    [SerializeField] private float _magneticExplosionEffectLifeTime;                                //Magnetic explosion effect life time
    [SerializeField] private GameObject _nukeExplosionEffect;                                       //Nuke explosion effect
    [SerializeField] private float _nukeExplosionEffectLifeTime;                                    //Nuke explosion effect life time
    [SerializeField] private GameObject _lightingEffect;                                            //Lightning effect
    [SerializeField] private float _lightingEffectLifeTime;                                         //Lightning effect life time
    #endregion

    #region PROPERTIES
    /// <summary>
    /// PROPERTY : Manager instance for direct access
    /// </summary>
    public static FXManager Instance { get; private set; }

    /// <summary>
    /// PROPERTY : Return light smoke trail prefab effect for non managed instantiation
    /// </summary>
    public GameObject LightSmokeTrailPrefab { get { return _lightSmokeTrailPrefab; } }

    /// <summary>
    /// PROPERTY : Return heavy smoke trail prefab effect for non managed instantiation
    /// </summary>
    public GameObject HeavySmokeTrailPrefab { get { return _heavySmokeTrailPrefab; } }

    /// <summary>
    /// PROPERTY : Return fire trail prefab effect for non managed instantiation
    /// </summary>
    public GameObject FireTrailPrefab { get { return _fireTrailPrefab; } }

    /// <summary>
    /// PROPERTY : Return debris trail prefab effect for non managed instantiation
    /// </summary>
    public GameObject DebrisTrailPrefab { get { return _debrisTrailPrefab; } }
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
    #endregion

    #region METHODS
    /// <summary>
    /// METHOD : Create bullet effects (muzzle, line renderer)
    /// </summary>
    /// <param name="origin">Fire origin</param>
    /// <param name="destination">Fire destination</param>
    public void CreateBulletEffect(Vector3 origin, Quaternion originRotation, Vector3 destination, Transform parent)
    {
        if (_muzzleFlashPrefab)
        {
            GameObject tempMuzzle = null;

            if (parent != null)
            {
                tempMuzzle = GameObject.Instantiate(_muzzleFlashPrefab, origin, originRotation, parent);
            }
            else
            {
                tempMuzzle = GameObject.Instantiate(_muzzleFlashPrefab, origin, originRotation);
            }

            tempMuzzle.transform.SetParent(GameObjectManager.Instance.transform);
            GameObject.Destroy(tempMuzzle, _muzzleFlashLifeTime);
        }

        if (_shellPrefab)
        {
            GameObject tempShell = GameObject.Instantiate(_shellPrefab, origin, originRotation);
            tempShell.transform.SetParent(GameObjectManager.Instance.transform);
            GameObject.Destroy(tempShell, _shellLifeTime);
        }

        if (_bulletLineRendererPrefab != null)
        {
            GameObject tempBulletLineRenderer = GameObject.Instantiate(_bulletLineRendererPrefab);
            tempBulletLineRenderer.GetComponent<BulletLineRenderer>().Initialize(origin, destination);
            tempBulletLineRenderer.transform.SetParent(GameObjectManager.Instance.transform);
            Destroy(tempBulletLineRenderer, _bulletLineRendererLifeTime);
        }
    }

    /// <summary>
    /// METHOD : Create bullet hit impact
    /// </summary>
    /// <param name="hitPoint">Hit point position</param>
    /// <param name="hitRotation">Hit point rotation</param>
    /// <param name="parent">Parent transform</param>
    public void CreateBulletHitImpactEffect(Vector3 hitPoint, Quaternion hitRotation, Transform parent)
    {
        if (_minigunImpact != null)
        {
            GameObject tempImpact = null;

            if (parent != null)
            {
                tempImpact = Instantiate(_minigunImpact, hitPoint, hitRotation, parent);
            }
            else
            {
                tempImpact = Instantiate(_minigunImpact, hitPoint, hitRotation);
                tempImpact.transform.SetParent(GameObjectManager.Instance.transform);
            }

            GameObject.Destroy(tempImpact, _minigunImpactLifeTime);
        }
    }

    /// <summary>
    /// METHOD : Create spray effect
    /// </summary>
    /// <param name="position">Spray effect position</param>
    public void CreateBulletHitImpactEffect(Vector3 position)
    {
        if (_sprayEffectPrefab != null)
        {
            GameObject sprayEffect = Instantiate(_sprayEffectPrefab, position, Quaternion.identity);
            sprayEffect.transform.SetParent(GameObjectManager.Instance.transform);
            GameObject.Destroy(sprayEffect, _sprayEffectLifeTime);
        }
    }

    /// <summary>
    /// METHOD : Create ice explosion effect
    /// </summary>
    /// <param name="position">Hit point position</param>
    /// <param name="parent">Parent transform</param>
    public void CreateIceExplosionEffect(Vector3 position, Transform parent)
    {
        if (_iceExplosionEffect != null)
        {
            GameObject tempImpact = null;

            if (parent != null)
            {
                tempImpact = Instantiate(_iceExplosionEffect, position, Quaternion.identity, parent);
            }
            else
            {
                tempImpact = Instantiate(_iceExplosionEffect, position, Quaternion.identity);
                tempImpact.transform.SetParent(GameObjectManager.Instance.transform);
            }

            GameObject.Destroy(tempImpact, _iceExplosionEffectLifeTime);
        }
    }

    /// <summary>
    /// METHOD : Create magnetic explosion effect
    /// </summary>
    /// <param name="position">Hit point position</param>
    /// <param name="parent">Parent transform</param>
    public void CreateMagneticExplosionEffect(Vector3 position, Transform parent)
    {
        if (_magneticExplosionEffect != null)
        {
            GameObject tempImpact = null;

            if (parent != null)
            {
                tempImpact = Instantiate(_magneticExplosionEffect, position, Quaternion.identity, parent);
            }
            else
            {
                tempImpact = Instantiate(_magneticExplosionEffect, position, Quaternion.identity);
                tempImpact.transform.SetParent(GameObjectManager.Instance.transform);
            }

            GameObject.Destroy(tempImpact, _magneticExplosionEffectLifeTime);
        }
    }

    /// <summary>
    /// METHOD : Create nuke explosion effect
    /// </summary>
    /// <param name="position">Hit point position</param>
    /// <param name="parent">Parent transform</param>
    public void CreateNukeExplosionEffect(Vector3 position, Transform parent)
    {
        if (_nukeExplosionEffect != null)
        {
            GameObject tempImpact = null;

            if (parent != null)
            {
                tempImpact = Instantiate(_nukeExplosionEffect, position, Quaternion.identity, parent);
            }
            else
            {
                tempImpact = Instantiate(_nukeExplosionEffect, position, Quaternion.identity);
                tempImpact.transform.SetParent(GameObjectManager.Instance.transform);
            }

            GameObject.Destroy(tempImpact, _nukeExplosionEffectLifeTime);
        }
    }

    /// <summary>
    /// METHOD : Create lightning effect
    /// </summary>
    /// <param name="position">Position</param>
    /// <param name="parent">Parent transform</param>
    public void CreateLightningEffect(Vector3 position, Transform parent = null)
    {
        if (_lightingEffect != null)
        {
            GameObject tempLightning = null;

            if (parent != null)
            {
                tempLightning = Instantiate(_lightingEffect, position, Quaternion.identity, parent);
            }
            else
            {
                tempLightning = Instantiate(_lightingEffect, position, Quaternion.identity);
                tempLightning.transform.SetParent(GameObjectManager.Instance.transform);
            }

            GameObject.Destroy(tempLightning, _lightingEffectLifeTime);
        }
    }

    /// <summary>
    /// METHOD : Create huge explosion effect
    /// </summary>
    /// <param name="position">Explosion position</param>
    /// <param name="parent">Explosion position</param>
    public void CreateHugeExplosionEffect(Vector3 position, Transform parent = null)
    {
        if (_explosionHuge != null)
        {
            GameObject tempExplosion = null;

            if (parent != null)
            {
                tempExplosion = Instantiate(_explosionHuge, position, Quaternion.identity, parent);
            }
            else
            {
                tempExplosion = Instantiate(_explosionHuge, position, Quaternion.identity);
                tempExplosion.transform.SetParent(GameObjectManager.Instance.transform);
            }

            GameObject.Destroy(tempExplosion, _explosionHugeLifeTime);
        }
    }

    /// <summary>
    /// METHOD : Create explosion effect
    /// </summary>
    /// <param name="position">Explosion position</param>
    /// <param name="parent">Explosion position</param>
    public void CreateExplosionEffect(Vector3 position, Transform parent = null)
    {
        if (_explosionSmall != null)
        {
            GameObject tempExplosion = null;

            if (parent != null)
            {
                tempExplosion = Instantiate(_explosionSmall, position, Quaternion.identity, parent);
            }
            else
            {
                tempExplosion = Instantiate(_explosionSmall, position, Quaternion.identity);
                tempExplosion.transform.SetParent(GameObjectManager.Instance.transform);
            }

            GameObject.Destroy(tempExplosion, _explosionSmallLifeTime);
        }
    }

    /// <summary>
    /// METHOD : Create heavy fire effect
    /// </summary>
    /// <param name="position">Explosion position</param>
    /// <returns>Instantiate effect</returns>
    public GameObject CreateHeavyFireEffect(Vector3 position)
    {
        if (_heavyFirePrefab != null)
        {
            return Instantiate(_heavyFirePrefab, position, Quaternion.identity);
        }

        return null;
    }

    /// <summary>
    /// METHOD : Create twirling effect
    /// </summary>
    /// <param name="position">Loaded effect position</param>
    /// <returns>Instantiate effect</returns>
    public GameObject CreateTwirlingEffect(Vector3 position)
    {
        if (_twirlingEffect != null)
        {
            return Instantiate(_twirlingEffect, position, Quaternion.identity);
        }

        return null;
    }
    #endregion
}
