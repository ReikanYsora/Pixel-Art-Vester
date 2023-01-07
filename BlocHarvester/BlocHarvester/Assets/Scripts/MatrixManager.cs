using UnityEngine;

public class MatrixManager : MonoBehaviour
{
	#region ATTRIBUTES
	[SerializeField] private int XWidth;
    [SerializeField] private int YWidth;
    [SerializeField] private GameObject BlocPrefab;
	[SerializeField] private Transform BlocAnchor;
    private GameObject[,] _blocs;
    #endregion

    #region PROPERTIES
    public static MatrixManager Instance { get; private set; }
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

        GenerateMatrix();
    }
    #endregion

    #region METHODS
    private void GenerateMatrix()
	{
        _blocs = new GameObject[XWidth, YWidth];

        for (int x = 0; x < XWidth; x++)
		{
			for (int y = 0; y < YWidth; y++)
			{
				GameObject tempBloc = GameObject.Instantiate(BlocPrefab, new Vector3(x, 0, y), Quaternion.identity);
				tempBloc.transform.SetParent(BlocAnchor);
                _blocs[x, y] = tempBloc;
            }
		}
	}

    public Vector2 GetOrigin()
    {
        return new Vector2(0, 0);
    }

    public int GetXWidth()
    {
        return XWidth;
    }

    public int GetYWidth()
    {
        return YWidth;
    }

    public GameObject GetBloc(int x, int y)
    {
        return _blocs[x, y];
    }
    #endregion
}
