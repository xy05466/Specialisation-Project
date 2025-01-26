using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    public Transform InteractionPoint;
    public LayerMask InteractionLayer;
    public float InteractionPointRadius = 1f;
    public bool IsInteracting { get; private set; }
    private GameObject interactingObject;
    private bool isInCollider;
    private HashSet<GameObject> objectsInsideSphere = new HashSet<GameObject>();

    private void Update()
    {
        var colliders = Physics.OverlapSphere(InteractionPoint.position, InteractionPointRadius, InteractionLayer);

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                var interactable = colliders[i].GetComponent<IInteractable>();
                if (interactable != null)
                {
                    StartInteraction(interactable);
                    interactingObject = colliders[i].gameObject;
                }

            }

        }
        HashSet<GameObject> currentlyDetectedObjects = new HashSet<GameObject>();
        foreach (var collider in colliders)
        {
            var component = collider.GetComponent<IInteractable>();
            if (component != null)
            {
                currentlyDetectedObjects.Add(collider.gameObject);
            }
        }

        // Check for objects that have left the sphere
        foreach (var obj in objectsInsideSphere)
        {
            if (!currentlyDetectedObjects.Contains(obj))
            {
                EndInteraction(interactingObject.GetComponent<IInteractable>());
                interactingObject = null;
            }
        }
        objectsInsideSphere = currentlyDetectedObjects;
        //for (int i = 0;i < colliders.Length;i++)
        //{
        //    if (colliders[i].gameObject != interactingObject)
        //    {

        //    }
        //}
        //if (!isInCollider && interactingObject != null)
        //{
        //    EndInteraction(interactingObject.GetComponent<IInteractable>());
        //    interactingObject = null;
        //}
    }

    void StartInteraction(IInteractable interactable)
    {
        interactable.Interact(this, out bool interactSuccessful);
        IsInteracting = true;
    }

    void EndInteraction(IInteractable interactable)
    {
        interactable.EndInteraction();
        IsInteracting = false;
    }
}
