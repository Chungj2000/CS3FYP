using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class AbstractUnitUI : MonoBehaviour {
    
    [SerializeField] protected Canvas unitUICanvas;

    [SerializeField] protected TextMeshProUGUI unitName_Text;

    [SerializeField] protected TextMeshProUGUI unitHP_Text;
    [SerializeField] protected TextMeshProUGUI unitATK_Text;
    [SerializeField] protected TextMeshProUGUI unitDEF_Text;
    [SerializeField] protected TextMeshProUGUI unitMOVE_Text;
    [SerializeField] protected TextMeshProUGUI unitATK_RANGE_Text;

    protected bool isShowing = false;

    protected UnitHandler selectedUnit;

    private void Start() {
        HideUnitUI();
    }

    public void SetSelectedUnit(UnitHandler unit) {
        selectedUnit = unit;
        Debug.Log("Selected unit set.");

        UpdateCanvasFields();
        Debug.Log("Fields updated.");

        //Show the UI if it's concealed.
        if(!isShowing) {
            ShowUnitUI();
        }
    }

    protected void UpdateCanvasFields() {
        //Set text fields for name using the selected unit.
        unitName_Text.text = selectedUnit.tag;

        //Set text fields for parameters using the selected unit.
        unitHP_Text.text = selectedUnit.GetParamHP().ToString();
        unitATK_Text.text = selectedUnit.GetParamATK().ToString();
        unitDEF_Text.text = selectedUnit.GetParamDEF().ToString();
        unitMOVE_Text.text = selectedUnit.GetParamMOVE().ToString();
        unitATK_RANGE_Text.text = selectedUnit.GetParamATK_RANGE().ToString();
    }

    //For whenever health value changes.
    public void UpdateHealthField() {
        unitHP_Text.text = selectedUnit.GetParamHP().ToString();
    }

    protected void ShowUnitUI() {
        unitUICanvas.enabled = true;
        isShowing = true;
    }

    protected void HideUnitUI() {
        unitUICanvas.enabled = false;
        isShowing = false;
    }

}
