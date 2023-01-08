using System.Collections.Generic;
using UnityEngine;

public class DrillBehavior : MonoBehaviour
{
    #region ATTRIBUTES
    private List<CMYColor> _currentHarvest;
    private bool _realMode;
    #endregion

    public void StartHarvest(bool realMode)
    {
        _realMode = realMode;
        GameManager.Instance.ProcessHarvest(MatrixManager.Instance.HarvestLine(Mathf.CeilToInt(transform.position.z)));
    }

    public void Initialize()
    {
        _currentHarvest = new List<CMYColor>();
    }
}
