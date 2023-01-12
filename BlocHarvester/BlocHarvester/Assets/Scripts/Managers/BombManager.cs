using UnityEngine;

public class BombManager : MonoBehaviour
{
    #region ATTRIBUTES
    [SerializeField] private Transform _bombAnchor;
    [SerializeField] private GameObject _bombPrefab;
    [SerializeField] private float _offset;
    private GameObject[] _bombs;
    #endregion

    #region PROPERTIES
    public static BombManager Instance { get; private set; }
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

    private void Start()
    {
        int yWidth = MatrixManager.Instance.GetYWidth();
        _bombs = new GameObject[yWidth];

        for (int y = 0; y < yWidth; y++)
        {
            GameObject tempDrill = GameObject.Instantiate(_bombPrefab, new Vector3(-_offset, 0f, y), Quaternion.identity);
            tempDrill.transform.SetParent(_bombAnchor);

            _bombs[y] = tempDrill;
        }
    }
    #endregion

    #region METHODS
    public void ExplodeAll()
    {
        foreach (GameObject tempBomb in _bombs)
        {
            tempBomb.GetComponent<BombBehavior>().StartHarvest(false);
        }
    }
    #endregion
}
