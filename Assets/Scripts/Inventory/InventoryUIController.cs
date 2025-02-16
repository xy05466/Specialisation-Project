using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InventoryUIController : MonoBehaviour
{
    public DynamicInventoryDisplay inventoryPanel;
    public DynamicInventoryDisplay healPanel;
    public GameObject itemGroup;
    public GameObject statusDisplay;

    private void Awake()
    {
        //itemGroup.SetActive(false);
        inventoryPanel.transform.parent.gameObject.SetActive(false);
        healPanel.transform.parent.gameObject.SetActive(false);
        itemGroup.transform.gameObject.SetActive(false);
        //itemButton.SetActive(false);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        if (!scene.name.Contains("BattleArena"))
        {
            inventoryPanel.transform.parent.gameObject.SetActive(false);
            healPanel.transform.parent.gameObject.SetActive(false);
            itemGroup.transform.gameObject.SetActive(false);
        }
  //inventoryPanel.transform.parent.gameObject.SetActive(false);
                //healPanel.transform.parent.gameObject.SetActive(false);
                //itemGroup.transform.gameObject.SetActive(false);
                Debug.Log("OnSceneLoaded: " + scene.name);
    }


    private void OnEnable()
    {
        InventoryHolder.OnDynamicInventoryDisplayRequested += DisplayInventory;
        InventoryHolder.OnHealDisplayRequested += DisplayHealPoint;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        InventoryHolder.OnDynamicInventoryDisplayRequested -= DisplayInventory;
        InventoryHolder.OnHealDisplayRequested -= DisplayHealPoint;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    // Update is called once per frame
    void Update()
    {
        //if (inventoryPanel.gameObject.activeInHierarchy && Keyboard.current.escapeKey.wasPressedThisFrame)
        //{
        //    inventoryPanel.gameObject.SetActive(false);
        //}
        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            if (inventoryPanel.gameObject.activeInHierarchy)
            {
                inventoryPanel.transform.parent.gameObject.SetActive(false);
            }
            if (healPanel.gameObject.activeInHierarchy)
            {
                healPanel.transform.parent.gameObject.gameObject.SetActive(false);
            }
            itemGroup.gameObject.SetActive(false);
            //if (statusDisplay.gameObject.activeInHierarchy)
            //{
            //    statusDisplay.gameObject.SetActive(false);
            //}
            //if (itemGroup.gameObject.activeInHierarchy)
            //{
            //    itemGroup.gameObject.SetActive(false);
            //}
        }
    }

    void DisplayInventory(InventorySystem invToDisplay)
    {
        inventoryPanel.transform.parent.gameObject.SetActive(true);
        //inventoryPanel.transform.parent.gameObject.SetActive(true);
        itemGroup.gameObject.SetActive(true);
        inventoryPanel.RefreshDynamicInventory(invToDisplay);
    }

    void DisableInventory()
    {
        inventoryPanel.transform.parent.gameObject.SetActive(false);
    }

    void DisableHealPoint()
    {
        healPanel.transform.parent.gameObject.SetActive(false);
        itemGroup.SetActive(false);
        statusDisplay.SetActive(false);
    }

    void DisplayHealPoint(InventorySystem invToDisplay)
    {
        healPanel.transform.parent.gameObject.SetActive(true);
        itemGroup.SetActive(true);
        statusDisplay.SetActive(true);
        healPanel.RefreshDynamicInventory(invToDisplay);
    }

    
}
