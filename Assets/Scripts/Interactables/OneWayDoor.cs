using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OneWayDoor : MonoBehaviour, IInteractable
{
    public GameObject interactionImage;
    public GameObject collider;
    private GameObject playerObject;
    private HashSet<GameObject> objectsInsideSphere = new HashSet<GameObject>();
    private Animator animator;

    public readonly int openDoor = Animator.StringToHash("OneWayDoorOpen");
    public readonly int closeDoor = Animator.StringToHash("OneWayDoorClose");
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        HashSet<GameObject> currentObjects = new HashSet<GameObject>();
        var colliders = Physics.OverlapSphere(transform.position, 1.5f);
        for (int i = 0; i < colliders.Length; i++)
        {
            currentObjects.Add(colliders[i].gameObject);
            if (colliders[i].gameObject.tag == "NormalPresence" && !interactionImage.activeSelf)
            {
                Vector3 toOther = colliders[i].transform.position - transform.position;
                float dotProductUD = Vector3.Dot(Vector3.forward, toOther);
                float dotProductLR = Vector3.Dot(Vector3.right, toOther);
                if ((transform.rotation.eulerAngles.y == 0 && dotProductUD < 0) ||
                    (transform.rotation.eulerAngles.y == 90 && dotProductLR < 0) ||
                    (transform.rotation.eulerAngles.y == 180 && dotProductUD > 0) ||
                    (transform.rotation.eulerAngles.y == 270 && dotProductLR > 0))
                {

                    interactionImage.SetActive(true);
                    playerObject = colliders[i].gameObject;

                }
               

            }
        }

        foreach (var obj in objectsInsideSphere)
        {
            if (!currentObjects.Contains(obj))
            {
                if (playerObject != null)
                {
                    interactionImage.SetActive(false);
                }


            }
        }
        objectsInsideSphere = currentObjects;
    }
    public void EndInteraction()
    {
        collider.SetActive(true);
        animator.CrossFade(closeDoor, 0);
    }

    public void Interact(Interactor interactor, out bool interactSuccessful)
    {
        //Vector3 directionToTarget = interactor.transform.position - transform.position;
        Vector3 toOther = Vector3.Normalize(interactor.transform.position - transform.position);

        float dotProductUD = Vector3.Dot(Vector3.forward, toOther);
        float dotProductLR = Vector3.Dot(Vector3.right, toOther);
        if ((transform.rotation.eulerAngles.y == 0 && dotProductUD < 0) ||
            (transform.rotation.eulerAngles.y == 90 && dotProductLR < 0) ||
            (transform.rotation.eulerAngles.y == 180 && dotProductUD > 0) ||
            (transform.rotation.eulerAngles.y == 270 && dotProductLR > 0))
        {
            collider.SetActive(false);
            animator.enabled = true;
            animator.CrossFade(openDoor, 0);
            interactSuccessful = true;
            
        }
        else
        {
            interactSuccessful = false;
        }

       
        
    }

    //private void OnTriggerStay(Collider other)
    //{
        
    //    if (other.tag == "Player" && Input.GetKeyDown(KeyCode.E))
    //    {
    //        animator.enabled = true;
    //        animator.CrossFade(openDoor, 0);
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.tag == "Player")
    //    {
    //        animator.CrossFade(closeDoor, 0);
    //    }
    //}
}
