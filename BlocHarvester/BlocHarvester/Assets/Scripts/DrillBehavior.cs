using DG.Tweening;
using UnityEngine;

public class DrillBehavior : MonoBehaviour
{
    #region ATTRIBUTES
    [SerializeField] private LayerMask _blocLayer;
    private Vector3 _originalPosition;
    [SerializeField] private MeshCollider _collider;
    [SerializeField] private Transform _screw;
    [SerializeField] private float _screwRotateSpeed;
    [SerializeField] private float _blocDetectionRange;
    private bool _isEnabled;

    #endregion

    #region UNITY METHODS
    private void Awake()
    {
        _originalPosition = transform.position;
    }

    private void Update()
    {
        _screw.Rotate(new Vector3(_screwRotateSpeed * Time.deltaTime, 0f, 0f));

        if (_isEnabled)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(transform.position, Vector3.right, out hit, _blocDetectionRange, _blocLayer))
            {
                MatrixManager.Instance.HarvestBloc((int)hit.collider.gameObject.transform.position.x, (int) hit.collider.gameObject.transform.position.z);
            }
        }
    }
    #endregion

    public void StartDestroyBlocs()
    {
        _isEnabled = true;
        _collider.enabled = false;

        transform.DOMoveX(MatrixManager.Instance.GetXWidth() + 3, 2f).OnComplete(() =>
        {
            transform.DOMove(_originalPosition, 0f);
            _collider.enabled = true;
            _isEnabled = false;
        });
    }
}
