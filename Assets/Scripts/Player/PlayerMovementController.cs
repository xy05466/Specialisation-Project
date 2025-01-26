using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour, PlayerInput.IPlayerActions
{
    private Vector3 _direction;
    private Quaternion _rotation;

    public CharacterController _characterController;
    private bool isMoving;

    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private AnimationController animCon;
    private Camera camera;

    private void Start()
    {
        camera = Camera.main;
    }

    private void Awake()
    {
        // Create a rotational matrix with 45def offset on Y
        //_isometricInputMatrix = Matrix4x4.Rotate(Quaternion.Euler(0f, 45f, 0f));
        //_characterController = GetComponent<CharacterController>();
        camera = Camera.main;
    }

    private void Update()
    {
        _characterController.Move(_direction * movementSpeed * Time.deltaTime);
        float singleStep = movementSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, _direction, singleStep, 0.0f);
        transform.parent.rotation = Quaternion.LookRotation(newDirection);
    }

    public void UpdateMovement()
    {
        _characterController.Move(_direction * movementSpeed * Time.deltaTime);
        
        float singleStep = movementSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, _direction, singleStep, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    //public void GatherInput(InputAction.CallbackContext context)
    //{
    //    // Get user input
    //    _input = context.ReadValue<Vector2>();

    //    // Convert into isometric coordinates
    //    _direction = _isometricInputMatrix.MultiplyPoint3x4(new Vector3(_input.x, 0.0f, _input.y));
    //}

    //void Look()
    //{
    //    var relative = (transform.position + _direction) - transform.position;
    //    _rotation = Quaternion.LookRotation(relative, Vector3.up);
    //}

    //public void Move()
    //{
    //    transform.rotation = Quaternion.Lerp(transform.rotation, _rotation, Time.deltaTime * rotationSpeed).normalized;
    //    _characterController.Move(_direction * movementSpeed * Time.deltaTime);
    //}

    public void OnNewaction(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
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
