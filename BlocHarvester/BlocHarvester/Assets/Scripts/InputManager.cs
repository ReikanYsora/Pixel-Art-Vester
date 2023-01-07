using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    #region ATTRIBUTES
    [SerializeField] private LayerMask _drillButton;
    [SerializeField] private LayerMask _bloc;
    private List<GameObject> _drillsSelection;
    #endregion

    #region PROPERTIES
    public static InputManager Instance { get; private set; }
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

        _drillsSelection = new List<GameObject>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 200, _drillButton))
            {
                if (!_drillsSelection.Contains(hit.transform.gameObject))
                {
                    _drillsSelection.Add(hit.transform.gameObject);
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            foreach (GameObject tempDrill in _drillsSelection)
            {
                DrillBehavior behavior = tempDrill.GetComponentInParent<DrillBehavior>();
                behavior.StartHarvest();
            }

            _drillsSelection.Clear();
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 200, _bloc))
            {
                if (hit.collider.gameObject.GetComponent<BlocBehavior>() != null)
                {
                    hit.collider.gameObject.GetComponent<BlocBehavior>().Color = GameManager.Instance.Paint();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.PageDown))
        {
            GameManager.Instance.NextPaint();
        }

        if (Input.GetKeyDown(KeyCode.PageUp))
        {
            GameManager.Instance.PrevPaint();
        }
    }
    #endregion
}
