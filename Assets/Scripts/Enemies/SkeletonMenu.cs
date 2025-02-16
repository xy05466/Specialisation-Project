using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMenu : MonoBehaviour
{
    private float moveSpeed = 3f;
    [SerializeField] private Transform waypoint;
    private AnimationController animCon;
    private Animator animator;
    public Transform InteractionPoint;
    bool isLookingAround;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play("walk");
        isLookingAround = false;
    }

    // Update is called once per frame
    void Update()
    {

        float moveDistance = moveSpeed * Time.deltaTime;
        transform.Translate(Vector3.forward * moveDistance);
        if (transform.position.z < -16.0f)
        {
            transform.position = waypoint.position;
        }
        
       
    }

}
