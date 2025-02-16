using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;
using static UnityEngine.UIElements.UxmlAttributeDescription;

//defines what an item is in game

[CreateAssetMenu(menuName = "InventorySystem/Inventory Item")]
public class InventoryItemData : ScriptableObject
{
    public int ID;
    public string displayName;
    [TextArea(4, 4)]
    public string description;
    public Sprite icon;
    public int maxStack;
    public int manaCost;
    public bool keyItem;
    public bool isUsable;
    public bool isUsedOverworld;
    public Action useEffect;
    public Action nameEffect;
    public Action endEffect;
    private string toUseOn;
    private IUsableItems item;

    public void AssignUse(IUsableItems usable)
    {
        useEffect = usable.UseItem();
        item = usable;
        Debug.Log(toUseOn);
    }

    public void AssignEnd(IUsableItems usable)
    {
        endEffect = usable.EndUse();
    }

    public void AssignName(string name)
    {
        nameEffect = item.AssignName(name);
    }

    public void UseItem()
    {
        //toUseOn = name;
        //Debug.Log(name);
        nameEffect.Invoke();
        useEffect.Invoke();
    }

    public void EndItem()
    {
        if (isUsable) 
        {
            endEffect.Invoke();
        }
    }
}
