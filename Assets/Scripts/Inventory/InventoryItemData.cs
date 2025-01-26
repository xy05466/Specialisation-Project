using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public Action endEffect;
    private string toUseOn;

    public void AssignUse(IUsableItems usable)
    {
        useEffect = usable.UseItem(toUseOn);
    }

    public void AssignEnd(IUsableItems usable)
    {
        endEffect = usable.EndUse();
    }

    public void UseItem(string name)
    {
        toUseOn = name;
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
