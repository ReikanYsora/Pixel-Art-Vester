using UnityEngine;

public class BlocBehavior : MonoBehaviour
{
    #region ATTRIBUTES
    private Material _material;
    #endregion

    #region UNITY METHODS
    private void Awake()
    {
        _material = GetComponent<MeshRenderer>().material;
    }

    private void Start()
    {
        GameManager.Instance.OnTimeTick += CBInstance_OnTimeTick;
    }

    private void CBInstance_OnTimeTick()
    {
        Color tempColor = _material.GetColor("_Color");

        if (tempColor.r > 0f)
        {
            tempColor.r += 0.1f;
            Mathf.Clamp(tempColor.r, 0f, 1f);
        }

        if (tempColor.g > 0f)
        {
            tempColor.g += 0.1f;
            Mathf.Clamp(tempColor.g, 0f, 1f);
        }

        if (tempColor.b > 0f)
        {
            tempColor.b += 0.1f;
            Mathf.Clamp(tempColor.b, 0f, 1f);
        }

        _material.SetColor("_Color", tempColor);
    }

    private void Update()
    {
       
    }
    #endregion

    public void AddColor(ColorType color)
    {
        Color tempColor = _material.GetColor("_Color");

        switch (color)
        {
            case ColorType.Red:

                tempColor.r += GameManager.Instance.GetColorSpeed();
                Mathf.Clamp(tempColor.r, 0f, 1f);
                break;
            case ColorType.Green:
                tempColor.g += GameManager.Instance.GetColorSpeed();
                Mathf.Clamp(tempColor.g, 0f, 1f);
                break;
            case ColorType.Blue:
                tempColor.b += GameManager.Instance.GetColorSpeed();
                Mathf.Clamp(tempColor.b, 0f, 1f);
                break;
        }

        _material.SetColor("_Color", tempColor);
    }
}
