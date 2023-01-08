using TMPro;
using UnityEngine;

public class BlocBehavior : MonoBehaviour
{
    #region ATTRIBUTES
    private Material _material;
    private CMYColor _color;
    public bool IsHarvestable;
    public TMP_Text _quantityText;
    #endregion

    #region PROPERTIES
    public CMYColor Color
    {
        set
        {
            _color = value;

            Color tempColor = _material.GetColor("_Color");
            _material.SetColor("_Color", ColorHelper.ConvertCMYColorToRGBColor(_color));
        }
        get
        {
            return _color;
        }
    }
    #endregion

    #region UNITY METHODS
    private void Awake()
    {
        _material = GetComponent<MeshRenderer>().material;
        Color = ColorHelper.Ignore;
    }

    private void Update()
    {
       
    }
    #endregion

    #region METHODS
    public void Initialize()
    {
        IsHarvestable = true;
    }

    public void SetQuantity(int quantity)
    {
        if (_quantityText != null)
        {
            if (quantity == -1)
            {
                _quantityText.text = string.Empty;
            }
            else
            {
                _quantityText.text = quantity.ToString();
            }
        }
    }
    #endregion
}
