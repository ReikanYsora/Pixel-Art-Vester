using DG.Tweening;
using UnityEngine;

public class DrillBehavior : MonoBehaviour
{
    #region ATTRIBUTES
    private Vector3 _originalPosition;
    [SerializeField] private MeshCollider _collider;
    [SerializeField] private Transform _screw;
    [SerializeField] private float _screwRotateSpeed;
    #endregion

    #region UNITY METHODS
    private void Awake()
    {
        _originalPosition = transform.position;
    }

    private void Update()
    {
        _screw.Rotate(new Vector3(_screwRotateSpeed * Time.deltaTime, 0f, 0f));
    }
    #endregion

    public void StartDestroyBlocs()
    {
        _collider.enabled = false;

        transform.DOMoveX(MatrixManager.Instance.GetXWidth() + 3, 2f).OnComplete(() =>
        {
            transform.DOMove(_originalPosition, 0f);
            _collider.enabled = true;
        });
    }
}
