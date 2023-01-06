using DG.Tweening;
using UnityEngine;

public class MagneticModifier : Modifier
{
    #region ATTRIBUTES
    private float _radius;                                                          //Effect radius
    private float _pullForce;                                                       //Pull force
    private GameObject _twirlingEffect;                                             //Twirling visual effect
    #endregion

    #region UNITY METHODS
    private void Awake()
    {
        base.ProtectedAwake();    
    }

    private void Update()
    {
        base.ProtectedUpdate();
    }

    public void FixedUpdate()
    {
        //Manage attraction effect
        ManageAttractionEffect();
    }
    #endregion

    #region METHODS

    /// <summary>
    /// METHOD : Apply attraction force to all managed entity
    /// </summary>
    private void ManageAttractionEffect()
    {
        //Detect all entity in a certain radius sphere
        foreach (Collider collider in Physics.OverlapSphere(transform.position, _radius))
        {
            EntityReference linkEntity = collider.gameObject.GetComponentInChildren<EntityReference>();

            if ((linkEntity != null) && linkEntity.EntityBase.IsAlive() && (linkEntity.EntityPhysics is IAEntityPhysics))
            {
                IAEntityPhysics tempIA = (IAEntityPhysics)linkEntity.EntityPhysics;

                //Create disable IA modifier to disable IA
                DisableModifier disableModifier = null;                    

                if (!tempIA.gameObject.TryGetComponent<DisableModifier>(out disableModifier))
                {
                    disableModifier = tempIA.gameObject.AddComponent<DisableModifier>();
                }

                disableModifier.InitializeModifier(linkEntity.EntityBase, linkEntity.EntityPhysics, _lifeTime - _currentLifeTime);
                Vector3 forceDirection = transform.position - collider.transform.position;
                linkEntity.EntityPhysics.GetRigidbody().AddForce(forceDirection.normalized * _pullForce * Time.fixedDeltaTime);
            }
        }
    }

    /// <summary>
    /// METHOD : Intialize modifier effect
    /// </summary>
    /// <param name="lifeTime">Life time</param>
    /// <param name="radius">Attraction radius</param>
    /// <param name="pullForce">Pull force</param>
    /// <param name="destructionTime">Time when apply secondary effects before destruction</param>
    /// <returns>Initialize result</returns>
    public bool InitializeModifier(float lifeTime, float radius, float pullForce, float destructionTime)
    {
        //Clamp destruction time and life time
        if (destructionTime > lifeTime)
        {
            if (lifeTime <= 0d)
            {
                lifeTime = 0.1f;
            }

            destructionTime = lifeTime - 0.1f;
        }

        _radius = radius;
        _pullForce = pullForce;
        _lifeTime = lifeTime;
        _cooldownApplyModifier = 0f;
        _useCooldown = false;
        _isEnabled = true;
        _destructionTime = destructionTime;

        //Create FX
        float calculatedRadius = (radius - 1f) / Mathf.PI;
        Vector3 scaleRadius = new Vector3(calculatedRadius, calculatedRadius, calculatedRadius);

        _twirlingEffect = FXManager.Instance.CreateTwirlingEffect(transform.position);
        _twirlingEffect.transform.localScale = scaleRadius;
        _twirlingEffect.transform.SetParent(transform);

        if (transform.GetChild(0) != null)
        {
            transform.GetChild(0).DOScale(0.1f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InCirc);
        }

        Destroy(this, _lifeTime);

        return true;
    }

    /// <summary>
    /// METHOD : When modifier destruction is engaged
    /// </summary>
    /// <param name="timeBeforeDestruction">Time before destruction</param>
    protected override void StartDestruction(float timeBeforeDestruction)
    {
        transform.DOScale(0f, timeBeforeDestruction);
        _twirlingEffect.transform.DOScale(0f, timeBeforeDestruction);
        Destroy(_twirlingEffect, timeBeforeDestruction);
        Destroy(gameObject, timeBeforeDestruction);
    }
    #endregion
}
