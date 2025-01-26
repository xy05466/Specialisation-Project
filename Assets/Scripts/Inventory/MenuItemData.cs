using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuItemData : MonoBehaviour
{
    public Image ItemSprite;
    public TMP_Text ItemCount;
    public InventorySlot AssignedInventorySlot;

    private void Awake()
    {
        ItemSprite.color = Color.clear;
        ItemCount.text = "";
    }

    public void UpdateMouseSlot(InventorySlot invSlot)
    {
        AssignedInventorySlot.AssignItem(invSlot);
        ItemSprite.sprite = invSlot.ItemData.icon;
        ItemCount.text = invSlot.StackSize.ToString();
        ItemSprite.color = Color.white;
    }

    private void StartUseItem(string name)
    {
        AssignedInventorySlot.ItemData.UseItem(name);
        AssignedInventorySlot.RemoveFromStack(1);
        if (AssignedInventorySlot.StackSize == 0)
        {
            ClearSlot();
        }
        else
        {
            ItemCount.text = AssignedInventorySlot.StackSize.ToString();
        }
    }

    private void Update()
    {
        // if has item, follow mouse pos
        if (AssignedInventorySlot.ItemData != null)
        {
            transform.position = Mouse.current.position.ReadValue();

            if (Mouse.current.leftButton.wasPressedThisFrame && !IsPointerOverUIObject() && !AssignedInventorySlot.ItemData.keyItem)
            {
                ClearSlot();
            }
            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                if (AssignedInventorySlot.ItemData.isUsable)
                {
                    if (!AssignedInventorySlot.ItemData.isUsedOverworld && !SceneManager.GetActiveScene().name.Contains("BattleArena"))
                    {
                        return;
                    }
                    else
                    {
                        foreach (RaycastResult result in GetClickedObjects())
                        {
                            if (result.gameObject.GetComponent<GetPlayerName>())
                            {
                                string name = result.gameObject.GetComponent<GetPlayerName>().GetPlayerNameString();

                                StartUseItem(name);

                            }
                        }
                    }

                    //if (AssignedInventorySlot.ItemData.isUsedOverworld)
                    //{
                    //    StartUseItem(name);
                    //}
                    //else
                    //{
                    //    if (SceneManager.GetActiveScene().name.Contains("BattleArena"))
                    //    {

                    //    }
                    //}
                    //    if (AssignedInventorySlot.ItemData.isUsedOverworld && !SceneManager.GetActiveScene().name.Contains("BattleArena"))
                    //{
                       
                    //}
                    //if (!AssignedInventorySlot.ItemData.isUsedOverworld && SceneManager.GetActiveScene().name.Contains("BattleArena"))
                    //{
                    //    StartUseItem(null);
                    //}
                }
                    
                    //ay, out hit, Mathf.Infinity, layerMask
                    //RaycastHit2D cubeHit = Physics2D.Raycast(cubeRay, Vector2.zero);

                    //if (cubeHit)
                    //{
                    //    Debug.Log("We hit " + cubeHit.collider.name);

                    //    AssignedInventorySlot.ItemData.UseItem();

                    //}
            }
            
            
        }

    }

    public void ClearSlot()
    {
        AssignedInventorySlot.ClearSlot();
        ItemCount.text = "";
        ItemSprite.color = Color.clear;
        ItemSprite.sprite = null;
    }

    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = Mouse.current.position.ReadValue();
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    public List<RaycastResult> GetClickedObjects()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = Mouse.current.position.ReadValue();
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results;
    }
}
