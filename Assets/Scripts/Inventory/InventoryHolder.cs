using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

[System.Serializable]

public class InventoryHolder : MonoBehaviour
{
    [SerializeField] private int inventorySize;
    [SerializeField] protected InventorySystem inventorySystem;

    public InventorySystem InventorySystem => inventorySystem;
    public static UnityAction<InventorySystem> OnDynamicInventoryDisplayRequested;
    public static UnityAction<InventorySystem> OnHealDisplayRequested;
    public static UnityAction OnDynamicInventoryDisplayDisabled;
    public static UnityAction<InventorySystem> OnHealDisplayDisabled;
    public static UnityAction<InventorySystem> OnDynamicInventoryChanged;
    public static UnityAction OnPlayerInventoryChanged;
    public static UnityAction<SaveData> onSaveDataRequested;


    private void Start()
    {
        Debug.Log(this.gameObject.name);
        var invSaveData = new InventorySaveData(inventorySystem);
        SaveGameManager.data.playerInventory = invSaveData;
    }

    private void OnEnable()
    {
        OnPlayerInventoryChanged?.Invoke();
    }

    protected virtual void Awake()
    {
        inventorySystem = new InventorySystem(inventorySize);
        SaveLoad.OnLoadGame += LoadInventory;
    }
    protected virtual void LoadInventory(SaveData data)
    {
        if (data.playerInventory.invSystem != null)
        {
            this.inventorySystem = data.playerInventory.invSystem;
            OnPlayerInventoryChanged?.Invoke();
            Debug.Log("Inventory loaded");
        }
    }

    private void OnDisable()
    {
        SaveLoad.OnLoadGame -= LoadInventory;
        Debug.Log("Inventory destroyed");
    }
}

[System.Serializable]
public struct InventorySaveData
{
    public InventorySystem invSystem;

    public InventorySaveData(InventorySystem _invSystem)
    {
        invSystem = _invSystem;
    }
}
