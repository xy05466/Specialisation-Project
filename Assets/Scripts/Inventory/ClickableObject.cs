using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ClickableObject : MonoBehaviour, IPointerClickHandler
{
    public UnityEvent onLeftClick;
    public UnityEvent onRightClick;
    public UnityEvent onMiddleClick;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("Left click");
        }
            
        else if (eventData.button == PointerEventData.InputButton.Middle)
        {
            Debug.Log("Middle click");
        }
           
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            //onRightClick.Invoke();
            GetPlayerName();
            Debug.Log("Right click");
        }
    }

    public string GetPlayerName()
    {
        return transform.parent.gameObject.GetComponent<SimpleStatusHUD>().name.text;
    }
}
