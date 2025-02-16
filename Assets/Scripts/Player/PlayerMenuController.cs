using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMenuController : MonoBehaviour
{
    private float moveSpeed = 8f;
    public GameObject skeleton;
    [SerializeField] private Transform waypoint;
    private AnimationController animCon;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        
        //animCon = AnimationController.Instance;
        if (SceneManager.GetActiveScene().name == "End")
        {
            animator.Play("sit");
        }
        else if (SceneManager.GetActiveScene().name == "Start")
        {
            animator.Play("walk");
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Start")
        {
            float moveDistance = moveSpeed * Time.deltaTime;

            transform.Translate(Vector3.forward * moveDistance);
            if (skeleton.transform.position.z < -2.0f)
            {
                transform.position = waypoint.position;

            }
        }
    }
}
