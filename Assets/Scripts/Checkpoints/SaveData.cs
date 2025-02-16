using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    public List<string> collectedItems;
    public List<string> itemGenerator;
    public SerializableDict<string, InventorySaveData> chestDictionary;
    public SerializableDict<string, ItemPickUpSaveData> activeItems;
    public InventorySaveData playerInventory;
    //public List<CharacterStatus> playerParty;

    public SaveData() 
    {
        collectedItems = new List<string>();
        itemGenerator = new List<string>();
        chestDictionary = new SerializableDict<string, InventorySaveData>();
        activeItems = new SerializableDict<string, ItemPickUpSaveData> ();
        playerInventory = new InventorySaveData();
        //playerParty = new List<CharacterStatus>();
    }
}
