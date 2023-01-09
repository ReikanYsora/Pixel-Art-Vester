using System.Collections.Generic;
using UnityEngine;

public class DrillBehavior : MonoBehaviour
{
    #region ATTRIBUTES
    private Animator _animator;
    private List<CMYColor> _currentHarvest;
    private bool _realMode;
    #endregion

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void StartHarvest(bool realMode)
    {
        _animator.SetTrigger("Explode");
        _realMode = realMode;
        GameManager.Instance.ProcessHarvest(MatrixManager.Instance.HarvestLine(Mathf.CeilToInt(transform.position.z)));
    }

    public void Initialize()
    {
        _currentHarvest = new List<CMYColor>();
    }
}
