using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region CONSTANTS
    [SerializeField] private float _colorSpeed = 0.2f;
    [SerializeField] private float _timeToDestroy = 2f;
    #endregion

    #region EVENTS
    public delegate void TimeTick();
    public event TimeTick OnTimeTick;
    #endregion

    #region PROPERTIES
    public static GameManager Instance { get; private set; }
    [SerializeField] private bool PauseMode = false;
    private float _currentTime;
    [SerializeField] private float _tickDelay;
    [SerializeField] private Texture2D[] _pixelArts;

    public Texture2D PixelArt { set; get; }
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
        int index = UnityEngine.Random.Range(0, _pixelArts.Length);
        PixelArt = _pixelArts[index];

        UIManager.Instance.ChangeCurrentPixelArt(PixelArt);
    }

    private void Update()
    {
        if (!PauseMode)
        {
            if (_currentTime < _tickDelay)
            {
                _currentTime += Time.deltaTime;
            }
            else
            {
                _currentTime = 0f;
                OnTimeTick?.Invoke();
            }
        }
    }

    public float GetColorSpeed()
    {
        return _colorSpeed;
    }
    #endregion
}
