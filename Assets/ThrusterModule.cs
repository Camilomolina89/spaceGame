using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterModule : BaseModule
{
    // Start is called before the first frame update
    Rigidbody rBody;
    private void Awake() {
        rBody = transform.root.GetComponent<Rigidbody>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rBody.AddForceAtPosition(transform.forward, transform.position, ForceMode.Force);
    }
}
