using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private BuilderCamera cameraActions;
    private Transform cameraTransform;
    

    float xAxis = 0f;
    float yAxis = 0f;


    
    private InputAction movement;

    [SerializeField]
    private float yMaxRotationLimit = 80f;

    [SerializeField]
    private float yMinRotationLimit = 80f;

    private void Awake()
    {
        cameraActions = new BuilderCamera();
        cameraTransform = this.GetComponentInChildren<Camera>().transform;
    }


    private void OnEnable()
    {
        movement = cameraActions.Camera.Movement;
        cameraActions.Camera.Enable();
    }

    private void OnDisable()
    {
        /* cameraActions.Camera.RotateCamera.performed -= RotateCamera;
        cameraActions.Disable(); */
    }

    void rotateCamera()
    {
        xAxis -= movement.ReadValue<Vector2>().x;
        yAxis += movement.ReadValue<Vector2>().y;

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
        rotateCamera();
    }

    private Vector3 GetCameraForward()
    {
        Vector3 forward = cameraTransform.forward;
        forward.y = 0f;
        return forward;
    }
    private Vector3 GetCameraRight()
    {
        Vector3 right = cameraTransform.right;
        right.y = 0f;
        return right;
    }
}
