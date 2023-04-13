using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BuildingButton : MonoBehaviour
{
    [SerializeField] public Building building;
    public delegate void BuildingButtonHandler(Building t);
    public event BuildingButtonHandler onClick;
    private void Awake() {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(delegate(){
            onClick?.Invoke(building);
        });
    }
}
