using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public class PlanetData
{
    public List<PanelData> panels = new List<PanelData>();
    
}


[System.Serializable]
public class PanelData{
    public string name;
    public BuildingData building;
    // public TemplateType type;
    [System.NonSerialized] public GameObject obj;
    public Vector3 normal;
    public Vector3 position;
    public Quaternion rotation;

    public List<Pop> pop = new List<Pop>();
    
    /* public delegate void HighlightOnMouse();
    public event HighlightOnMouse onMouse; */
    


    // public List<Panel> connections = new List<Panel>();
    public PanelData(string panelName) {
        name = panelName;
    }
    public GameObject instantiateBuilding(RaycastHit hit) {
        
        GameObject model = Resources.Load(building.modelName) as GameObject;;
        var build = GameObject.Instantiate(model, normal*9, Quaternion.identity);
        build.transform.rotation = Quaternion.LookRotation(build.transform.position + normal * 5) * Quaternion.Euler(new Vector3(90,0,0));
        build.transform.localScale = new Vector3(5,5,5);
        position = build.transform.position;
        rotation = build.transform.rotation;

        build.transform.parent = obj.transform;
        return build;
    }
    public void InstantiatePreviousBuilding() {
        if (building == null) return;
        if (building.modelName== "")  return;
        GameObject model = Resources.Load(building.modelName) as GameObject;;
        var build = GameObject.Instantiate(model, position, Quaternion.identity);
        build.transform.rotation = rotation;
        build.transform.localScale = new Vector3(5,5,5);
        position = build.transform.position;
        rotation = build.transform.rotation;

        build.transform.parent = obj.transform;
    }
    public GameObject instantiateTemporalBuilding(RaycastHit hit, BuildingData building) {
        
        GameObject model = Resources.Load(building.modelName) as GameObject;;
        var build = GameObject.Instantiate(model, normal*9, Quaternion.identity);
        build.transform.rotation = Quaternion.LookRotation(build.transform.position + normal * 5) * Quaternion.Euler(new Vector3(90,0,0));
        build.transform.localScale = new Vector3(5,5,5);
        position = build.transform.position;
        rotation = build.transform.rotation;

        return build;
    }
    public void locateBuild(GameObject build) {
        rotation = Quaternion.LookRotation(build.transform.position + normal * 5) * Quaternion.Euler(new Vector3(90,0,0));
        position = normal*9;
        build.transform.position = position;
        build.transform.rotation = rotation;
    }

    public void setSelectedColor () {
        var renderer = obj.GetComponent<Renderer>();
        renderer.material.color = Color.red;
    }

}
