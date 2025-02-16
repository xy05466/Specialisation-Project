using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMenu : MonoBehaviour
{
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play("die", 0,(1f / 108f) * 108f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
