using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region PROPERTIES
    public static GameManager Instance { get; private set; }
    [SerializeField] private bool PauseMode = false;
    [SerializeField] private Texture2D[] _pixelArts;
    private List<PaintInventory> _paintInventory;
    public int _currentPaint;

    public Texture2D PixelArt { set; get; }
    public CMYColor[,] realColors;
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
        InitializeInventory();

        int index = UnityEngine.Random.Range(0, _pixelArts.Length);
        PixelArt = _pixelArts[index];
        realColors = new CMYColor[PixelArt.width, PixelArt.height];

        for (int x = 0; x < PixelArt.width; x++)
        {
            for (int y = 0; y < PixelArt.height; y++)
            {
                realColors[x, y] = new CMYColor(PixelArt.GetPixel(x, y));
            }
        }

        UIManager.Instance.ChangeCurrentPixelArt(PixelArt);
    }
    #endregion

    #region METHODS
    private void InitializeInventory()
    {
        _paintInventory = new List<PaintInventory>
        {
            new PaintInventory
            {
                Color = new CMYColor
                {
                    c = 0,
                    m = 0,
                    y = 0
                },
                Quantity = 0,
                Infinite = true
            },
            new PaintInventory
            {
                Color = new CMYColor
                {
                    c = 1,
                    m = 0,
                    y = 0
                },
                Quantity = 0,
                Infinite = true
            },
            new PaintInventory
            {
                Color = new CMYColor
                {
                    c = 0,
                    m = 1,
                    y = 0
                },
                Quantity = 0,
                Infinite = true
            },
            new PaintInventory
            {
                Color = new CMYColor
                {
                    c = 0,
                    m = 0,
                    y = 1
                },
                Quantity = 0,
                Infinite = true
            }
        };

        _currentPaint = 0;
    }

    private void AddToInventory(CMYColor color)
    {
        PaintInventory tempInventory = _paintInventory.Where(x => x.Color.c == color.c && x.Color.m == color.m && x.Color.y == color.y).FirstOrDefault();

        if (tempInventory == null)
        {
            _paintInventory.Add(new PaintInventory
            {
                Color = color,
                Quantity = 1,
                Infinite = false
            });
        }
        else
        {
            tempInventory.Quantity += 1;
        }
    }

    public void NextPaint()
    {
        if (_currentPaint < _paintInventory.Count - 1)
        {
            _currentPaint++;
        }
        else
        {
            _currentPaint = 0;
        }

        UIManager.Instance.ChangeCurrentPaint(GetCurrentPaint());
    }

    public void PrevPaint()
    {
        if (_currentPaint > 0)
        {
            _currentPaint--;
        }
        else
        {
            _currentPaint = _paintInventory.Count - 1;
        }

        UIManager.Instance.ChangeCurrentPaint(GetCurrentPaint());
    }

    public CMYColor GetCurrentPaint()
    {
        return _paintInventory[_currentPaint].Color;
    }

    public void ProcessHarvest(List<CMYColor> harvest)
    {
        List<CMYColor> finalHarvest = new List<CMYColor>();
        List<List<CMYColor>> totalHarvestSplitted = new List<List<CMYColor>>();
        List<CMYColor> tempHarvest = new List<CMYColor>();

        for (int i = 0; i < harvest.Count; i++)
        {
            CMYColor tempColor = harvest[i];

            if (!ColorHelper.IsWhite(tempColor))
            {
                tempHarvest.Add(tempColor);
            }
            else
            {
                if (tempHarvest.Count >= 2)
                {
                    totalHarvestSplitted.Add(tempHarvest);
                }

                tempHarvest = new List<CMYColor>();
            }
        }

        foreach (List<CMYColor> toCombine in totalHarvestSplitted)
        {
            List<string> toCombineStr = new List<string>();

            foreach (CMYColor tempColor in toCombine)
            {
                toCombineStr.Add(tempColor.ToString());
            }

            //Get a black bloc
            if ((toCombineStr.Count == 3) && toCombineStr.Contains(ColorHelper.Cyan.ToString()) && toCombineStr.Contains(ColorHelper.Magenta.ToString()) && toCombineStr.Contains(ColorHelper.Yellow.ToString()))
            {
                finalHarvest.Add(ColorHelper.Black);

                continue;
            }
            //Get a white bloc
            else if ((toCombineStr.Count == 3) && toCombineStr.Contains(ColorHelper.Red.ToString()) && toCombineStr.Contains(ColorHelper.Green.ToString()) && toCombineStr.Contains(ColorHelper.Blue.ToString()))
            {
                finalHarvest.Add(ColorHelper.White);

                continue;
            }
            else if (toCombineStr.Count == 2)
            {
                //Get a blue bloc
                if (toCombineStr.Contains(ColorHelper.Cyan.ToString()) && toCombineStr.Contains(ColorHelper.Magenta.ToString()))
                {
                    finalHarvest.Add(ColorHelper.Blue);

                    continue;
                }

                //Get a green bloc
                if (toCombineStr.Contains(ColorHelper.Cyan.ToString()) && toCombineStr.Contains(ColorHelper.Yellow.ToString()))
                {
                    finalHarvest.Add(ColorHelper.Green);

                    continue;
                }

                //Get a red bloc
                if (toCombineStr.Contains(ColorHelper.Magenta.ToString()) && toCombineStr.Contains(ColorHelper.Yellow.ToString()))
                {
                    finalHarvest.Add(ColorHelper.Red);

                    continue;
                }

                //Get a Light Blue bloc
                if (toCombineStr.Contains(ColorHelper.Cyan.ToString()) && toCombineStr.Contains(ColorHelper.Blue.ToString()))
                {
                    finalHarvest.Add(ColorHelper.LightBlue);

                    continue;
                }

                //Get a Dark purple bloc
                if (toCombineStr.Contains(ColorHelper.Blue.ToString()) && toCombineStr.Contains(ColorHelper.Magenta.ToString()))
                {
                    finalHarvest.Add(ColorHelper.DarkPurple);

                    continue;
                }

                //Get a Dark magenta bloc
                if (toCombineStr.Contains(ColorHelper.Red.ToString()) && toCombineStr.Contains(ColorHelper.Magenta.ToString()))
                {
                    finalHarvest.Add(ColorHelper.DarkMagenta);

                    continue;
                }

                //Get a orange bloc
                if (toCombineStr.Contains(ColorHelper.Red.ToString()) && toCombineStr.Contains(ColorHelper.Yellow.ToString()))
                {
                    finalHarvest.Add(ColorHelper.Orange);

                    continue;
                }

                //Get a light green bloc
                if (toCombineStr.Contains(ColorHelper.Green.ToString()) && toCombineStr.Contains(ColorHelper.Yellow.ToString()))
                {
                    finalHarvest.Add(ColorHelper.LightGreen);

                    continue;
                }

                //Get a middle green bloc
                if (toCombineStr.Contains(ColorHelper.Green.ToString()) && toCombineStr.Contains(ColorHelper.Cyan.ToString()))
                {
                    finalHarvest.Add(ColorHelper.MiddleGreen);

                    continue;
                }
            }
            else
            {
                continue;
            }            
        }

        foreach (CMYColor color in finalHarvest)
        {
            AddToInventory(color);
        }
    }

    public CMYColor Paint()
    {
        CMYColor tempColor = _paintInventory[_currentPaint].Color;

        if (_paintInventory[_currentPaint].Quantity == 1)
        {
            _paintInventory.RemoveAt(_currentPaint);
        }

        return tempColor;
    }
    #endregion
}
