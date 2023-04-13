using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UIController : MonoBehaviour
{
    // [SerializeField] Button createButton;
    [SerializeField] TextMeshProUGUI energyResourceText;
    [SerializeField] TextMeshProUGUI nutrientsResourceText;
    /* public delegate void CreateButtonHandler(BuildingTemplate t);
    public event CreateButtonHandler onCreateButton; */

    [SerializeField] public float energy = 0;
    [SerializeField] public float nutrients = 0;
    public List<BuildingButton> buildingButtons = new List<BuildingButton>();
    /* private void Awake() {
        createButton.onClick.AddListener(delegate()
        {
            
            onCreateButton?.Invoke(building);
        });
    }
     */
    private void Update() {
        energyResourceText.text = "Energy: "+energy.ToString();
        nutrientsResourceText.text = "Nutrients: "+nutrients.ToString();
    }
    private void Start() {
        foreach (Transform child in transform)
        {
            if (child.gameObject.TryGetComponent<BuildingButton>(out BuildingButton button)) {
                buildingButtons.Add(button);
                
            }
        }
    }
}
