using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChestInventory : InventoryHolder, IInteractable
{
    public GameObject interactionImage;
    private GameObject playerObject;
    private HashSet<GameObject> objectsInsideSphere = new HashSet<GameObject>();
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
        Debug.Log(this.gameObject.name);
        animator = GetComponent<Animator>();
        var ChestSaveData = new InventorySaveData(inventorySystem);
        SaveGameManager.data.chestDictionary.Add(GetComponent<UniqueID>().ID, ChestSaveData);
    }

    void Update()
    {
        HashSet<GameObject> currentObjects = new HashSet<GameObject>();
        var colliders = Physics.OverlapSphere(transform.position, 2f);
        for (int i = 0; i < colliders.Length; i++)
        {
            currentObjects.Add(colliders[i].gameObject);
            if (colliders[i].gameObject.tag == "NormalPresence" && !interactionImage.activeSelf)
            {
                interactionImage.SetActive(true);
                playerObject = colliders[i].gameObject;

            }
        }

        foreach (var obj in objectsInsideSphere)
        {
            if (!currentObjects.Contains(obj))
            {
                if (playerObject != null)
                {
                    interactionImage.SetActive(false);
                }


            }
        }
        objectsInsideSphere = currentObjects;
    }

    protected override void LoadInventory(SaveData data)
    {
        //check save data for this chest inventory, if it exists, load it in
       if (this != null)
       {
            if (data.chestDictionary.TryGetValue(GetComponent<UniqueID>().ID, out InventorySaveData chestData))
            {
                this.inventorySystem = chestData.invSystem;
                Debug.Log("Inventory loaded");
            }

       }



    }

    private void OnDisable()
    {
       SaveLoad.OnLoadGame -= LoadInventory;
        Debug.Log("Inventory destroyed");
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


