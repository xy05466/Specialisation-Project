using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    public InventoryItemData itemData;
    public int amount = 1;
    [SerializeField] private ItemPickUpSaveData itemSaveData;
    private string id;

    private void Awake()
    {
        id= GetComponent<UniqueID>().ID;
        SaveLoad.OnLoadGame += LoadGame;
        SaveLoad.OnTempLoadGame += LoadGame;
        itemSaveData = new ItemPickUpSaveData(itemData);
    }

    private void LoadGame(SaveData saveData)
    {
        if (saveData.collectedItems.Contains(id))
        {

                Destroy(this.gameObject);
            
           
        }
    }

    private void LoadGame(TempSaveData saveData)
    {
        if (saveData.collectedItems.Contains(id))
        {

            Destroy(this.gameObject);


        }
    }

    private void OnDestroy()
    {
        if (SaveGameManager.data.activeItems.ContainsKey(id))
        {
            SaveGameManager.data.activeItems.Remove(id);
        }
        SaveLoad.OnLoadGame -= LoadGame;
        SaveLoad.OnTempLoadGame -= LoadGame;
    }

    private void Start()
    {
        SaveGameManager.data.activeItems.Add(id, itemSaveData);
    }

    private void OnTriggerEnter(Collider other)
    {
        var inventory = other.transform.GetComponentInChildren<InventoryHolder>();
        if (!inventory) return;
        var usableItem = GetComponent<IUsableItems>();
        if (usableItem != null)
        {
            itemData.AssignUse(usableItem);
            itemData.AssignEnd(usableItem);
        }
        
        if (inventory.InventorySystem.AddToInventory(itemData, amount))
        {
            SaveGameManager.data.collectedItems.Add(id);
            SaveGameManager.tempData.collectedItems.Add(id);
            Destroy(this.gameObject);
        }
    }
}

[System.Serializable]
public struct ItemPickUpSaveData
{
    public InventoryItemData inventoryItemData;

    public ItemPickUpSaveData(InventoryItemData _inventoryItemData)
    {
        inventoryItemData = _inventoryItemData; 

    }
}