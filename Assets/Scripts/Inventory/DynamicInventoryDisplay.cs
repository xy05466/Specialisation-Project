using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

public class DynamicInventoryDisplay : InventoryDisplay
{
    [SerializeField] protected InventorySlotUI slotPrefab;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

    }

    private void OnEnable()
    {
        InventoryHolder.OnDynamicInventoryChanged += RefreshDynamicInventory;
    }

    private void OnDisable()
    {
        InventoryHolder.OnDynamicInventoryChanged -= RefreshDynamicInventory;
    }

    private void OnDestroy()
    {
        if (inventorySystem != null)
        {
            inventorySystem.OnInventorySlotChanged -= UpdateSlot;
        }
    }

    public void RefreshDynamicInventory(InventorySystem invToDisplay)
    {
        ClearSlots();
        inventorySystem = invToDisplay;
        AssignSlot(invToDisplay);
    }

    public override void AssignSlot(InventorySystem invToDisplay)
    {
        ClearSlots();

        slotDictionary = new Dictionary<InventorySlotUI, InventorySlot>();
        if (invToDisplay == null )  
            return;
        for (int i = 0;i < invToDisplay.InventorySize; i++)
        {
            var uiSlot = Instantiate(slotPrefab, transform);
            slotDictionary.Add(uiSlot, invToDisplay.InventorySlots[i]);
            uiSlot.Init(invToDisplay.InventorySlots[i]);
            uiSlot.UpdateUISlot();
        }

    }

    private void ClearSlots()
    {
        foreach (var item in transform.Cast<Transform>())
        {
            Destroy(item.gameObject);
        }

        if (slotDictionary != null)
        {
            slotDictionary.Clear();
        }
    }


    public void RemoveItems(InventorySlot slot,int num)
    {
        inventorySystem.RemoveFromInventory(slot.ItemData, num);

            foreach (var item in transform.Cast<Transform>())
            {
                item.GetComponent<InventorySlotUI>().UpdateUISlot();
            }
        
       
    }

    public List<InventorySlot> GetSlots()
    {
        var slots = new List<InventorySlot>();
        foreach (var item in slotDictionary)
        {
            slots.Add(item.Value);
        }
        return slots;
    }
}
