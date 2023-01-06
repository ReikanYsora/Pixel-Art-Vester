using UnityEngine;

public class SimpleRotation : MonoBehaviour
{
    #region ATTRIBUTES
    [SerializeField] private Vector3 _rotationAxis;                                                 //Rotation axis
    [SerializeField] private float _rotationSpeed;                                                  //Rotation speed
    private bool _enabled;                                                                          //Enabled or not
    #endregion

    #region UNITY METHODS
    private void Awake()
    {
        _enabled = true;
    }

    private void Update()
    {
        if (_rotationAxis != Vector3.zero && _enabled)
        {
            transform.Rotate(_rotationAxis * _rotationSpeed * Time.deltaTime);
        }
    }
    #endregion

    #region METHODS
    /// <summary>
    /// METHOD : Disable rotation effect
    /// </summary>
    public void Disable()
    {
        _enabled = false;
    }

    /// <summary>
    /// METHOD : Enable rotation effect
    /// </summary>
    public void Enable()
    {
        _enabled = true;
    }
    #endregion
}
