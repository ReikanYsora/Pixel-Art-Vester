using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	#region ATTRIBUTES
	[SerializeField] private Image _pixelArtDisplay;
    [SerializeField] private GameObject _panelLegend;
    [SerializeField] private GameObject _panelQuantity;
    [SerializeField] private Image _paintDisplay;
    [SerializeField] private TMP_Text _paintQuantity;
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private TMP_Text _completedText;
    [SerializeField] private TMP_Text _progressionText;
    [SerializeField] private GameObject _playButton;
    [SerializeField] private GameObject _pauseButton;
    [SerializeField] private GameObject _reloadButton;
    #endregion

    #region PROPERTIES
    public static UIManager Instance { get; private set; }
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
        RefreshUI();
    }
    #endregion

    #region METHODS
    private void RefreshUI()
    {
        if (GameManager.Instance == null || !GameManager.Instance.GameInitialized)
        {
            _playButton.SetActive(false);
            _pauseButton.SetActive(false);
            _reloadButton.SetActive(false);
        }

        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.GameInitialized)
            {
                _playButton.SetActive(GameManager.Instance.Pause);
                _pauseButton.SetActive(!GameManager.Instance.Pause);
                _reloadButton.SetActive(true);
            }
            else
            {
                _playButton.SetActive(false);
                _pauseButton.SetActive(false);
                _reloadButton.SetActive(false);
            }

            PaintInventory tempInventory = GameManager.Instance.GetCurrentPaint();

            if (tempInventory != null)
            {
                if (tempInventory.Infinite)
                {
                    _panelQuantity.SetActive(false);
                }
                else
                {
                    _paintQuantity.text = tempInventory.Quantity.ToString();
                    _panelQuantity.SetActive(true);
                }

                _paintDisplay.color = ColorHelper.ConvertCMYColorToRGBColor(tempInventory.Color);
                _panelLegend.SetActive(true);
            }
            else
            {
                _panelLegend.SetActive(false);
            }

            string tempTime = GameManager.Instance.FormatedTime;

            if (string.IsNullOrEmpty(tempTime))
            {
                _timeText.text = string.Empty;
            }
            else
            {
                _timeText.text = GameManager.Instance.FormatedTime.ToString();
            }

            _completedText.text = string.Format($"{GameManager.Instance.GetScore()} %");
        }
        else
        {
            _panelLegend.SetActive(false);
        }
    }

    public void ChangeCurrentPixelArt(Texture2D pixelArt)
	{
		_pixelArtDisplay.sprite = Sprite.Create(pixelArt, new Rect(0, 0, 16, 16), Vector2.zero);
    }
	#endregion
}
