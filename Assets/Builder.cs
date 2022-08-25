using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Builder : MonoBehaviour
{
    // private GameObject hitObject;
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
        cameraTransform = Camera.main.transform;
    }
 

    void Update() {
        RaycastHit hit;
        Camera camera = Camera.main;
        
        Ray ray = Camera.main.ScreenPointToRay(mouse.position.ReadValue());
        if(Physics.Raycast(ray, out hit)) {
            var hitObject = hit.collider.gameObject;
            BaseModule baseModule;
            bool hasModule = hitObject.TryGetComponent<BaseModule>(out baseModule);
            if (hasModule) {
                baseModule.selected = true;
                Vector3 spawnPoint = hitObject.transform.position + hit.normal;
                if (mouse.leftButton.wasPressedThisFrame) {
                    GameObject box = Instantiate(boxModule, spawnPoint, Quaternion.identity);
                    connectNewBlock(box);
                }
            }
        }
    }
    void connectNewBlock(GameObject box) {
        // Rigidbody childRigidBody = hitObject.GetComponent<Rigidbody>();
        
        // joint.connectedBody = childRigidBody;
        var directions = new List<Vector3>();
        directions.Add(Vector3.up);
        directions.Add(Vector3.right);
        directions.Add(Vector3.forward);
        directions.Add(-Vector3.up);
        directions.Add(-Vector3.right);
        directions.Add(-Vector3.forward);
        foreach (var rayDirection in directions) {
            RaycastHit hit;
            bool isHit =Physics.Raycast(transform.position, rayDirection, out hit);
            if (isHit) {
                BaseModule baseModule;
                var hitObject = hit.collider.gameObject;
                bool hasModule = hitObject.TryGetComponent<BaseModule>(out baseModule);
                if (hasModule) {

                    FixedJoint joint = box.AddComponent<FixedJoint>();
                    var childRigidBody = hitObject.GetComponent<Rigidbody>();
                    joint.connectedBody = childRigidBody;
                }
            }
        }
    }


}
