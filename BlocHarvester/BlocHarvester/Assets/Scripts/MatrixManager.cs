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
                CreateBloc(x, y);
            }
		}
	}

    public void CreateBloc(int X, int Y)
    {
        GameObject tempBloc = GameObject.Instantiate(BlocPrefab, new Vector3(X, 0, Y), Quaternion.identity);
        tempBloc.transform.SetParent(BlocAnchor);
        tempBloc.name = string.Format($"Bloc_{X}_{Y}");

        if (_blocs != null)
        {
            _blocs[X, Y] = tempBloc;
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

    public CMYColor HarvestBloc(int X, int Y)
    {
        if ((X < XWidth) && (Y < YWidth))
        {
            BlocBehavior tempBloc = _blocs[X, Y].GetComponent<BlocBehavior>();

            if ((tempBloc != null) && (tempBloc.IsHarvestable))
            {
                CMYColor tempColor = _blocs[X, Y].GetComponent<BlocBehavior>().Color;
                Destroy(_blocs[X, Y]);
                CreateBloc(X, Y);

                return tempColor;
            }
            else
            {
                return null;
            }
        }
        else
        {
            return null;
        }
    }
    #endregion
}