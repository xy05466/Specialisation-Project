using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPlayerName : MonoBehaviour
{
    public string GetPlayerNameString()
    {
        return transform.parent.gameObject.GetComponent<SimpleStatusHUD>().name.text;
    }

    
}
