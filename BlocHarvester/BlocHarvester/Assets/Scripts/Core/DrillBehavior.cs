using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class DrillBehavior : MonoBehaviour
{
    #region ATTRIBUTES
    [SerializeField] private Transform _screw;
    [SerializeField] private float _screwRotateSpeed;
    [SerializeField] private float _blocDetectionRange;
    private List<CMYColor> _currentHarvest;
    private bool _realMode;
    #endregion

    #region PROPERTIES
    private bool IsInitialized { set; get; }
    #endregion

    #region UNITY METHODS
    private void Update()
    {
        _screw.Rotate(new Vector3(_screwRotateSpeed * Time.deltaTime, 0f, 0f));
    }
    #endregion

    public void StartHarvest(bool realMode)
    {
        _realMode = realMode;

        GameManager.Instance.ProcessHarvest(MatrixManager.Instance.HarvestLine(Mathf.CeilToInt(transform.position.z)));
    }

    public void Initialize()
    {
        IsInitialized = true;
        _currentHarvest = new List<CMYColor>();
    }
}
