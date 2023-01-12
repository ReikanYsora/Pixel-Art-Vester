using System.Drawing;
using UnityEngine;

public static class ColorHelper
{
    public static CMYColor Magenta
    {
        get
        {
            return new CMYColor
            {
                c = 0,
                m = 1,
                y = 0
            };
        }
    }

    public static CMYColor Ignore
    {
        get
        {
            return new CMYColor
            {
                c = 0.2f,
                m = 0.3f,
                y = 0.4f
            };
        }
    }

    public static CMYColor Cyan
    {
        get
        {
            return new CMYColor
            {
                c = 1,
                m = 0,
                y = 0
            };
        }
    }

    public static CMYColor Yellow
    {
        get
        {
            return new CMYColor
            {
                c = 0,
                m = 0,
                y = 1
            };
        }
    }

    public static CMYColor Blue
    {
        get
        {
            return new CMYColor
            {
                c = 1,
                m = 1,
                y = 0
            };
        }
    }

    public static CMYColor Red
    {
        get
        {
            return new CMYColor
            {
                c = 0,
                m = 1,
                y = 1
            };
        }
    }

    public static CMYColor Green
    {
        get
        {
            return new CMYColor
            {
                c = 1,
                m = 0,
                y = 1
            };
        }
    }

    public static CMYColor Black
    {
        get
        {
            return new CMYColor
            {
                c = 1,
                m = 1,
                y = 1
            };
        }
    }

    public static CMYColor White
    {
        get
        {
            return new CMYColor
            {
                c = 0,
                m = 0,
                y = 0
            };
        }
    }

    public static CMYColor LightBlue
    {
        get
        {
            return new CMYColor
            {
                c = 1,
                m = 0.5f,
                y = 0
            };
        }
    }

    public static CMYColor Gray
    {
        get
        {
            return new CMYColor
            {
                c = 0.5f,
                m = 0.5f,
                y = 0.5f
            };
        }
    }

    public static CMYColor DarkPurple
    {
        get
        {
            return new CMYColor
            {
                c = 0.5f,
                m = 1,
                y = 0
            };
        }
    }

    public static CMYColor DarkMagenta
    {
        get
        {
            return new CMYColor
            {
                c = 0,
                m = 1,
                y = 0.5f
            };
        }
    }

    public static CMYColor Orange
    {
        get
        {
            return new CMYColor
            {
                c = 0,
                m = 0.5f,
                y = 1
            };
        }
    }

    public static CMYColor LightGreen
    {
        get
        {
            return new CMYColor
            {
                c = 0.5f,
                m = 0,
                y = 1
            };
        }
    }
    public static CMYColor MiddleGreen
    {
        get
        {
            return new CMYColor
            {
                c = 1,
                m = 0,
                y = 0.5f
            };
        }
    }

    public static CMYColor ConvertRGBToCMYColor(UnityEngine.Color color)
    {
        return new CMYColor
        {
            c = 1 - color.r,
            m = 1 - color.g,
            y = 1 - color.b
        };
    }

    public static UnityEngine.Color ConvertCMYColorToRGBColor(CMYColor color)
    {
        return new UnityEngine.Color
        {
            r = 1 - color.c,
            g = 1 - color.m,
            b = 1 - color.y,
            a = 1
        };
    }

    public static bool IsSeparation(CMYColor color)
    {
        return color.c == Ignore.c && color.m == Ignore.m && color.y == Ignore.y;
    }

    public static bool CompareColor(CMYColor color1, CMYColor color2)
    {
        return color1.c == color2.c && color1.m == color2.m && color1.y == color2.y;
    }
}
