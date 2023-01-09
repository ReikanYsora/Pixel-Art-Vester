using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    #region ENUMS
    public enum HoveredType { Nothing, Bloc, Inventory, Bomb }
    #endregion


    #region ATTRIBUTES
    [SerializeField] private LayerMask _bomb;
    [SerializeField] private LayerMask _bloc;
    [SerializeField] private LayerMask _inventoryBloc;
    private GameObject _hoveredGameobject;
    #endregion

    #region PROPERTIES
    public static InputManager Instance { get; private set; }

    public HoveredType HoverBloc { get; private set; }
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
    }

    private void Update()
    {
        if (!GameManager.Instance.GameInitialized && Input.GetMouseButton(0))
        {
            GameManager.Instance.StartGame();
        }

        if (!GameManager.Instance.Pause && GameManager.Instance.GameInitialized)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


            if (Physics.Raycast(ray, 200, _bloc))
            {
                HoverBloc = HoveredType.Bloc;
            }
            else if (Physics.Raycast(ray, 200, _inventoryBloc))
            {
                HoverBloc = HoveredType.Inventory;
            }
            else if (Physics.Raycast(ray, 200, _bomb))
            {
                HoverBloc = HoveredType.Bomb;
            }
            else
            {
                HoverBloc = HoveredType.Nothing;
            }


            if (Input.GetMouseButton(0))
            {
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 200, _bloc))
                {
                    BlocBehavior blocBehavior = hit.collider.gameObject.GetComponent<BlocBehavior>();

                    if ((blocBehavior != null) && (blocBehavior.Color.ToString() != GameManager.Instance.GetCurrentPaintColor().ToString()))
                    {
                        CMYColor tempColor = GameManager.Instance.Paint();

                        if (tempColor != null)
                        {
                            blocBehavior.Color = tempColor;
                        }
                    }
                }
                else if (Physics.Raycast(ray, out hit, 200, _inventoryBloc))
                {
                    BlocBehavior blocBehavior = hit.collider.gameObject.GetComponent<BlocBehavior>();

                    if (blocBehavior != null)
                    {
                        GameManager.Instance.SetCurrentPaint(blocBehavior.Color);
                    }
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 200, _bomb))
                {
                    DrillBehavior behavior = hit.transform.gameObject.GetComponentInParent<DrillBehavior>();
                    behavior.StartHarvest(true);
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 200, _bloc))
                {
                    MatrixManager.Instance.DestroyBloc(hit.collider.gameObject);
                }
            }
        }
    }
    #endregion
}
