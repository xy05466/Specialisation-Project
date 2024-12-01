using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoor : MonoBehaviour
{
    private Animator animator;
    public readonly int openFront = Animator.StringToHash("RegularDoorOpenFront");
    public readonly int closeFront = Animator.StringToHash("RegularDoorCloseFront");
    public readonly int openBack = Animator.StringToHash("RegularDoorOpenBack");
    public readonly int closeBack = Animator.StringToHash("RegularDoorCloseBack");
    public readonly int openOneWay = Animator.StringToHash("RegularDoorOpenFront");

    private bool isOpened;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && Input.GetKeyDown(KeyCode.E) && !isOpened && PlayerPrefs.GetInt("keynum", 0) == 3)
        {
            animator.enabled = true;
            if (transform.root.localRotation.eulerAngles.y == 0)
            {
                isOpened = true;
                if (other.transform.position.z > transform.position.z)
                {

                    animator.CrossFade(openBack, 0);
                }
                else
                {

                    animator.CrossFade(openFront, 0);
                }
            }
            else if (transform.root.localRotation.eulerAngles.y == 90)
            {
                if (other.transform.position.x < transform.position.x)
                {
                    animator.CrossFade(openBack, 0);
                }
                else
                {
                    animator.CrossFade(openFront, 0);

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
                if (other.transform.position.z > transform.position.z)
                {
                    animator.CrossFade(closeFront, 0);
                }
                else
                {
                    animator.CrossFade(closeBack, 0);
                }
            }
            else if (transform.root.localRotation.eulerAngles.y == 90)
            {
                if (other.transform.position.x < transform.position.x)
                {
                    animator.CrossFade(closeFront, 0);
                }
                else
                {
                    animator.CrossFade(closeBack, 0);
                }
            }
            isOpened = false;
            //animator.Play("RegularDoorClose");
        }
    }
}
