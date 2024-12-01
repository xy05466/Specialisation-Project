using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayDoor : MonoBehaviour
{
    private Animator animator;

    public readonly int openDoor = Animator.StringToHash("OneWayDoorOpen");
    public readonly int closeDoor = Animator.StringToHash("OneWayDoorClose");
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnTriggerStay(Collider other)
    {
        
        if (other.tag == "Player" && Input.GetKeyDown(KeyCode.E))
        {
            animator.enabled = true;
            animator.CrossFade(openDoor, 0);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            animator.CrossFade(closeDoor, 0);
        }
    }
}
