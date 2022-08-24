using UnityEngine;
using UnityEngine.InputSystem;


public class CameraMovement : MonoBehaviour
{
    public Vector2 moveVector;
    private BuilderCamera cameraActions;
        private InputAction movement;
        private Transform cameraTransform;

        
        [SerializeField]
        private float maxSpeed = 5f;
        private float speed = 2f;
        
        [SerializeField]
        private float acceleration = 10f;
        
        [SerializeField]
        private float damping = 15f;

        
        [SerializeField]
        private float stepSize = 2f;
        
        [SerializeField]
        private float zoomDampening = 7.5f;
        
        [SerializeField]
        private float minHeight = 5f;
        
        [SerializeField]
        private float maxHeight = 50f;
        
        [SerializeField]
        private float zoomSpeed = 2f;

        
        [SerializeField]
        private float maxRotationSpeed = 1f;
        [SerializeField] private float yRotationLimit = 80f;

        
        [SerializeField]
        [Range(0f,0.1f)]
        private float edgeTolerance = 0.05f;

        //value set in various functions 
        //used to update the position of the camera base object.
        private Vector3 targetPosition;

        private float zoomHeight;

        //used to track and maintain velocity w/o a rigidbody
        private Vector3 horizontalVelocity;
        private Vector3 lastPosition;

        //tracks where the dragging action started
        Vector3 startDrag;

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
        cameraActions.Camera.Enable();
        cameraActions.Camera.RotateCamera.performed  += RotateCamera;
    }
    private void OnDisable()
    {
        cameraActions.Camera.RotateCamera.performed -= RotateCamera;
        cameraActions.Disable();
    }
    void RotateCamera(InputAction.CallbackContext  inputValue) {
        // Vector2 rotation = Vector2.zero;
        /* if (!Mouse.current.middleButton.isPressed) {
            return;
        } */
        xAxis += inputValue.ReadValue<Vector2>().x;
        yAxis -= inputValue.ReadValue<Vector2>().y;
        // float value = inputValue.ReadValue<Vector2>().x;
        // float valueY = inputValue.ReadValue<Vector2>().y;
        // float valueY = Mathf.Clamp( inputValue.ReadValue<Vector2>().y, -yRotationLimit, yRotationLimit);
       /*  Quaternion rotation = Quaternion.Euler(-yAxis * maxRotationSpeed + transform.rotation.eulerAngles.x, xAxis * maxRotationSpeed + transform.rotation.eulerAngles.y, 0f);
        // rotation.y = Mathf.Clamp(rotation.y, -yRotationLimit, yRotationLimit);
        // var eulerRot = rotation.eulerAngles;
        // eulerRot.z = Mathf.Clamp(rotation.z, -yRotationLimit, yRotationLimit);
        if (rotation.eulerAngles.y > yRotationLimit) {
            rotation
        }
        transform.localRotation = rotation; */
        Quaternion targetRotation;
        
        if (yAxis > yRotationLimit) {
            yAxis = yRotationLimit;

        }
        if (yAxis< -yRotationLimit) {
            yAxis = -yRotationLimit;
        }
        targetRotation = Quaternion.Euler(Vector3.up * xAxis) * Quaternion.Euler(Vector3.right * yAxis);
        Debug.Log(targetRotation.eulerAngles.y);
        transform.rotation = targetRotation;
        // rotation.x += xAxis * maxRotationSpeed;
		// rotation.y += yAxis * maxRotationSpeed;
		// rotation.y = Mathf.Clamp(rotation.y, -yRotationLimit, yRotationLimit);
		// var xQuat = Quaternion.AngleAxis(rotation.x, Vector3.up);
		// var yQuat = Quaternion.AngleAxis(rotation.y, Vector3.left);

		// transform.rotation = transform.rotation * (xQuat * yQuat); //Quaternions seem to rotate more cons
        //transform.localRotation = Quaternion.Euler(-rotation.x * maxRotationSpeed + transform.rotation.eulerAngles.x, rotation.y * maxRotationSpeed + transform.rotation.eulerAngles.y, 0f);

    }
    private void Update() {
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
        Vector3 inputValue = movement.ReadValue<Vector2>().x * GetCameraRight()
                    + movement.ReadValue<Vector2>().y * GetCameraForward();

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
                horizontalVelocity = Vector3.Lerp(horizontalVelocity, Vector3.zero, Time.deltaTime * damping);
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