using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    [SerializeField] private InventoryItemData itemData;
    [SerializeField] private int stackSize;
    // Start is called before the first frame update

    public InventoryItemData ItemData => itemData;
    public int StackSize => stackSize;
    public Action itemEffect;
    public Action itemEnd;

    // constructor to make occupied inventory slot
    public InventorySlot(InventoryItemData source, int amount)
    {
        itemData = source;
        stackSize = amount;
    }

    //constructor to make empty slot
    public InventorySlot()
    {
        ClearSlot();
    }

    public void ClearSlot()
    {

        itemData = null;
        stackSize = -1;
    }

    public void AssignItem(InventorySlot invSlot)
    {
        //add to stack if item is same
        if (itemData == invSlot.ItemData)
        {
            AddToStack(invSlot.stackSize);
        }
        // overwrite slot with slot being passed in
        else
        {
            itemData = invSlot.itemData;
            stackSize = 0;
            AddToStack(invSlot.stackSize);
            itemEffect = itemData.useEffect;
            itemEnd = itemData.endEffect;
        }
    }

    public void UpdateInventorySlot(InventoryItemData data, int amount)
    {
        itemData = data;
        stackSize = amount;
        itemEffect = itemData.useEffect;
        itemEnd = itemData.endEffect;
    }

    //checks if there is enough room in stack for adding amount
    public bool EnoughRoomLeftInStack(int amountToAdd, out int amountRemaining)
    {
        amountRemaining = ItemData.maxStack - stackSize;
        return EnoughRoomLeftInStack(amountToAdd);
    }

    public bool EnoughRoomLeftInStack(int amountToAdd)
    {
        if (itemData == null || itemData != null && stackSize + amountToAdd <= itemData.maxStack) return true;
        else return false;
    }

    public void AddToStack(int amount)
    {
        stackSize += amount;
    }

    public void RemoveFromStack(int amount) 
    {
        stackSize -= amount;
    }

    public bool SplitStack(out InventorySlot splitStack)
    {
        // check if theres enough to split
        if (stackSize <= 1)
        {
            splitStack = null;
            return false;
        }
        // get half of stack
        int halfStack = Mathf.RoundToInt(stackSize / 2);
        RemoveFromStack(halfStack);

        //creates copy of this slot with half stack size
        splitStack = new InventorySlot(itemData, halfStack);
        return true;
    }

}
