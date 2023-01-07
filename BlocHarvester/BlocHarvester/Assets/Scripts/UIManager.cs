using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	#region ATTRIBUTES
	[SerializeField] private Image _pixelArtDisplay;
    [SerializeField] private Image _currentPaintDisplay;
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
    #endregion

    #region METHODS
    public void ChangeCurrentPixelArt(Texture2D pixelArt)
	{
		_pixelArtDisplay.sprite = Sprite.Create(pixelArt, new Rect(0, 0, 16, 16), Vector2.zero);
    }

    public void ChangeCurrentPaint(CMYColor color)
    {
        UnityEngine.Color tempColor = ColorHelper.ConvertCMYColorToRGBColor(color);
        tempColor.a = 1;
        _currentPaintDisplay.color = tempColor;
    }
	#endregion
}
