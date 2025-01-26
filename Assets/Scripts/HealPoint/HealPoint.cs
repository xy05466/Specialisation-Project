using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealPoint : InventoryHolder, IInteractable
{
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    protected override void Awake()
    {
        base.Awake();
        SaveLoad.OnLoadGame += LoadInventory;
    }

    private void Start()
    {
        //animator = GetComponent<Animator>();
        var ChestSaveData = new InventorySaveData(inventorySystem, transform.position, transform.rotation);
        //SaveGameManager.data.chestDictionary.Add(GetComponent<UniqueID>().ID, ChestSaveData);
    }

    protected override void LoadInventory(SaveData data)
    {
        //check save data for this chest inventory, if it exists, load it in
        if (data.chestDictionary.TryGetValue(GetComponent<UniqueID>().ID, out InventorySaveData chestData))
        {
            this.inventorySystem = chestData.invSystem;
            this.transform.position = chestData.position;
            this.transform.rotation = chestData.rotation;
        }

    }

    public void EndInteraction()
    {
        
    }

    public void Interact(Interactor interactor, out bool interactSuccessful)
    {
        OnHealDisplayRequested?.Invoke(inventorySystem);
        interactSuccessful = true;
        //interactor.gameObject.GetComponent<StatusManager>().SetCharacterStatus()
    }
}
