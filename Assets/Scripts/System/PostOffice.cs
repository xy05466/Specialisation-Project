using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class PostOffice : MonoBehaviour
{
    public static PostOffice instance;

    List<GameObject> recipients = new List<GameObject>();
    private PostOffice()
    {
    }
    public static PostOffice GetInstance()
    {
        if (!instance)
        {
            GameObject postOffice = new GameObject();
            postOffice.name = "Post Office";
            postOffice.AddComponent<PostOffice>();
            instance = postOffice.GetComponent<PostOffice>();
        }
        return instance;
    }
    public void SendToPostOffice(int currentState)
    {
        
        
    }

    public void Subscribe(GameObject newListener)
    {
        recipients.Add(newListener);
    }
}
