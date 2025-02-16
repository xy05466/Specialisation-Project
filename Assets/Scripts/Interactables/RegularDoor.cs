using Eflatun.SceneReference;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class RegularDoor : MonoBehaviour, IInteractable
{
    public GameObject interactionImage;
    private GameObject playerObject;
    private HashSet<GameObject> objectsInsideSphere = new HashSet<GameObject>();

    private Animator animator;
    public readonly int openFront = Animator.StringToHash("RegularDoorOpenFront");
    public readonly int closeFront = Animator.StringToHash("RegularDoorCloseFront");
    public readonly int openBack = Animator.StringToHash("RegularDoorOpenBack");
    public readonly int closeBack = Animator.StringToHash("RegularDoorCloseBack");
    public readonly int openOneWay = Animator.StringToHash("RegularDoorOpenFront");

    private bool isOpened;
    private Vector3 prevPlayerPos;

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
                interactionImage.SetActive(true);
                playerObject = colliders[i].gameObject;

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
        Vector3 directionToTarget = prevPlayerPos - transform.position;
        float dotProductLR = Vector3.Dot(transform.right, directionToTarget);
        float dotProductUD = Vector3.Dot(transform.forward, directionToTarget);

        if (dotProductLR > 0)
        {
            Debug.Log("Target is to the right");
            animator.CrossFade(closeFront, 0.1f);
        }
        else if (dotProductLR < 0)
        {
            Debug.Log("Target is to the left");
            animator.CrossFade(closeBack, 0.1f);
        }
        if (dotProductUD > 0)
        {
            animator.CrossFade(closeBack, 0.1f);
            Debug.Log("Target is in front");
        }
        else if (dotProductUD < 0)
        {
            animator.CrossFade(closeFront, 0.1f);
            Debug.Log("Target is behind");

        }
        //if (transform.root.localRotation.eulerAngles.y == 0)
        //{
        //    if (prevPlayerPos.z > transform.position.z)
        //    {
        //        animator.CrossFade(closeBack, 0.1f);
        //    }
        //    else
        //    {
        //        animator.CrossFade(closeFront, 0.1f);
        //    }
        //}
        //else if (transform.root.localRotation.eulerAngles.y == 90)
        //{
        //    if (prevPlayerPos.x < transform.position.x)
        //    {
        //        animator.CrossFade(closeBack, 0.1f);

        //    }
        //    else
        //    {
        //        animator.CrossFade(closeFront, 0.1f);
        //    }
        //}
        isOpened = false;
    }

    public void Interact(Interactor interactor, out bool interactSuccessful)
    {
       
        if (!isOpened)
        {
            animator.enabled = true;
            Vector3 directionToTarget = interactor.transform.position - transform.position;
            prevPlayerPos = interactor.transform.position;

            if (transform.rotation.eulerAngles.z == 0)
            {
                float dotProductUD = Vector3.Dot(transform.forward, directionToTarget);
                if (dotProductUD > 0)
                {
                    animator.CrossFade(openBack, 0.1f);
                    Debug.Log("Target is in front");
                }
                else if (dotProductUD < 0)
                {
                    animator.CrossFade(openFront, 0.1f);
                    Debug.Log("Target is behind");

                }
            }
            else if (transform.rotation.eulerAngles.z == 90)
            {
                float dotProductLR = Vector3.Dot(transform.right, directionToTarget);
                if (dotProductLR > 0)
                {
                    Debug.Log("Target is to the right");
                    animator.CrossFade(openFront, 0.1f);
                }
                else if (dotProductLR < 0)
                {
                    Debug.Log("Target is to the left");
                    animator.CrossFade(openBack, 0.1f);
                }
            }
            
            isOpened = true;
            //
            //if (transform.root.localRotation.eulerAngles.y == 0)
            //{
            //    isOpened = true;
            //    if (interactor.transform.position.z > transform.position.z)
            //    {
            //        animator.CrossFade(openBack, 0.1f);
            //    }
            //    else
            //    {
            //        animator.CrossFade(openFront, 0.1f);
            //    }

            //}
            //else if (transform.root.localRotation.eulerAngles.y == 90)
            //{
            //    if (interactor.transform.position.x < transform.position.x)
            //    {
            //        animator.CrossFade(openBack, 0.1f);
            //    }
            //    else
            //    {
            //        animator.CrossFade(openFront, 0.1f);
            //    }
            //}

        }
        interactSuccessful = true;
    }


    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.tag == "Player" && Input.GetKeyDown(KeyCode.E) && !isOpened)
    //    {
    //        animator.enabled = true;
    //        prevPlayerPos = other.transform.position;
    //        if (transform.root.localRotation.eulerAngles.y == 0)
    //        {
    //            isOpened = true;
    //            if (other.transform.position.z > transform.position.z)
    //            {
    //                animator.CrossFade(openBack, 0.1f);

    //            }
    //            else
    //            {
    //                animator.CrossFade(openFront, 0.1f);
    //            }
                
    //        }
    //        else if (transform.root.localRotation.eulerAngles.y == 90)
    //        {
    //            if (other.transform.position.x < transform.position.x)
    //            {
    //                animator.CrossFade(openBack, 0.1f);
    //            }
    //            else
    //            {  
    //                animator.CrossFade(openFront, 0.1f);

    //            }
    //        }
    //        //Debug.Log(myCollider.name);
           
    //        //animator.Play("RegularDoorOpen");
    //    }

    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.tag == "Player")
    //    {
    //        if (transform.root.localRotation.eulerAngles.y == 0)
    //        {
    //            if (prevPlayerPos.z > transform.position.z)
    //            {
    //                animator.CrossFade(closeBack, 0.1f);
    //            }
    //            else
    //            {
    //                animator.CrossFade(closeFront, 0.1f);
    //            }
    //        }
    //        else if (transform.root.localRotation.eulerAngles.y == 90)
    //        {
    //            if (prevPlayerPos.x < transform.position.x)
    //            {
    //                animator.CrossFade(closeBack, 0.1f);
                    
    //            }
    //            else
    //            {
    //                animator.CrossFade(closeFront, 0.1f);
    //            }
    //        }
    //        isOpened = false;
    //        //animator.Play("RegularDoorClose");
    //    }
    //}
}
