using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChestInventory : InventoryHolder, IInteractable
{
    private Animator animator;
    public readonly int openChest = Animator.StringToHash("ChestOpen");
    public readonly int closeChest = Animator.StringToHash("ChestClose");
    public UnityAction<IInteractable> OnInteractionComplete { get;set; }
    
    protected override void Awake()
    {
        base.Awake();
        SaveLoad.OnLoadGame += LoadInventory;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        var ChestSaveData = new InventorySaveData(inventorySystem, transform.position,transform.rotation);
        SaveGameManager.data.chestDictionary.Add(GetComponent<UniqueID>().ID, ChestSaveData);
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
        animator.CrossFade(closeChest, 0);
    }

    public void Interact(Interactor interactor, out bool interactSuccessful)
    {
        animator.enabled = true;
        animator.CrossFade(openChest,0);
        OnDynamicInventoryDisplayRequested?.Invoke(inventorySystem);
        interactSuccessful = true;
    }
}


