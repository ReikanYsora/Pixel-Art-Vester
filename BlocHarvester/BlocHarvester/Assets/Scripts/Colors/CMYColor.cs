using System;
using UnityEngine;

public class CMYColor
{
    #region PROPERTIES
    public float c { set; get; }
    public float m { set; get; }
    public float y { set; get; }
    #endregion

    #region METHODS
    public CMYColor(Color color)
    {
        CMYColor tempColor =  ColorHelper.ConvertRGBToCMYColor(color);

        c = (float) Math.Round(tempColor.c, 1);
        m = (float) Math.Round(tempColor.m, 1);
        y = (float) Math.Round(tempColor.y, 1);
    }

    public CMYColor()
    {
        c = 0;
        m = 0;
        y = 0;
    }

    public override string ToString()
    {
        return string.Format($"{c}_{m}_{y}");
    }
    #endregion
}
