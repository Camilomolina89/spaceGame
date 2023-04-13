using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public class City : MonoBehaviour
{
    public List<BuildingSlot> buildingSlots = new List<BuildingSlot>();
    public float totalResource = 0;
    public TextMeshProUGUI resourceText;
    private void OnEnable() {
        World.onTick += hacerCosas;
    }
    void hacerCosas() {
        buildingSlots.Clear();
        foreach (Transform child in transform) {
        
        BuildingSlot slot;
        
            bool hasSlot = child.TryGetComponent<BuildingSlot>(out slot);
            if (!hasSlot) continue;
            buildingSlots.Add(slot);
        }
        foreach (var slot in buildingSlots)
        {
            var buildign = slot.building;
            if (buildign==null) {
                continue;
            }
            // totalResource += (buildign.energyIncome - buildign.energyCost);
            resourceText.text = "Recursos: " + totalResource.ToString();
        }
    }
}
