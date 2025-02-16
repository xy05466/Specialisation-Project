using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUsableItems
{
    public System.Action AssignName(string newName);
    public System.Action UseItem();
    public System.Action EndUse();
}
