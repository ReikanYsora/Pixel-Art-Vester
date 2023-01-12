using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region ATTRIBUTES
    [Header("Panels")]
	[SerializeField] private Image _pixelArtDisplay;
    [SerializeField] private GameObject _panelLegend;
    [SerializeField] private GameObject _panelQuantity;
    [SerializeField] private Image _paintDisplay;

    [Header("UI texts")]
    [SerializeField] private TMP_Text _paintQuantity;
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private TMP_Text _completedText;
    [SerializeField] private TMP_Text _progressionText;

    [Header("Action button")]
    [SerializeField] private GameObject _playButton;
    [SerializeField] private GameObject _pauseButton;
    [SerializeField] private GameObject _reloadButton;

    [Header("Mouse icons")]
    [SerializeField] private Texture2D _normalIconPaint;
    [SerializeField] private Texture2D _paintIconPaint;
    [SerializeField] private Texture2D _bombIconPaint;
    [SerializeField] private Texture2D _inventoryIconPaint;
    [SerializeField] private Texture2D _crossIconPaint;
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
        ChangeMouseIcon();
        RefreshUI();
    }
    #endregion

    #region METHODS
    private void ChangeMouseIcon()
    {
        switch (InputManager.Instance.HoverBloc)
        {
            default:
            case InputManager.HoveredType.Nothing:
                Cursor.SetCursor(_normalIconPaint, Vector2.zero, CursorMode.ForceSoftware);
                break;
            case InputManager.HoveredType.Bloc:
                Cursor.SetCursor(_paintIconPaint, new Vector2(0f, 32f), CursorMode.ForceSoftware);
                break;
            case InputManager.HoveredType.InventoryMissing:
                Cursor.SetCursor(_crossIconPaint, new Vector2(0f, 32f), CursorMode.ForceSoftware);
                break;
            case InputManager.HoveredType.Inventory:
                Cursor.SetCursor(_inventoryIconPaint, new Vector2(0f, 32f), CursorMode.ForceSoftware);
                break;
            case InputManager.HoveredType.Bomb:
                Cursor.SetCursor(_bombIconPaint, Vector2.zero, CursorMode.ForceSoftware);
                break;
        }
    }

    private void RefreshUI()
    {
        if (SaveDataManager.Instance != null)
        {
            _completedText.text = SaveDataManager.Instance.CompletedPuzzles.Count.ToString();
        }

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

            _progressionText.text = string.Format($"{GameManager.Instance.CurrentScore} %");
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
