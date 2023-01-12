using TMPro;
using UnityEngine;

public class InventoryBlocBehavior : MonoBehaviour
{

    #region ATTRIBUTES
    private Material _material;
    private CMYColor _color;
    public TMP_Text _quantityText;
    private Animator _animator;
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
        _animator = GetComponent<Animator>();
        Color = ColorHelper.Ignore;
    }
    #endregion

    #region METHODS
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

    public void Initialize()
    {
        
    }

    public void StartFX()
    {
        Vector3 position = transform.position;
        position.y += 1;
        SFXManager.Instance.PlayInventoryAddEffect(position);
        _animator.SetTrigger("Shake");
    }
    #endregion
}
