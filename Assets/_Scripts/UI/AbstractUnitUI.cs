using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
 * An Abstract class which serves as the base Script for UnitUIs.
 * Contains functions for handling UI elements such as visibility, formatting, and updating visual components.
 */
public abstract class AbstractUnitUI : MonoBehaviour {
    
    [SerializeField] protected Canvas unitUICanvas;
    [SerializeField] protected Image background;

    [SerializeField] protected TextMeshProUGUI unitName_Text;

    [SerializeField] protected TextMeshProUGUI unitHP_Text;
    [SerializeField] protected TextMeshProUGUI unitATK_Text;
    [SerializeField] protected TextMeshProUGUI unitDEF_Text;
    [SerializeField] protected TextMeshProUGUI unitMOVE_Text;
    [SerializeField] protected TextMeshProUGUI unitATK_RANGE_Text;
    [SerializeField] protected TextMeshProUGUI unitGOLD_COST_Text;
    [SerializeField] protected TextMeshProUGUI unitWEAPON_TYPE_Text;
    [SerializeField] protected TextMeshProUGUI unitARMOUR_TYPE_Text;
    [SerializeField] protected TextMeshProUGUI playerTOTAL_GOLD_Text;

    protected bool isShowing = false;

    protected Color32 player1Color;
    protected Color32 player2Color;

    protected UnitHandler selectedUnit;

    private void Start() {
        HideUnitUI();
        player1Color = new Color32(17, 90, 179, 255);
        player2Color = new Color32(168, 36, 19, 255);
    }

    //Set the background color of the UI depending on Player.
    protected abstract void SetBackgroundColor();

    public void InitialiseUI() {
        SetBackgroundColor();
    }

    //Allows for UnitUI to display information regarding a selected unit by setting the selected unit.
    public void SetSelectedUnit(UnitHandler unit) {
        selectedUnit = unit;
        //Debug.Log("Selected unit set.");

        UpdateCanvasFields();
        //Debug.Log("Fields updated.");

        //Show the UI if it's concealed.
        if(!isShowing) {
            ShowUnitUI();
        }
    }

    //Changes text field values when selected unit is changed.
    protected void UpdateCanvasFields() {
        //Set text fields for name using the selected unit.
        unitName_Text.text = selectedUnit.tag;

        //Set text fields for parameters using the selected unit.
        unitHP_Text.text = selectedUnit.GetParamHP().ToString();
        unitATK_Text.text = selectedUnit.GetParamATK().ToString();
        unitDEF_Text.text = selectedUnit.GetParamDEF().ToString();
        unitMOVE_Text.text = selectedUnit.GetParamMOVE().ToString();
        unitATK_RANGE_Text.text = selectedUnit.GetParamATK_RANGE().ToString();
        unitGOLD_COST_Text.text = selectedUnit.GetParamGOLD_COST().ToString();
        unitWEAPON_TYPE_Text.text = selectedUnit.GetWeaponTypeToString();
        unitARMOUR_TYPE_Text.text = selectedUnit.GetArmourTypeToString();

        UpdateTotalGoldField();
    }

    //Resets UnitUI fields.
    protected void ClearCanvasFields() {
        //Set text fields for name using the selected unit.
        unitName_Text.text = "";

        //Set text fields for parameters using the selected unit.
        unitHP_Text.text = "";
        unitATK_Text.text = "";
        unitDEF_Text.text = "";
        unitMOVE_Text.text = "";
        unitATK_RANGE_Text.text = "";
        unitGOLD_COST_Text.text = "";
        unitWEAPON_TYPE_Text.text = "";
        unitARMOUR_TYPE_Text.text = "";

        playerTOTAL_GOLD_Text.text = "";

        selectedUnit = null;
    }

    //Hides the UnitUI and resets fields.
    public void ConcealUnitUI() {
        ClearCanvasFields();
        HideUnitUI();
    }

    //For whenever health value changes.
    public void UpdateHealthField() {
        unitHP_Text.text = selectedUnit.GetParamHP().ToString();
    }

    //For whenever gold value changes.
    public abstract void UpdateTotalGoldField();

    //Visibility functions.
    protected void ShowUnitUI() {
        unitUICanvas.enabled = true;
        isShowing = true;
    }

    protected void HideUnitUI() {
        unitUICanvas.enabled = false;
        isShowing = false;
    }

    public bool IsShowing() {
        return isShowing;
    }

    //Getters.
    public UnitHandler GetSelectedUnit() {
        return selectedUnit;
    }

}
