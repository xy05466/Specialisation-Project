using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevivalPotion : MonoBehaviour, IUsableItems
{
    Action IUsableItems.EndUse()
    {
        return delegate
        {
            Debug.Log("end use");
        };
    }

    Action IUsableItems.UseItem(string name)
    {
        return delegate
        {
            PlayerItemManager.Instance.SetPartyCondition(name);
            
            //Debug.Log("use item");
        };
    }
}
