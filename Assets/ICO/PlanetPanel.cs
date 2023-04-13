using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class PlanetPanel : MonoBehaviour
{
    // public bool occupied = false;
    public bool isSelected;
    public bool isBuildingOn;
    /* public Vector3 normal;
    public Vector3 position;
    public Quaternion rotation; */
    public List<PlanetPanel> connections = new List<PlanetPanel>();
    /* public Building building;
    public string templateName;
    public GameObject temporalPrefab;
    public GameObject tempInstance; */
    void Start()
    {
        Invoke("deactivateTrigger", 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isBuildingOn) {
            GetComponent<Renderer>().material.color = Color.blue;
        } else
        if (isSelected) {
            GetComponent<Renderer>().material.color = Color.red;
        } else {
            GetComponent<Renderer>().material.color = Color.white;
        }
    }
    void OnTriggerEnter(Collider collision)
    {
        var res = connections.Find( connection => connection.gameObject.name == collision.gameObject.name);
        if (res==null) {
            connections.Add(collision.gameObject.GetComponent<PlanetPanel>());
        }
    }
    void deactivateTrigger() {
        var sphereCollider = GetComponent<SphereCollider>();
        var rigidBody = GetComponent<Rigidbody>();

        Destroy (rigidBody);
        Destroy (sphereCollider);
    }
}
