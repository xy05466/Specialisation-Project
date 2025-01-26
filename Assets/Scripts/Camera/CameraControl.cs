using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using static UnityEngine.Rendering.DebugUI;

public class CameraControl : MonoBehaviour
{
    private Camera camera;
    private Vector2 _mousePosition = Vector2.zero;
    public float speed = 50f;
    private float rotationX;
    private CinemachineVirtualCamera virtualCamera;
    public GameObject peg;
    LayerMask layermask;
    private GameObject prevHit;

    // Start is called before the first frame update
    void Awake()
    {
        camera = Camera.main;
        rotationX = camera.transform.rotation.eulerAngles.x;
        virtualCamera = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
        layermask = LayerMask.GetMask("Walls");
        //peg.transform.RotateAround(transform.position, Vector3.up, virtualCamera.transform.localEulerAngles.y);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Input.GetKey(KeyCode.U))
        {
            //Camera.transform.Rotate(new Vector3(0, speed * Time.deltaTime, 0f));
            //Camera.transform.rotation = Quaternion.Euler(rotationX, transform.rotation.eulerAngles.y, 0f);
            camera.transform.RotateAround(transform.position,Vector3.up, speed * Time.deltaTime);
            //RotatePeg(true);
        }
        else if (Input.GetKey(KeyCode.O))
        {
            //Camera.transform.Rotate(new Vector3(0, -speed * Time.deltaTime, 0f));
            //Camera.transform.rotation = Quaternion.Euler(rotationX, transform.rotation.eulerAngles.y, 0f);
            camera.transform.RotateAround(transform.position, Vector3.up, -speed * Time.deltaTime);
            //RotatePeg(false);
        }
        //else
        //{
        //    camera.transform.localRotation = Quaternion.Euler(0.0f, gameObject.transform.rotation.y * -1.0f, 0.0f);
        //}
        //SetTransparentObject();
    }

    private void RotatePeg(bool isIncrease)
    {
        if (isIncrease)
        {
            peg.transform.RotateAround(transform.position, Vector3.up, speed * Time.deltaTime);
        }
        else
        {
            peg.transform.RotateAround(transform.position, Vector3.up, -speed * Time.deltaTime);
        }

    }

    private void SetTransparentObject()
    {
       
        RaycastHit hit;
        //Vector3 virtualRotation = virtualCamera.transform.position
       
        Vector3 direction = (peg.transform.localPosition - transform.position).normalized;
        if (Physics.Raycast(transform.position, direction, out hit, 1000, layermask))
        {
            if (prevHit!= null && prevHit != hit.transform.gameObject)
            {
                ChangeAlpha(prevHit.GetComponent<Renderer>().material, 1.0f);
            }
            //Debug.Log(hit.transform.gameObject.name);
            //ChangeAlpha(hit.transform.gameObject.GetComponent<Renderer>().material, 0.2f);
            prevHit = hit.transform.gameObject;
        }


    }

    private void ChangeAlpha(Material mat, float alphaValue)
    {
        Color oldColor = mat.color;
        Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, alphaValue);
        mat.SetColor("_Color", newColor);
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
