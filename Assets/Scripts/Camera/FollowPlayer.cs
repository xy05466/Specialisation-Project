using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;
    private Transform target;

    void Start()
    {
        virtualCamera = this.GetComponent < CinemachineVirtualCamera>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        virtualCamera.Follow = target;
    }
}
