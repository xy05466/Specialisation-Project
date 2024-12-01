using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Runtime.CompilerServices.RuntimeHelpers;

public class CameraControl : MonoBehaviour
{
    public GameObject camera;
    private Vector2 _mousePosition = Vector2.zero;
    public float speed = 50f;
    private float rotationX;
    // Start is called before the first frame update
    void Awake()
    {
        rotationX = camera.transform.rotation.eulerAngles.x;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Input.GetKey(KeyCode.U))
        {
            //Camera.transform.Rotate(new Vector3(0, speed * Time.deltaTime, 0f));
            //Camera.transform.rotation = Quaternion.Euler(rotationX, transform.rotation.eulerAngles.y, 0f);
            camera.transform.RotateAround(transform.position,Vector3.up, speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.O))
        {
            //Camera.transform.Rotate(new Vector3(0, -speed * Time.deltaTime, 0f));
            //Camera.transform.rotation = Quaternion.Euler(rotationX, transform.rotation.eulerAngles.y, 0f);
            camera.transform.RotateAround(transform.position, Vector3.up, -speed * Time.deltaTime);
        }
    }

    public void RotateCamera(KeyCode keyCode)
    {
        if (keyCode == KeyCode.U)
        {
            camera.transform.Rotate(camera.transform.rotation.x, camera.transform.rotation.y * Time.deltaTime, camera.transform.rotation.z);
        }
        else if (keyCode == KeyCode.O)
        {
        }
    }

    public void UpdateCamera()
    {
        if (Input.GetKey(KeyCode.U))
        {
            camera.transform.Rotate(0, camera.transform.rotation.y * speed *Time.deltaTime, 0);
        }
        else if (Input.GetKey(KeyCode.O))
        {

        }
    }

}
