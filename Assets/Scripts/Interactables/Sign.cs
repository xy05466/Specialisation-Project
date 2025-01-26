using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Sign : MonoBehaviour, IInteractable
{
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EndInteraction()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void Interact(Interactor interactor, out bool interactSuccessful)
    {
        transform.GetChild(0).gameObject.SetActive(true);
        interactSuccessful = true;
    }
}
