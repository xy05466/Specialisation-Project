using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffects : MonoBehaviour
{
    public static ItemEffects Instance = null;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject.transform.parent.gameObject);
            return;
        }
        Instance = this;
        if (transform.parent == null)
            DontDestroyOnLoad(this);
    }

    public void UseItem(InventoryItemData inventoryItemData)
    {

    }


}
