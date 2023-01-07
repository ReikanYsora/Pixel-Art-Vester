using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class DrillBehavior : MonoBehaviour
{
    #region ATTRIBUTES
    [SerializeField] private LayerMask _blocLayer;
    [SerializeField] private MeshCollider _collider;
    [SerializeField] private Transform _screw;
    [SerializeField] private float _screwRotateSpeed;
    [SerializeField] private float _blocDetectionRange;
    private List<CMYColor> _currentHarvest;
    private bool _realMode;
    #endregion

    #region PROPERTIES
    private bool IsInitialized { set; get; }

    public bool HarvestEnabled
    {
        set
        {
            _collider.enabled = !value;
        }
        get
        {
            return !_collider.enabled;
        }
    }
    #endregion

    #region UNITY METHODS
    private void Awake()
    {
        _collider.enabled = false;
    }

    private void Update()
    {
        _screw.Rotate(new Vector3(_screwRotateSpeed * Time.deltaTime, 0f, 0f));

        if (IsInitialized && HarvestEnabled)
        {
            Harvest();
        }
    }
    #endregion

    #region METHOD
    private void Harvest()
    {
        CMYColor tempColor = MatrixManager.Instance.HarvestBloc(Mathf.CeilToInt(transform.position.x), Mathf.CeilToInt(transform.position.z));

        if ((_realMode) && (tempColor != null))
        {
            _currentHarvest.Add(tempColor);
        }
    }
    #endregion

    public void StartHarvest(bool realMode)
    {
        HarvestEnabled = true;
        _realMode = realMode;

        Vector3 transformPosition = transform.position;

        transform.DOMoveX(20f, 2f).OnComplete(() =>
        {
            GameManager.Instance.ProcessHarvest(new List<CMYColor>(_currentHarvest));
            DrillManager.Instance.CreateNewDrillAtLine((int)transformPosition.z);
            Destroy(gameObject);
        });
    }

    public void Initialize()
    {
        IsInitialized = true;
        _currentHarvest = new List<CMYColor>();
        HarvestEnabled = false;
    }
}
