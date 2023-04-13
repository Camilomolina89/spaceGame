using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CityBuilder : MonoBehaviour
{
    public GameObject constructableBuilding;

    private Mouse mouse;

    public Button button;

    public int mode = 0;
    public GameObject temporalPrefabBuilding;

    private void Awake()
    {
        mouse = Mouse.current;
        button.onClick.AddListener(delegate ()
            {
                temporalPrefabBuilding = Instantiate(constructableBuilding);
                // permitir construír sobre slots
                mode = 1;
                button.interactable = false;
                
            });
    }
    void Update()
    {
        if (mode == 1)
        {
            var emptySlot= getSlotOnMouse();
            if (mouse.rightButton.wasPressedThisFrame){
                mode = 0;
                button.interactable = true;
                Destroy(temporalPrefabBuilding);
            }
            if (!emptySlot.found) return;
            var newPosition = emptySlot.hit.transform.position;
            temporalPrefabBuilding.transform.position = new Vector3(newPosition.x, newPosition.y + 1 , newPosition.z);
            if (mouse.leftButton.wasPressedThisFrame){
                mode = 0;
                button.interactable = true;
                temporalPrefabBuilding.transform.SetParent(emptySlot.hit.transform);
                var slot = emptySlot.hit.transform.GetComponent<BuildingSlot>();
                slot.building = temporalPrefabBuilding.GetComponent<Building>();
            }
        }
        
    }

    HitPoint getSlotOnMouse()
    {
        Camera camera = Camera.main;
        Mouse mouse = Mouse.current;
        HitPoint hitpoint = new HitPoint();
        hitpoint.found = false;
        Ray ray = camera.ScreenPointToRay(mouse.position.ReadValue());
        RaycastHit[] hits = Physics.RaycastAll(ray, 50);
        BuildingSlot slot;
        if (hits.Length == 0) return hitpoint;
        var hit = hits[hits.Length - 1];
        bool foundSlot = hit.collider.TryGetComponent<BuildingSlot>(out slot);
        if (!foundSlot) return hitpoint;
        if (slot.building != null) return hitpoint;

        // Asignación y respuesta
        hitpoint.found = true;
        hitpoint.hit = hit;
        return hitpoint;
    }
}
