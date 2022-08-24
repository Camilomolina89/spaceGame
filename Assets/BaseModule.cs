using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseModule : MonoBehaviour
{
    Rigidbody rigidbody;
    public bool selected;
    private void Awake() {
        if (GetComponent<Rigidbody>() == null) {
            gameObject.AddComponent<Rigidbody>();
        }
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.useGravity = false;
        // rigidbody.angularVelocity = Vector3.down;
        
    }

    // Update is called once per frame
    void Update()
    {
        var renderer = GetComponent<Renderer>();
        //
        if (selected) {

            renderer.material.color = Color.red;
        } else {
            renderer.material.color = Color.white;
        }
        selected = false;
    }
}
