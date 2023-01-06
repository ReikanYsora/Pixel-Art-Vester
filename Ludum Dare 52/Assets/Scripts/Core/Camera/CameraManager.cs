using UnityEngine;
using DG.Tweening;

public class CameraManager : MonoBehaviour
{
    #region ATTRIBUTES
    [Header("Camera")]
    [SerializeField] private Camera _camera;
    [SerializeField] private float[] _cameraDistances;
    [SerializeField] private float _cameraModeSpeed = 0.5f;
    private int _cameraSelectedDistance;
    private float _cameraDistance;

    [Header("Camera rotation")]
    [SerializeField] private float _rotationSpeed = 20.0f;
    [SerializeField] private float _rotationDamp = 1.0f;

    [Header("Camera shake")]
    [SerializeField] private float _shakeSmoothness = 100f;

    private float _rotationX = 0.0f;
    private float _distance;
    #endregion

    #region PROPERTIES
    /// <summary>
    /// PROPERTY : Manager instance for direct access
    /// </summary>
    public static CameraManager Instance { get; private set; }

    /// <summary>
    /// PROPERTY : Camera target
    /// </summary>
    public Transform Target { set; get; }

    /// <summary>
    /// PROPERTY : Camera rotation
    /// </summary>
    public Vector2 Rotation { set; get; }

    /// <summary>
    /// PROPERTY : Set or get main camera
    /// </summary>
    public Camera Camera
    { 
        get
        {
            return _camera;
        }
    }
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

        _cameraSelectedDistance = 0;
        _cameraDistance = _cameraDistances[_cameraSelectedDistance];
    }

    private void Start()
    {
        Vector3 angles = transform.eulerAngles;
        _rotationX = angles.x;
    }

    private void LateUpdate()
    {
        if (!Target)
        {
            return;
        }

        if (Rotation.magnitude > 0.1f)
        {
            _rotationX += Rotation.x * _rotationSpeed * 0.02f;
        }
        else
        {
            _rotationX = Mathf.LerpAngle(transform.eulerAngles.y, Target.eulerAngles.y, _rotationDamp * Time.deltaTime);
        }

        _distance = Mathf.Clamp(_distance, _cameraDistance, _cameraDistance);
        Quaternion rotation = Quaternion.Euler(0, _rotationX, 0);
        Vector3 targetOffset = new Vector3(0, -_cameraDistance, 0);
        Vector3 position = Target.position - (rotation * Vector3.forward * _distance + targetOffset);

        transform.rotation = rotation;
        transform.position = position;
    }
    #endregion

    #region METHOD
    /// <summary>
    /// METHOD : Shake camera depending on smoothness and strenght value
    /// </summary>
    /// <param name="strenght">Strenght</param>
    public void Shake(float strenght)
    {
        int strength = Mathf.CeilToInt(strenght / _shakeSmoothness);
        int vibrato = Mathf.CeilToInt(strenght);

        Camera.DOShakePosition(0.2f, strength, vibrato, 5f, true);
    }

    /// <summary>
    /// METHOD : Change current camera distance preset
    /// </summary>
    public void ChangeCameraDistance()
    {
        if (_cameraSelectedDistance < _cameraDistances.Length - 1)
        {
            _cameraSelectedDistance++;
        }
        else
        {
            _cameraSelectedDistance = 0;
        }

        DOTween.To(() => _cameraDistance, x => _cameraDistance = x, _cameraDistances[_cameraSelectedDistance], _cameraModeSpeed).SetUpdate(true);
    }
    #endregion
}