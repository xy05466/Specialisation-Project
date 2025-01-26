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
        var invSaveData = new InventorySaveData(inventorySystem, transform.position, transform.rotation);
        SaveGameManager.data.playerInventory = invSaveData;
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
        }
    }
}

[System.Serializable]
public struct InventorySaveData
{
    public InventorySystem invSystem;
    public Vector3 position;
    public Quaternion rotation;

    public InventorySaveData(InventorySystem _invSystem, Vector3 _position, Quaternion _rotation)
    {
        invSystem = _invSystem;
        position = _position;
        rotation = _rotation;
    }

    public InventorySaveData(InventorySystem _invSystem)
    {
        invSystem = _invSystem;
        position = Vector3.zero;
        rotation = Quaternion.identity;
    }
}
