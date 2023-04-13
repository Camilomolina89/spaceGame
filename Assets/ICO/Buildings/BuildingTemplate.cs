using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Building", menuName = "ScriptableObjects/BuildingTemplate"), System.Serializable]
public class BuildingTemplate : ScriptableObject
{
    public string templateName;
    public int buildingEnergyCost = 1;
    public float energyIncome = 2;
    public float energyCost = 1;
    public int slots = 1;
    public GameObject prefab;
    public GameObject temporalPrefab;
    public PlanetPanel planetPanel;
    // public BuildingData buildingData;
}
