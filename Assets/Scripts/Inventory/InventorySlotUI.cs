using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image itemSprite;
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text itemCount;
    [SerializeField] private TMP_Text itemDesc;
    [SerializeField] private InventorySlot assignedInventorySlot;

    private Button button;
    public Action itemEffect;
    private ClickableObject clickableObject;
    public InventorySlot AssignedInventorySlot => assignedInventorySlot;
    public InventoryDisplay ParentDisplay {get; private set; }

    private void Awake()
    {
        ClearSlot();
        button = GetComponent<Button>();
        clickableObject = GetComponent<ClickableObject>();
        button?.onClick.AddListener(OnUISlotClick);
        //button?.onClick.AddListener(OnUISlotRightClick);
        ParentDisplay = transform.parent.GetComponent<InventoryDisplay>();
    }

    public void Init(InventorySlot slot)
    {
        assignedInventorySlot = slot;
        UpdateUISlot(slot);
    }

    public void ClearSlot()
    {
        assignedInventorySlot?.ClearSlot();
        itemSprite.sprite = null;
        itemSprite.color = Color.clear;
        itemCount.text = "";
        if (itemName && itemDesc != null)
        {
            itemName.text = "";
            itemDesc.text = "";
        }
        
    }

    public void UpdateUISlot(InventorySlot slot)
    {
        if (slot.ItemData != null)
        {
            if (itemName && itemDesc != null)
            {
                itemName.text = slot.ItemData.displayName;
                itemDesc.text = slot.ItemData.description;
            }
            itemSprite.sprite = slot.ItemData.icon;
            itemSprite.color = Color.white;
            if (slot.StackSize > 1)
            {
                itemCount.text = slot.StackSize.ToString();
            }
            else if (slot.StackSize == 1)
            {
                itemCount.text = "";
            }
            else
            {
                ClearSlot();
            }
        }
        else
        {
            ClearSlot();
        }
    }

    public void UpdateUISlot()
    {
        if (assignedInventorySlot != null)
        {
            UpdateUISlot(assignedInventorySlot);
        }
    }

    public void OnUISlotClick()
    {
        ParentDisplay?.SlotClicked(this);
    }

    public void OnUISlotRightClick()
    {
        ParentDisplay?.SlotRightClicked(this);
    }
}
