using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region PROPERTIES
    public static GameManager Instance { get; private set; }
    [SerializeField] private Texture2D[] _pixelArts;
    public int _currentPaint;
    private GameObject[] _blocInventory;
    [SerializeField] private GameObject _inventoryBloc;
    [SerializeField] private GameObject _inventoryLegendBloc;
    [SerializeField] private Transform _inventoryBlocAnchor;
    private double _playTime;

    public Texture2D PixelArt { set; get; }
    public CMYColor[,] realColors;
    public bool Pause { set; get; }
    public bool GameInitialized { set; get; }

    public string FormatedTime { private set; get; }
    #endregion

    #region UNITY METHODS
    private void Awake()
    {
        GameInitialized = false;
        Pause = false;

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

        for (int x = 0; x < PixelArt.width; x++)
        {
            for (int y = 0; y < PixelArt.height; y++)
            {
                GameObject tempBloc = MatrixManager.Instance.GetBloc(x, y);
                tempBloc.GetComponent<BlocBehavior>().Color = realColors[x, y];
            }
        }

        UIManager.Instance.ChangeCurrentPixelArt(PixelArt);
    }

    private void Update()
    {
        if ((!Pause) && (GameInitialized))
        {            
            _playTime += Time.deltaTime;
            TimeSpan tempSpan = TimeSpan.FromSeconds(_playTime);
            FormatedTime = tempSpan.Hours.ToString("00") + ":" + tempSpan.Minutes.ToString("00") + ":" + tempSpan.Seconds.ToString("00");
        }
    }

    public int GetScore()
    {
        float tempResult = MatrixManager.Instance.GetScore(realColors) * 100f;
        return Mathf.FloorToInt(tempResult);
    }

    public void ResetGame()
    {
        if (GameInitialized)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void PauseOrResume()
    {
        if (GameInitialized)
        {
            Pause = !Pause;
        }
    }

    public void StartGame()
    {
        DrillManager.Instance.DrillForStart();
        GameInitialized = true;
    }
    #endregion

    #region METHODS
    private void InitializeInventory()
    {
        if (SaveDataManager.Instance.Inventory == null)
        {
            SaveDataManager.Instance.Inventory = new List<PaintInventory>
            {
                //White
                new PaintInventory
                {
                    Index = 0,
                    Color = ColorHelper.White,
                    Quantity = 0,
                    Infinite = true,
                    ToCreate = new List<CMYColor>()
                },
                //Cyan
                new PaintInventory
                {
                    Index = 1,
                    Color = ColorHelper.Cyan,
                    Quantity = 0,
                    Infinite = true,
                    ToCreate = new List<CMYColor>()
                },
                //Light blue
                new PaintInventory
                {
                    Index = 2,
                    Color = ColorHelper.LightBlue,
                    Quantity = 0,
                    Infinite = false,
                    ToCreate = new List<CMYColor>
                    {
                        ColorHelper.Cyan, ColorHelper.Blue
                    }
                },
                //Blue
                new PaintInventory
                {
                    Index = 3,
                    Color = ColorHelper.Blue,
                    Quantity = 0,
                    Infinite = false,
                    ToCreate = new List<CMYColor>
                    {
                        ColorHelper.Cyan, ColorHelper.Magenta
                    }
                },
                //Dark purple
                new PaintInventory
                {
                    Index = 4,
                    Color = ColorHelper.DarkPurple,
                    Quantity = 0,
                    Infinite = false,
                    ToCreate = new List<CMYColor>
                    {
                        ColorHelper.Blue, ColorHelper.Magenta
                    }
                },
                //Magenta
                new PaintInventory
                {
                    Index = 5,
                    Color = ColorHelper.Magenta,
                    Quantity = 0,
                    Infinite = true,
                    ToCreate = new List<CMYColor>()
                },
                //Dark magenta
                new PaintInventory
                {
                    Index = 6,
                    Color = ColorHelper.DarkMagenta,
                    Quantity = 0,
                    Infinite = false,
                    ToCreate = new List<CMYColor>
                    {
                        ColorHelper.Magenta, ColorHelper.Red
                    }
                },
                //Red
                new PaintInventory
                {
                    Index = 7,
                    Color = ColorHelper.Red,
                    Quantity = 0,
                    Infinite = false,
                    ToCreate = new List<CMYColor>
                    {
                        ColorHelper.Magenta, ColorHelper.Yellow
                    }
                },
                //orange
                new PaintInventory
                {
                    Index = 8,
                    Color = ColorHelper.Orange,
                    Quantity = 0,
                    Infinite = false,
                    ToCreate = new List<CMYColor>
                    {
                        ColorHelper.Red, ColorHelper.Yellow
                    }
                },
                //Yellow
                new PaintInventory
                {
                    Index = 9,
                    Color = ColorHelper.Yellow,
                    Quantity = 0,
                    Infinite = true,
                    ToCreate = new List<CMYColor>()
                },
                //Light green
                new PaintInventory
                {
                    Index = 10,
                    Color = ColorHelper.LightGreen,
                    Quantity = 0,
                    Infinite = false,
                    ToCreate = new List<CMYColor>
                    {
                        ColorHelper.Yellow, ColorHelper.Green
                    }
                },
                //Green
                new PaintInventory
                {
                    Index = 11,
                    Color = ColorHelper.Green,
                    Quantity = 0,
                    Infinite = false,
                    ToCreate = new List<CMYColor>
                    {
                        ColorHelper.Yellow, ColorHelper.Cyan
                    }
                },
                //Middle green
                new PaintInventory
                {
                    Index = 12,
                    Color = ColorHelper.MiddleGreen,
                    Quantity = 0,
                    Infinite = false,
                    ToCreate = new List<CMYColor>
                    {
                        ColorHelper.Green, ColorHelper.Cyan
                    }
                },
                //Black
                new PaintInventory
                {
                    Index = 13,
                    Color = ColorHelper.Black,
                    Quantity = 0,
                    Infinite = false,
                    ToCreate = new List<CMYColor>
                    {
                        ColorHelper.Cyan, ColorHelper.Magenta, ColorHelper.Yellow
                    }
                },
                //Gray
                new PaintInventory
                {
                    Index = 14,
                    Color = ColorHelper.Gray,
                    Quantity = 0,
                    Infinite = false,
                    ToCreate = new List<CMYColor>
                    {
                        ColorHelper.White, ColorHelper.Black
                    }
                }
            };
        }

        _blocInventory = new GameObject[15];

        for (int i = 0; i < SaveDataManager.Instance.Inventory.Count; i++)
        {
            GameObject tempBloc = GameObject.Instantiate(_inventoryBloc, new Vector3(-3.5f, 0, i), Quaternion.identity);
            tempBloc.GetComponent<BlocBehavior>().Color = SaveDataManager.Instance.Inventory[i].Color;

            if (SaveDataManager.Instance.Inventory[i].Infinite)
            {
                tempBloc.GetComponent<BlocBehavior>().SetQuantity(-1);
            }
            else
            {
                tempBloc.GetComponent<BlocBehavior>().SetQuantity(SaveDataManager.Instance.Inventory[i].Quantity);
            }

            tempBloc.transform.parent = _inventoryBlocAnchor;

            _blocInventory[i] = tempBloc;

            for (int j = 0; j < SaveDataManager.Instance.Inventory[i].ToCreate.Count; j++)
            {
                GameObject tempLegend = GameObject.Instantiate(_inventoryLegendBloc, new Vector3(-5f -j, 0, i), Quaternion.identity);
                tempLegend.GetComponent<BlocBehavior>().Color = SaveDataManager.Instance.Inventory[i].ToCreate[j];
                tempLegend.transform.parent = _inventoryBlocAnchor;
            }
        }

        _currentPaint = 0;
    }

    private void AddToInventory(CMYColor color)
    {
        PaintInventory tempInventory = SaveDataManager.Instance.Inventory.Where(x => x.Color.c == color.c && x.Color.m == color.m && x.Color.y == color.y).FirstOrDefault();

        if (tempInventory != null)
        {
            tempInventory.Quantity += 1;
            _blocInventory[tempInventory.Index].GetComponent<BlocBehavior>().SetQuantity(tempInventory.Quantity);
        }
    }

    public void SetCurrentPaint(CMYColor color)
    {
        for (int i = 0; i < SaveDataManager.Instance.Inventory.Count; i++)
        {
            if (SaveDataManager.Instance.Inventory[i].Color.ToString() == color.ToString())
            {
                _currentPaint = i;
                break;
            }
        }
    }

    public PaintInventory GetCurrentPaint()
    {
        return SaveDataManager.Instance.Inventory[_currentPaint];
    }

    public CMYColor GetCurrentPaintColor()
    {
        return SaveDataManager.Instance.Inventory[_currentPaint].Color;
    }

    public void ProcessHarvest(List<CMYColor> harvest)
    {
        List<CMYColor> finalHarvest = new List<CMYColor>();
        List<List<CMYColor>> totalHarvestSplitted = new List<List<CMYColor>>();
        List<CMYColor> tempHarvest = new List<CMYColor>();

        for (int i = 0; i < harvest.Count; i++)
        {
            CMYColor tempColor = harvest[i];

            if (!ColorHelper.IsSeparation(tempColor))
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
                //Get a gray bloc
                if (toCombineStr.Contains(ColorHelper.Black.ToString()) && toCombineStr.Contains(ColorHelper.White.ToString()))
                {
                    finalHarvest.Add(ColorHelper.Gray);

                    continue;
                }

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
        CMYColor tempColor = SaveDataManager.Instance.Inventory[_currentPaint].Color;

        if (SaveDataManager.Instance.Inventory[_currentPaint].Infinite)
        {
            return tempColor;

        }
        else if (SaveDataManager.Instance.Inventory[_currentPaint].Quantity > 0)
        {
            SaveDataManager.Instance.Inventory[_currentPaint].Quantity -= 1;
            _blocInventory[_currentPaint].GetComponent<BlocBehavior>().SetQuantity(SaveDataManager.Instance.Inventory[_currentPaint].Quantity);

            return tempColor;
        }
        else
        {
            return null;
        }
    }
    #endregion
}
