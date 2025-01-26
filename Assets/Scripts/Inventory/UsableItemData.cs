using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//defines what an item is in game

[CreateAssetMenu(menuName = "InventorySystem/Inventory Item")]
public class UsableItemData : ScriptableObject
{
    public int ID;
    public string displayName;
    [TextArea(4,4)]
    public string description;
    public Sprite icon;
    public int maxStack;
    public int manaCost;
    public bool keyItem;
    public bool isUsable;
    public bool isUsedOverworld;
}
