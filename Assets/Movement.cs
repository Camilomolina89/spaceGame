using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    Keyboard keyboard;
    public float forward;
    public float backward;
    public float left;
    public float right;
    public float rotateRight;
    public float rotateLeft;
    public float lift;
    public float deep;



    private Rigidbody rigidBody;
    public int force = 1;
    public ForceMode forceMode = ForceMode.Acceleration;
    public TextMeshProUGUI textMesh;
    // Start is called before the first frame update
    private void Awake() {
        rigidBody = GetComponent<Rigidbody>();
    }
    void Start()
    {
        keyboard = Keyboard.current;
        
    }

    // Update is called once per frame
    void Update()
    {
        checkInput();

        if (forward>0) {
            rigidBody.AddForce( force * transform.forward, forceMode);
        }
        if (backward>0) {
            rigidBody.AddForce( force * -transform.forward, forceMode);
        }
        if (left>0) {
            rigidBody.AddForce( force * -transform.right, forceMode);
        }
        if (right>0) {
            rigidBody.AddForce( force * transform.right, forceMode);
        }
        if (rotateLeft>0){
            rigidBody.AddTorque(force * -transform.up, forceMode);
        }
        if (rotateRight>0){
            rigidBody.AddTorque(force * transform.up, forceMode);
        }
        if (lift>0){
            rigidBody.AddForce( force * transform.up, forceMode);
        }
        if (deep>0){
            rigidBody.AddForce( force * -transform.up, forceMode);
        }
        textMesh.text = rigidBody.velocity.ToString();
        textMesh.text = rigidBody.velocity.magnitude.ToString();
        // textMesh.text  = rigidBody.GetPointVelocity().ToString();
    }
    void checkInput() {
        forward =keyboard.wKey.ReadValue();
        backward =keyboard.sKey.ReadValue();
        left =keyboard.aKey.ReadValue();
        right =keyboard.dKey.ReadValue();
        rotateRight =keyboard.eKey.ReadValue();
        rotateLeft =keyboard.qKey.ReadValue();
        lift =keyboard.spaceKey.ReadValue();
        deep=keyboard.shiftKey.ReadValue();
    }
}
