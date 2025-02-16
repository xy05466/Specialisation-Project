using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovementController : MonoBehaviour, PlayerInput.IPlayerActions
{
    private Vector3 _direction;
    private Quaternion _rotation;

    private Rigidbody playerRB;
    public CharacterController _characterController;
    public GameObject boss1Dialogue;
    private bool isMoving;

    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private AnimationController animCon;
    private Camera camera;

    private void Awake()
    {
        // Create a rotational matrix with 45def offset on Y
        //_isometricInputMatrix = Matrix4x4.Rotate(Quaternion.Euler(0f, 45f, 0f));
        //_characterController = GetComponent<CharacterController>();
        playerRB = GetComponent<Rigidbody>();
        //
    }

    void OnEnable()
    {        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
       // _characterController.Move(transform.position);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        camera = Camera.main;
        _characterController.enabled = false;
        Debug.Log("charcon" + transform.position);
    }

    void OnDisable()
    {
        //_characterController.enabled = false;
        Debug.Log("OnDisable");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        if (!boss1Dialogue.activeSelf)
        {
            if ((Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0) && !_characterController.enabled)
            {
                _characterController.enabled = true;
            }
            if (_characterController.enabled)
            {
                _characterController.Move(_direction * movementSpeed * Time.deltaTime);
            }

            float singleStep = movementSpeed * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, _direction, singleStep, 0.0f);
            transform.parent.rotation = Quaternion.LookRotation(newDirection);
        }
       
    }

    private Vector3 IsoVectorConvert(Vector3 vector)
    {
        Quaternion rotation = Quaternion.Euler(0,camera.transform.eulerAngles.y, 0);
        Matrix4x4 isoMatrix = Matrix4x4.Rotate(rotation);
        Vector3 result = isoMatrix.MultiplyPoint3x4(vector);
        return result;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector3 readVector = context.ReadValue<Vector3>();
        Vector3 toConvert = new Vector3(readVector.x, 0, readVector.y).normalized;

        _direction = IsoVectorConvert(toConvert);
        if (_direction == Vector3.zero)
        {
            isMoving = false;
            animCon.ChangeAnimation(animCon.Idle, 0.1f, 0, 0);
        }
        else
        {
            isMoving = true;
            animCon.ChangeAnimation(animCon.Walk, 0.03f, 0, 0);
        }
    }

    public bool GetIsMoving()
    {
        return isMoving;
    }
}
