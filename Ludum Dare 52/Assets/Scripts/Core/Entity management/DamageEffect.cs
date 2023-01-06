using UnityEngine;

public class DamageEffect : MonoBehaviour
{
    #region ATTRIBUTES
    private Material _originalMaterial;                                                 //Original material
    private MeshRenderer _meshRenderer;                                                 //Mesh renderer
    #endregion

    #region UNITY METHODS
    private void OnDestroy()
    {
        if (_meshRenderer != null)
        {
            _meshRenderer.material = _originalMaterial;
        }
    }
    #endregion

    #region METHODS
    /// <summary>
    /// METHOD : Intialize damage effect
    /// </summary>
    /// <param name="damageMaterial">Damage material applied on hit</param>
    /// <param name="delay">Display delay</param>
    public void InitializeEffect(Material damageMaterial, float delay)
    {
        _meshRenderer = GetComponentInChildren<MeshRenderer>();

        if (_meshRenderer != null)
        {
            _originalMaterial = _meshRenderer.material;
            _meshRenderer.material = damageMaterial;
        }

        Destroy(this, delay);
    }
    #endregion
}
