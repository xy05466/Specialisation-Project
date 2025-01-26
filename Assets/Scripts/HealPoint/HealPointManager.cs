using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static UnityEditor.Progress;

public class HealPointManager : MonoBehaviour
{
    public EncounterManager statusManager;
    public DynamicInventoryDisplay inventoryDisplay;
    public TMP_Text text;
    public InventoryItemData healItem;
    public InventoryItemData reviveItem;
    private int healCount = 2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        text.text = "x2 red gems for heal\r\nx1 blue gem for revive";
    }

    public void HealPlayer()
    {
        if (inventoryDisplay != null)
        {
            List<InventorySlot> slots = inventoryDisplay.GetSlots();
            
            InventorySlot foundItem = slots.FirstOrDefault(i => i.ItemData == healItem);
            if (foundItem != null)
            {
                if (foundItem.StackSize == 2)
                {
                    if (statusManager.SetPartyHealth())
                    {
                        inventoryDisplay.RemoveItems(foundItem,2);
                        text.text = "Party restored to full health";
                    }
                    else
                    {
                        text.text = "Party already at full health";
                    }
                }
                else
                {
                    text.text = "Insufficient red gems";
                }
            }
            else
            {
                text.text = "No red gems placed";
            }
        }
    }

    public void RevivePlayer()
    {
        if (inventoryDisplay != null)
        {
            List<InventorySlot> slots = inventoryDisplay.GetSlots();

            InventorySlot foundItem = slots.FirstOrDefault(i => i.ItemData == reviveItem);
            if (foundItem != null)
            {
                if (foundItem.StackSize == 1)
                {
                    if (statusManager.SetPartyCondition())
                    {
                        inventoryDisplay.RemoveItems(foundItem,1);
                        text.text = "Party restored to full health";
                    }
                    else
                    {
                        text.text = "Party already at full health";
                    }
                }
                else
                {
                    text.text = "Insufficient blue gems";
                }
            }
            else
            {
                text.text = "No blue gems placed";
            }
        }
    }
}
