using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;


// [ExecuteInEditMode]
[System.Serializable]
public class GridPlanet : MonoBehaviour
{
    private BuilderCamera cameraActions;
    public PlanetData panetData;
    [SerializeField] UIController uiController;
    // public BuildingTemplate building;
    private Building building;
    public BuildingTemplate prefab;

    public enum Mode {building, selection};
    public Mode currentMode = Mode.selection;
    public PlanetPanel selectedPanel;
    private PlanetPanel previousSelectedPanel;
    private GameObject temporalBuild;
    private RaycastHit hit;
    private void Start() {
        panetData.panels = new List<PanelData>();
        cameraActions = new BuilderCamera();
        cameraActions.Camera.Enable();
        load();
        cameraActions.Camera.DoAction.performed += actionClick;
        cameraActions.Camera.UndoAction.performed += undoActionClick;
        
        foreach (var item in uiController.buildingButtons) {
            item.onClick += enableCreateBuilding;
        }
        World.onTick += updateResources;
        
        foreach( Transform child in transform) {
            PanelData p = null;
            if (panetData.panels != null ){

                p = panetData.panels.Find( pan => pan.name == child.name);
            }
            if (p==null) {
                p = new PanelData(child.gameObject.name);
                panetData.panels.Add(p);
            }
            p.obj = child.gameObject;
            p.InstantiatePreviousBuilding();
        }
    }

    private void enableCreateBuilding(Building b) {
        building = b;
        currentMode = Mode.building;
    }
    void updateResources() {
        float energy = 0f;
        float deltaNutrients = 0;
        foreach (var item in panetData.panels) {
            energy += item.building.idleEnergyProduction;
            foreach(var pop in item.pop) {
                energy+=item.building.energyIncome;
                energy -= pop.energyConsumtion;

                deltaNutrients += item.building.nutrientsProduction;
                deltaNutrients -= 1;
            }
            energy -= item.building.energyConsumtion;
            
        }
        uiController.energy = energy;
        uiController.nutrients += deltaNutrients;
        
    }
    void Update()
    {
        if (panetData.panels == null) return;
        cleanSelectedPanels();
        Mouse mouse = Mouse.current;
        bool mainPressed = mouse.leftButton.wasPressedThisFrame;
        bool secondaryPressed = mouse.rightButton.wasPressedThisFrame;
        var mainCamera = Camera.main;
        Ray ray = mainCamera.ScreenPointToRay(mouse.position.ReadValue());
        var hitRaycast = Physics.Raycast(ray, out hit); 
        if (hitRaycast && hit.collider.tag == "PlanetPanel") {
            var panel =panetData.panels.Find( panel => panel.name == hit.collider.name);
            var p = panel.obj.GetComponent<PlanetPanel>();
            if (currentMode == Mode.selection) {
                p.isSelected = true;
            } else if (currentMode == Mode.building){
                p.isBuildingOn = true;
            }
        }
        temporalBuilding();
    }
    
    void cleanSelectedPanels() {
        if (panetData.panels == null) return;
        foreach (var panel in panetData.panels) {
            if (panel.obj == null) {
                Debug.Log(panel.name);
            }
            var pan = panel.obj.GetComponent<PlanetPanel>();
            pan.isSelected = false;
            pan.isBuildingOn = false;
        }
    }
    void load() {

        var d = SaveGrid.Load();
        if (d!= null) {
            panetData = d;
        }
    }
    void createBuilding() {
        if (hit.collider == null) return;
        if ( hit.collider.tag != "PlanetPanel") return;
        PanelData panel =panetData.panels.Find( panel => panel.name == hit.collider.name);
        if (currentMode == Mode.building) {
            panel.normal = hit.normal;
            // var t = (BuildingTemplate)prefab;
            // panel.template = t;
            panel.building = building.data;
            panel.instantiateBuilding(hit);
            currentMode = Mode.selection;
        }
        SaveGrid.Save("ola", panetData);
    }
    void temporalBuilding() {
        if (hit.collider == null) return;
        if ( hit.collider.tag != "PlanetPanel") return;
        PanelData panel =panetData.panels.Find( panel => panel.name == hit.collider.name);
        panel.normal = hit.normal;
        if (temporalBuild != null) {
            panel.locateBuild(temporalBuild);
            return;
            // temporalBuild = Instantiate(building.temporalPrefab, gameObject.transform.position + selectedPanel.normal* 9 , Quaternion.identity);
        }
        if (currentMode == Mode.building) {
            // var t = (BuildingTemplate)prefab;
            // panel.template = t;
            temporalBuild = panel.instantiateTemporalBuilding(hit,building.data);
        }
    }
     void actionClick( InputAction.CallbackContext context) {
        if (currentMode == Mode.building) createBuilding();
        
        Destroy(temporalBuild);
    }
    void undoActionClick( InputAction.CallbackContext context) {
        currentMode = Mode.selection;
        Destroy(temporalBuild);
    }
}
