using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularDoor : MonoBehaviour
{
    private Animator animator;
    public readonly int openFront = Animator.StringToHash("RegularDoorOpenFront");
    public readonly int closeFront = Animator.StringToHash("RegularDoorCloseFront");
    public readonly int openBack = Animator.StringToHash("RegularDoorOpenBack");
    public readonly int closeBack = Animator.StringToHash("RegularDoorCloseBack");
    public readonly int openOneWay = Animator.StringToHash("RegularDoorOpenFront");

    private bool isOpened;
    private Vector3 prevPlayerPos;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && Input.GetKeyDown(KeyCode.E) && !isOpened)
        {
            animator.enabled = true;
            prevPlayerPos = other.transform.position;
            if (transform.root.localRotation.eulerAngles.y == 0)
            {
                isOpened = true;
                if (other.transform.position.z > transform.position.z)
                {

                    animator.CrossFade(openBack, 0.1f);
                }
                else
                {
                    
                    animator.CrossFade(openFront, 0.1f);
                }
                
            }
            else if (transform.root.localRotation.eulerAngles.y == 90)
            {
                if (other.transform.position.x < transform.position.x)
                {
                    animator.CrossFade(openBack, 0.1f);
                }
                else
                {  
                    animator.CrossFade(openFront, 0.1f);

                }
            }
            //Debug.Log(myCollider.name);
           
            //animator.Play("RegularDoorOpen");
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (transform.root.localRotation.eulerAngles.y == 0)
            {
                if (prevPlayerPos.z > transform.position.z)
                {
                    animator.CrossFade(closeBack, 0.1f);
                }
                else
                {
                    animator.CrossFade(closeFront, 0.1f);
                }
            }
            else if (transform.root.localRotation.eulerAngles.y == 90)
            {
                if (prevPlayerPos.x < transform.position.x)
                {
                    animator.CrossFade(closeBack, 0.1f);
                    
                }
                else
                {
                    animator.CrossFade(closeFront, 0.1f);
                }
            }
            isOpened = false;
            //animator.Play("RegularDoorClose");
        }
    }
}
