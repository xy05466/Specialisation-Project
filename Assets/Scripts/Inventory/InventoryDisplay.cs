using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class InventoryDisplay : MonoBehaviour
{
    [SerializeField] MenuItemData menuInventoryItem;
    protected InventorySystem inventorySystem;
    //pair up ui slots with system slots
    protected Dictionary<InventorySlotUI, InventorySlot> slotDictionary;
    public InventorySystem InventorySystem => inventorySystem;
    public Dictionary<InventorySlotUI, InventorySlot> SlotDictionary => slotDictionary;

    protected virtual void Start()
    {

    }
    public abstract void AssignSlot(InventorySystem invToDisplay);
    protected virtual void UpdateSlot(InventorySlot updatedSlot)
    {
        foreach (var slot in slotDictionary)
        {
            if (slot.Value == updatedSlot)
            {
                slot.Key.UpdateUISlot(updatedSlot);
            }
        }
    }

    public void SlotRightClicked(InventorySlotUI clickedUISlot)
    {
        if (clickedUISlot.AssignedInventorySlot.ItemData != null)
        {
           if (clickedUISlot.AssignedInventorySlot.ItemData.isUsable)
           {
              
                //clickedUISlot.AssignedInventorySlot.ItemData.UseItem();


           }
        }
        ////place mouse item into empty slot if clicked slot is empty
        //if (clickedUISlot.AssignedInventorySlot.ItemData == null && menuInventoryItem.AssignedInventorySlot.ItemData != null)
        //{
           
        //}
    }

    public void SlotClicked(InventorySlotUI clickedUISlot)
    {
        bool isShiftPressed = Keyboard.current.leftShiftKey.isPressed;

        if (clickedUISlot.AssignedInventorySlot.ItemData != null && menuInventoryItem.AssignedInventorySlot.ItemData == null)
        {
            //split stack if player is holding shift key
            if (isShiftPressed && clickedUISlot.AssignedInventorySlot.SplitStack(out InventorySlot halfStackSlot))
            {
                menuInventoryItem.UpdateMouseSlot(halfStackSlot);
                clickedUISlot.UpdateUISlot();
                return;
            }
            //pick up item in clicked slot
            else
            {
                menuInventoryItem.UpdateMouseSlot(clickedUISlot.AssignedInventorySlot);
                clickedUISlot.ClearSlot();
                return;
            }
           
        }
        //place mouse item into empty slot if clicked slot is empty
        if (clickedUISlot.AssignedInventorySlot.ItemData == null && menuInventoryItem.AssignedInventorySlot.ItemData != null)
        {
            clickedUISlot.AssignedInventorySlot.AssignItem(menuInventoryItem.AssignedInventorySlot);
            clickedUISlot.UpdateUISlot();

            menuInventoryItem.ClearSlot();
            return;
        }
        //combine same items

        //if slot stack size + mouse stack size > the slot max stack size, take from mouse

        //if different items, swap items
        if (clickedUISlot.AssignedInventorySlot.ItemData != null && menuInventoryItem.AssignedInventorySlot.ItemData != null)
        {
            bool isSameItem = clickedUISlot.AssignedInventorySlot.ItemData == menuInventoryItem.AssignedInventorySlot.ItemData;

            //combines same item
            if (isSameItem && clickedUISlot.AssignedInventorySlot.EnoughRoomLeftInStack(menuInventoryItem.AssignedInventorySlot.StackSize) )
            {
                clickedUISlot.AssignedInventorySlot.AssignItem(menuInventoryItem.AssignedInventorySlot);
                clickedUISlot.UpdateUISlot();
                menuInventoryItem.ClearSlot();
            }
            else if(isSameItem && !clickedUISlot.AssignedInventorySlot.EnoughRoomLeftInStack(menuInventoryItem.AssignedInventorySlot.StackSize, out int leftInStack))
            {
                if (leftInStack < 1)
                {
                    //stack is full so swap items
                    SwapSlots(clickedUISlot);
                    
                }
                else //add from mouse stack to un maxed slot
                {
                    int remainingOnMouse = menuInventoryItem.AssignedInventorySlot.StackSize - leftInStack;
                    clickedUISlot.AssignedInventorySlot.AddToStack(leftInStack);
                    clickedUISlot.UpdateUISlot();
                    var newItem = new InventorySlot(menuInventoryItem.AssignedInventorySlot.ItemData, remainingOnMouse);
                    menuInventoryItem.ClearSlot();
                    menuInventoryItem.UpdateMouseSlot(newItem);
                }
            }

            else if (!isSameItem)
            {
                SwapSlots(clickedUISlot);
            }
        }
    }

    private void SwapSlots(InventorySlotUI clickedUISlot)
    {
        var clonedSlot = new InventorySlot(menuInventoryItem.AssignedInventorySlot.ItemData, menuInventoryItem.AssignedInventorySlot.StackSize);
        menuInventoryItem.ClearSlot();

        menuInventoryItem.UpdateMouseSlot(clickedUISlot.AssignedInventorySlot);

        clickedUISlot.ClearSlot();
        clickedUISlot.AssignedInventorySlot.AssignItem(clonedSlot);
        clickedUISlot.UpdateUISlot();

    }
}
