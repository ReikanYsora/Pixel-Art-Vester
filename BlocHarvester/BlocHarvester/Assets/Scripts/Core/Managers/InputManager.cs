using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    #region ATTRIBUTES
    [SerializeField] private LayerMask _drillButton;
    [SerializeField] private LayerMask _bloc;
    [SerializeField] private LayerMask _inventoryBloc;
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
        if (!GameManager.Instance.GameInitialized && Input.GetMouseButton(0))
        {
            GameManager.Instance.StartGame();
        }
        
        if (!GameManager.Instance.Pause && GameManager.Instance.GameInitialized)
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
                    behavior.StartHarvest(true);
                }

                _drillsSelection.Clear();
            }

            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 200, _bloc))
                {
                    BlocBehavior blocBehavior = hit.collider.gameObject.GetComponent<BlocBehavior>();

                    if ((blocBehavior != null) && (blocBehavior.Color.ToString() != GameManager.Instance.GetCurrentPaint().ToString()))
                    {
                        CMYColor tempColor = GameManager.Instance.Paint();

                        if (tempColor != null)
                        {
                            blocBehavior.Color = tempColor;
                        }
                    }
                }

                if (Physics.Raycast(ray, out hit, 200, _inventoryBloc))
                {
                    BlocBehavior blocBehavior = hit.collider.gameObject.GetComponent<BlocBehavior>();

                    if (blocBehavior != null)
                    {
                        GameManager.Instance.SetCurrentPaint(blocBehavior.Color);
                    }
                }
            }
        }
    }
    #endregion
}
