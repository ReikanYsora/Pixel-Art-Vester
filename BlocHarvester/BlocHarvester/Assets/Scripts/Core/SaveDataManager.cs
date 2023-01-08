using System.Collections.Generic;
using UnityEngine;

public class SaveDataManager : MonoBehaviour
{
    #region PROPERTIES
    public static SaveDataManager Instance { get; private set; }
    public List<PaintInventory> Inventory;
    #endregion

    #region UNITY METHODS
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (Instance != null && Instance != this)
        {
            Inventory = Instance.Inventory;

            Destroy(this);
            return;
        }

        Instance = this;
    }
    #endregion

    #region METHODS
    #endregion
}
