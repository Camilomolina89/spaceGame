using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    public Vector2 moveVector;
    private BuilderCamera cameraActions;
    private InputAction movement;
    private InputAction upButton;
    private InputAction downButton;
    private Transform cameraTransform;

    [SerializeField]
    private float maxSpeed = 5f;
    private float speed = 2f;

    [SerializeField]
    private float acceleration = 10f;

    [SerializeField]
    private float damping = 15f;

    //[SerializeField]        private float stepSize = 2f;

    //[SerializeField]        private float zoomDampening = 7.5f;

    //[SerializeField]        private float minHeight = 5f;

    //[SerializeField]        private float maxHeight = 50f;

    //[SerializeField]        private float zoomSpeed = 2f;


    //[SerializeField]        private float maxRotationSpeed = 1f;
    [SerializeField]
    private float yMaxRotationLimit = 80f;

    [SerializeField]
    private float yMinRotationLimit = 80f;

    //[SerializeField]        [Range(0f,0.1f)]        private float edgeTolerance = 0.05f;

    //value set in various functions
    //used to update the position of the camera base object.
    private Vector3 targetPosition;

    //private float zoomHeight;

    //used to track and maintain velocity w/o a rigidbody
    private Vector3 horizontalVelocity;
    private Vector3 lastPosition;

    //tracks where the dragging action started
    //Vector3 startDrag;

    float xAxis = 0f;
    float yAxis = 0f;

    private void Awake()
    {
        cameraActions = new BuilderCamera();
        cameraTransform = this.GetComponentInChildren<Camera>().transform;
    }

    private void OnEnable()
    {
        lastPosition = this.transform.position;
        movement = cameraActions.Camera.Movement;
        // buttons = movement = cameraActions.Camera.Up;
        upButton = cameraActions.Camera.UpButton;
        downButton = cameraActions.Camera.downButton;
        cameraActions.Camera.Enable();
        cameraActions.Camera.RotateCamera.performed += RotateCamera;
    }

    private void OnDisable()
    {
        cameraActions.Camera.RotateCamera.performed -= RotateCamera;
        cameraActions.Disable();
    }

    void RotateCamera(InputAction.CallbackContext inputValue)
    {
        xAxis += inputValue.ReadValue<Vector2>().x;
        yAxis -= inputValue.ReadValue<Vector2>().y;
        Quaternion targetRotation;

        if (yAxis > yMaxRotationLimit)
        {
            yAxis = yMaxRotationLimit;
        }
        if (yAxis < -yMinRotationLimit)
        {
            yAxis = -yMinRotationLimit;
        }
        targetRotation =
            Quaternion.Euler(Vector3.up * xAxis) * Quaternion.Euler(Vector3.right * yAxis);
        transform.rotation = targetRotation;
    }

    private void Update()
    {
        getKeyboardMovement();
        updateVelocity();
        moveVector = movement.ReadValue<Vector2>();
        UpdateBasePosition();
    }

    void updateVelocity()
    {
        horizontalVelocity = (this.transform.position - lastPosition) / Time.deltaTime;
        horizontalVelocity.y = 0f;
        lastPosition = this.transform.position;
    }

    private void getKeyboardMovement()
    {
        Vector3 inputValue =
            movement.ReadValue<Vector2>().x * GetCameraRight()
            + movement.ReadValue<Vector2>().y * GetCameraForward();
        if (upButton.IsPressed())
        {
            inputValue.y += speed;
        }
        if (downButton.IsPressed())
        {
            inputValue.y -= speed;
        }
        inputValue = inputValue.normalized;

        if (inputValue.sqrMagnitude > 0.1f)
        {
            targetPosition += inputValue;
        }
    }

    private void UpdateBasePosition()
    {
        if (targetPosition.sqrMagnitude > 0.1f)
        {
            //create a ramp up or acceleration
            speed = Mathf.Lerp(speed, maxSpeed, Time.deltaTime * acceleration);
            transform.position += targetPosition * speed * Time.deltaTime;
        }
        else
        {
            //create smooth slow down
            horizontalVelocity = Vector3.Lerp(
                horizontalVelocity,
                Vector3.zero,
                Time.deltaTime * damping
            );
            transform.position += horizontalVelocity * Time.deltaTime;
        }

        //reset for next frame
        targetPosition = Vector3.zero;
    }

    private Vector3 GetCameraForward()
    {
        Vector3 forward = cameraTransform.forward;
        forward.y = 0f;
        return forward;
    }

    //gets the horizontal right vector of the camera
    private Vector3 GetCameraRight()
    {
        Vector3 right = cameraTransform.right;
        right.y = 0f;
        return right;
    }
}
