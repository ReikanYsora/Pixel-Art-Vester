using UnityEngine;

public class DrillManager : MonoBehaviour
{
    #region ATTRIBUTES
    [SerializeField] private Transform _drillsAnchor;
    [SerializeField] private GameObject _drillPrefab;
    [SerializeField] private float _offset;
    private GameObject[] _drills;
    #endregion

    #region UNITY METHODS
    private void Start()
    {
        int yWidth = MatrixManager.Instance.GetYWidth();
        _drills = new GameObject[yWidth];

        for (int y = 0; y < yWidth; y++)
        {
            GameObject tempDrill = GameObject.Instantiate(_drillPrefab, new Vector3(-_offset, 0f, y), Quaternion.identity);
            tempDrill.transform.SetParent(_drillsAnchor);

            _drills[y] = tempDrill;
        }
    }
    #endregion

}
