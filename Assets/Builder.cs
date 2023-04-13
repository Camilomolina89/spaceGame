using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Builder : MonoBehaviour
{
    public GameObject constructablePart;
    private Mouse mouse;


    BuilderCamera cameraActions;
    Transform cameraTransform;

    public LayerMask layer;
    [SerializeField] GameObject baseBlock;
    private void Awake()
    {
        mouse = Mouse.current;
        cameraActions = new BuilderCamera();
        cameraTransform = Camera.main.transform;
    }


    void Update(){
        // mover click a evento
        Camera camera = Camera.main;
        RaycastHit block = getBlockOnMouse(camera);
        Vector3 spawnPoint = getSpawnPoint(block);


        if (mouse.leftButton.wasPressedThisFrame){
            GameObject box = Instantiate(constructablePart, spawnPoint, Quaternion.identity);
            Destroy(box.GetComponent<Rigidbody>());
            box.transform.SetParent(baseBlock.transform);
        }
    }
    RaycastHit getBlockOnMouse(Camera camera){
        
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(mouse.position.ReadValue());
        if (!Physics.Raycast(ray, out hit)){
            return hit;
        }
        GameObject hitObject = hit.collider.gameObject;
        BaseModule baseModule;
        bool hasModule = hitObject.TryGetComponent<BaseModule>(out baseModule);
        if (!hasModule){
            return hit;
        }
        return hit;
        
    }
    Vector3 getSpawnPoint(RaycastHit hit) {
        GameObject hitObject = hit.collider.gameObject;
        Vector3 spawnPoint = hitObject.transform.position + hit.normal;
        //RaycastHit hitFromInner;
        Gizmos.color = Color.red;
        Debug.DrawRay(hitObject.transform.position, hit.normal);
        Ray r = new Ray(hitObject.transform.position, hit.normal);
        RaycastHit hitInfo;
        if (Physics.Raycast(r, out hitInfo, layer)){
            GameObject hitRay = hitInfo.collider.gameObject;
            hitRay.GetComponent<Renderer>().material.color = Color.red;
        }
        return spawnPoint;
    }





}
