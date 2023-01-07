using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region ENUMS
    public enum Direction
    {
        Up, Down, Left, Right
    }
    #endregion

    #region PROPERTIES
    public static PlayerManager Instance { get; private set; }
    #endregion

    #region EVENTS
    public delegate void DirectionChanged(Direction direction);
    public event DirectionChanged OnDirectionChanged;
    #endregion

    #region ATTRIBUTES
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private float _offsetY;
	private GameObject _playerInstance;
    [SerializeField] private ColorType _currentColor;


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
        _currentColor = ColorType.None;
        SpawnPlayer();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MovePlayerLeft();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MovePlayerRight();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
             MovePlayerUp();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MovePlayerDown();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Paint();
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            _currentColor = ColorType.Red;
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            _currentColor = ColorType.Green;
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            _currentColor = ColorType.Blue;
        }
    }
    #endregion

    #region METHODS
    private void SpawnPlayer()
    {
        Vector2 boundaries = MatrixManager.Instance.GetOrigin();
        Vector3 playerOriginalPosition = new Vector3(boundaries.x, _offsetY, boundaries.y);
        _playerInstance = GameObject.Instantiate(_playerPrefab, playerOriginalPosition, Quaternion.identity);
    }

    private void MovePlayerUp()
    {
        if (_playerInstance.transform.position.z < MatrixManager.Instance.GetYWidth() - 1)
        {
            Vector3 tempPosition = _playerInstance.transform.position;
            tempPosition.z += 1;
            _playerInstance.transform.position = tempPosition;

            OnDirectionChanged?.Invoke(Direction.Up);
        }
    }

    private void MovePlayerDown()
    {
        if (_playerInstance.transform.position.z > 0)
        {
            Vector3 tempPosition = _playerInstance.transform.position;
            tempPosition.z -= 1;
            _playerInstance.transform.position = tempPosition;

            OnDirectionChanged?.Invoke(Direction.Down);
        }
    }

    private void MovePlayerRight()
    {
        if (_playerInstance.transform.position.x < MatrixManager.Instance.GetXWidth() - 1)
        {
            Vector3 tempPosition = _playerInstance.transform.position;
            tempPosition.x += 1;
            _playerInstance.transform.position = tempPosition;

            OnDirectionChanged?.Invoke(Direction.Right);
        }
    }

    private void MovePlayerLeft()
    {
        if (_playerInstance.transform.position.x > 0)
        {
            Vector3 tempPosition = _playerInstance.transform.position;
            tempPosition.x -= 1;
            _playerInstance.transform.position = tempPosition;

            OnDirectionChanged?.Invoke(Direction.Left);
        }
    }

    private void Paint()
    {
        GameObject tempBloc = MatrixManager.Instance.GetBloc((int)_playerInstance.transform.position.x, (int) _playerInstance.transform.position.z);

        if (tempBloc != null && _currentColor != ColorType.None)
        {
            tempBloc.GetComponent<BlocBehavior>().AddColor(_currentColor);
        }
    }
    #endregion
}
