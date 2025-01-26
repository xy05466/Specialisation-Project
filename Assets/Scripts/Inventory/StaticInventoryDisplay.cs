using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticInventoryDisplay : InventoryDisplay
{
    [SerializeField] private InventoryHolder inventoryHolder;
    [SerializeField] private InventorySlotUI[] slots;

    private void OnEnable()
    {
        //if (inventoryHolder.InventorySystem.InventorySize == 0)
        //{
        //    inventorynventoryHolder;
        //    //inventoryHolder = go.transform.GetChild(0).GetComponent<InventoryHolder>();
        //}
        InventoryHolder.OnPlayerInventoryChanged += RefreshStaticDisplay;
    }

    private void OnDisable()
    {
        InventoryHolder.OnPlayerInventoryChanged -= RefreshStaticDisplay;
    }

    protected override void Start()
    {
        base.Start();
        RefreshStaticDisplay();
    }

    private void RefreshStaticDisplay()
    {
        if (inventoryHolder != null)
        {
            inventorySystem = inventoryHolder.InventorySystem;
            inventorySystem.OnInventorySlotChanged += UpdateSlot;
        }
        else Debug.LogWarning($"No inventory assigned to {this.gameObject}");
        AssignSlot(inventorySystem);
    }

    public override void AssignSlot(InventorySystem invToDisplay)
    {
        slotDictionary = new Dictionary<InventorySlotUI, InventorySlot>();
        if (slots.Length != inventorySystem.InventorySize)
        {
            Debug.Log($"Inventory slots out of sync on {this.gameObject}");
        }
        for (int i = 0; i < inventorySystem.InventorySize; i++) 
        {
            slotDictionary.Add(slots[i], inventorySystem.InventorySlots[i]);
            slots[i].Init(inventorySystem.InventorySlots[i]);
        }
    }

    
}
