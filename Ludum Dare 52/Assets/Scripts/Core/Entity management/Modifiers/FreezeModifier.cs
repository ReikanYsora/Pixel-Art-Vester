using UnityEngine;

public class FreezeModifier : Modifier
{
    #region ATTRIBUTES
    private IAEntityPhysics _entityPhysics;
    private EntityBase _entityBase;
    #endregion

    #region UNITY METHODS
    private void OnDestroy()
    {
        if ((_entityBase != null) && (_entityBase.CurrentHealth > 0))
        {
            _entityPhysics.IsActive = true;
        }
    }
    #endregion

    #region METHODS
    public bool InitializeModifier(EntityBase entityBase, EntityPhysics entityPhysics, float lifeTime)
    {
        if ((entityPhysics != null) && (entityPhysics is IAEntityPhysics) && (entityBase != null))
        {
            _entityPhysics = (IAEntityPhysics)entityPhysics;
            _entityBase = entityBase;
            _entityPhysics.IsActive = false;

            _cooldownApplyModifier = 0.0f;
            _useCooldown = false;
            _isEnabled = true;

            FXManager.Instance.CreateIceExplosionEffect(transform.position, transform);

            Destroy(this, lifeTime);

            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion
}
