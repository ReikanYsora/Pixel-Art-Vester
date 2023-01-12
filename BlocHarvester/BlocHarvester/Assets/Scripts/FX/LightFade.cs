using UnityEngine;

public class LightFade : MonoBehaviour
{
    #region ATTRIBUTES
    public float _lifeTime = 0.2f;
    public bool _autoKill = true; 
    private Light _light;
    private float _initialIntensity;
    #endregion

    private void Start()
    {
        if (gameObject.GetComponent<Light>())
        {
            _light = gameObject.GetComponent<Light>();
            _initialIntensity = _light.intensity;
        }
    }

    private void Update()
    {
        if (gameObject.GetComponent<Light>())
        {
            _light.intensity -= _initialIntensity * (Time.deltaTime / _lifeTime);

            if (_autoKill && _light.intensity <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}