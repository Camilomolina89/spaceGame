using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Builder : MonoBehaviour
{
    private GameObject hitObject;
    public GameObject boxModule;
    private Mouse mouse;
    Camera camera;
    [Range(0f, 90f)][SerializeField] float yRotationLimit = 88f;

    BuilderCamera cameraActions;
    Transform cameraTransform;
    Vector3 lastPosition;
    Vector3 targetPosition;
    Vector3 horizontalVelocity;
    private void Awake() {
        mouse = Mouse.current;
        cameraActions = new BuilderCamera();
        cameraTransform = GetComponentInChildren<Camera>().transform;
    }
 

    void Update() {
        RaycastHit hit;
        Camera camera = Camera.main;

        // camera.transform.position = transform.position;
        // camera.transform.rotation = transform.rotation;

        // var mouse = Mouse.current.position.ReadValue();
        
        Ray ray = Camera.main.ScreenPointToRay(mouse.position.ReadValue());
        if(Physics.Raycast(ray, out hit)) {
            hitObject = hit.collider.gameObject;
            BaseModule baseModule;
            bool hasModule = hitObject.TryGetComponent<BaseModule>(out baseModule);
            if (hasModule) {
                baseModule.selected = true;
                Vector3 spawnPoint = hitObject.transform.position + hit.normal;
                if (mouse.leftButton.wasPressedThisFrame) {
                    GameObject box = Instantiate(boxModule, spawnPoint, Quaternion.identity);
                    Rigidbody childRigidBody = hitObject.GetComponent<Rigidbody>();
                    FixedJoint joint = box.AddComponent<FixedJoint>();
                    joint.connectedBody = childRigidBody;
                }
            }
        }
        movement();
    }
    void movement(){
        horizontalVelocity = (this.transform.position - lastPosition) / Time.deltaTime;
        horizontalVelocity.y = 0;
        lastPosition = this.transform.position;
        
       /*  // camera.transform.LookAt(mouse.position.ReadValue());
        Vector3 look = Vector3.zero;
        // look.x = mouse.position.ReadValue().x;
        // mouse.position.
        look.x = camera.ScreenToViewportPoint(mouse.position.ReadValue()).x;
        // look.y = mouse.position.ReadValue().y;
        look.y = camera.ScreenToViewportPoint(mouse.position.ReadValue()).y;
        look.y = Mathf.Clamp(look.y, -yRotationLimit, yRotationLimit);
        // look.y = Mathf.Clamp(look.y, -  )

        Quaternion xQuaternion = Quaternion.AngleAxis(look.x, Vector3.up);
        Quaternion yQuaternion = Quaternion.AngleAxis(look.y, Vector3.left);


        transform.localRotation = xQuaternion * yQuaternion; */
    }



    Vector3 getCameraRight() {
        Vector3 right = cameraTransform.right;
        right.y = 0;
        return right;
    }
    Vector3 getCameraForward() {
        Vector3 forward = cameraTransform.forward;
        forward.y = 0;
        return forward;
    }
    void updateBasePosition() {
        if (targetPosition.sqrMagnitude > 0.1f) {
            float acceleration = 1;
            float speed = 1;
            speed = Mathf.Lerp(speed, 1, Time.deltaTime * acceleration);
            transform.position+=targetPosition * speed * Time.deltaTime;
        } else {
            float damping = 1;
            horizontalVelocity = Vector3.Lerp(horizontalVelocity, Vector3.zero, Time.deltaTime * damping);
            transform.position += horizontalVelocity * Time.deltaTime;
        }
        targetPosition = Vector3.zero;
    }
}
