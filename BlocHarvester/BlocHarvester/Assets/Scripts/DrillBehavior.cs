using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DrillBehavior : MonoBehaviour
{
    #region ATTRIBUTES
    [SerializeField] private LayerMask _blocLayer;
    private Vector3 _originalPosition;
    [SerializeField] private MeshCollider _collider;
    [SerializeField] private Transform _screw;
    [SerializeField] private float _screwRotateSpeed;
    [SerializeField] private float _blocDetectionRange;
    private List<CMYColor> _currentHarvest;
    #endregion

    #region PROPERTIES
    public bool HarvestEnabled { private set; get; }
    #endregion

    #region UNITY METHODS
    private void Awake()
    {
        _originalPosition = transform.position;
    }

    private void Update()
    {
        _screw.Rotate(new Vector3(_screwRotateSpeed * Time.deltaTime, 0f, 0f));

        if (HarvestEnabled)
        {
            Harvest();
        }
    }
    #endregion

    #region METHOD
    private void Harvest()
    {
        CMYColor tempColor = MatrixManager.Instance.HarvestBloc(Mathf.CeilToInt(transform.position.x), Mathf.CeilToInt(transform.position.z));

        if (tempColor != null)
        {
            _currentHarvest.Add(tempColor);
        }
    }
    #endregion

    public void StartHarvest()
    {
        HarvestEnabled = true;
        _collider.enabled = !HarvestEnabled;
        _currentHarvest = new List<CMYColor>();

        transform.DOMoveX(MatrixManager.Instance.GetXWidth() + 3, 2f).OnComplete(() =>
        {
            GameManager.Instance.ProcessHarvest(new List<CMYColor>(_currentHarvest));
            _currentHarvest.Clear();
            transform.DOMove(_originalPosition, 0f);
            HarvestEnabled = false;
            _collider.enabled = !HarvestEnabled;
        });
    }
}
