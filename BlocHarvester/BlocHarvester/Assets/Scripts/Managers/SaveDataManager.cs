using System;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataManager : MonoBehaviour
{
    #region PROPERTIES
    public static SaveDataManager Instance { get; private set; }
    public List<PaintInventory> Inventory;
    public List<string> CompletedPuzzles;
    public double ProgressTime;
    #endregion

    #region UNITY METHODS
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (Instance != null && Instance != this)
        {
            Inventory = Instance.Inventory;
            CompletedPuzzles = Instance.CompletedPuzzles;

            Destroy(this);
            return;
        }

        Instance = this;
    }

    public void SaveTime(double playTime)
    {
        ProgressTime = playTime;
    }
    #endregion

    #region METHODS
    #endregion
}
