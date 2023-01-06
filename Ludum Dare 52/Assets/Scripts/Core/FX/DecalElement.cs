using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DecalElement : MonoBehaviour
{
    #region ATTRIBUTES
    [SerializeField] private DecalProjector _decalProjector;                                        //Decal projector script
    [SerializeField] private float _fadeSpeed;                                                      //Fade speed
    [SerializeField] private Material _decalMaterial;                                               //Material
    #endregion

    #region METHODS
    /// <summary>
    /// METHOD : Set decal sprite on material
    /// </summary>
    /// <param name="sprite">Decal sprite to set</param>
    public void SetSprite(Sprite sprite)
    {
        Material tempMaterial = new Material(_decalMaterial);
        tempMaterial.SetTexture("_BaseMap", sprite.texture);
        _decalProjector.material = tempMaterial;
    }

    /// <summary>
    /// COROUTINE : Fade spray to 0
    /// </summary>
    /// <returns>Coroutine return</returns>
    public IEnumerator Fade()
    {
        if (_decalProjector != null)
        {
            while (_decalProjector.fadeFactor > 0)
            {
                _decalProjector.fadeFactor -= Time.deltaTime * _fadeSpeed;

                yield return null;
            }
        }

        Destroy(gameObject);
    }
    #endregion
}
