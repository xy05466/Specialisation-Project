using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPPotion : MonoBehaviour, IUsableItems
{
    string usedName;
    public Action AssignName(string newName)
    {
        return delegate
        {
            usedName = newName;
        };
    }
    Action IUsableItems.EndUse()
    {
        return delegate
        {
            Debug.Log("end use");
        };
    }

    Action IUsableItems.UseItem()
    {
        return delegate
        {
            Debug.Log(usedName);
            PlayerItemManager.Instance.SetMaxHP(usedName);

            //Debug.Log("use item");
        };
    }
}
