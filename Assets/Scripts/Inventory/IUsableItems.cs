using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUsableItems
{
    public System.Action UseItem(string name);
    public System.Action EndUse();
}
