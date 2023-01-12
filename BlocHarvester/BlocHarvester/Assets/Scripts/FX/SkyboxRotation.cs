using UnityEngine;

public class SkyboxRotation : MonoBehaviour
{
    #region ATTRIBUTES
    private Material _skyboxMaterial;
    [SerializeField] private float _rotationSpeed;
    private float _angle;
    #endregion

    #region UNITY METHODS
    private void Awake()
    {
        Skybox tempSkyBox = GetComponent<Skybox>();

        if ((tempSkyBox != null) && (tempSkyBox.material != null))
        {
            _skyboxMaterial = tempSkyBox.material;
        }
    }

    private void Update()
    {
        if (_skyboxMaterial != null)
        {
            _angle += Time.deltaTime * _rotationSpeed;

            if (_angle >= 360f)
            {
                _angle = 0;
            }

            _skyboxMaterial.SetFloat("_Rotation", _angle);
        }
    }
    #endregion
}
