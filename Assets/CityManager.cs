using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CityManager : MonoBehaviour
{
    // Start is called before the first frame update
    public int mineral;
    private Mouse mouse;
    public List<Building> buildings = new List<Building>();
    public GameObject build;
    private void Awake()
    {
        mouse = Mouse.current;
    }

    // Update is called once per frame
    void Update()
    {
        /* var b = GameObject.FindGameObjectsWithTag("building");
        buildings.Clear();
        foreach (var item in b) {
            var build = item.GetComponent<Building>();
            buildings.Add(build);
        } */
        Camera camera = Camera.main;
        HitPoint hitpoint = getBlockOnMouse(camera);
        if (hitpoint.found) {

            Debug.DrawRay(hitpoint.hit.point, hitpoint.hit.normal);
            build.transform.position = hitpoint.hit.point;
            // build.transform.rotation = Quaternion.LookRotation(hitpoint.hit.normal, Vector3.down);
            build.transform.localRotation = Quaternion.FromToRotation(Vector3.up, hitpoint.hit.normal);
        }
    }

    HitPoint getBlockOnMouse(Camera camera){
        HitPoint hitpoint = new HitPoint();
        hitpoint.found = false;
        // RaycastHit hit;
        
        Ray ray = camera.ScreenPointToRay(mouse.position.ReadValue());
        RaycastHit[] hits = Physics.RaycastAll(ray, 50);
        foreach (var h in hits) {
            if (h.collider.gameObject.name == "Sphere") {
                hitpoint.hit = h;
                hitpoint.found = true;
                return hitpoint;
            }
        }
        /* if (!Physics.Raycast(ray, out hit)){
            hitpoint.found = false;
            return hitpoint;
        }
        GameObject hitObject = hit.collider.gameObject;
        if (hitObject.name == "Sphere") {
            hitpoint.hit = hit;
            hitpoint.found = true;
            return hitpoint;
        } */
        /*
        BaseModule baseModule;
        bool hasModule = hitObject.TryGetComponent<BaseModule>(out baseModule);
        if (!hasModule){
            return hit;
        } */
        return hitpoint;
        
    }
}

public class HitPoint {
    public RaycastHit hit;
    public bool found;
}