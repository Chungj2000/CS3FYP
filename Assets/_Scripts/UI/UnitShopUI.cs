using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Script handles all logic related to the UnitShopUI.
 * Allows Players to interact with visual components, which will respond based on the context.
 * Players will be able to select Units they want to purchase from their Fortress through this.
 */
public class UnitShopUI : MonoBehaviour {

    public static UnitShopUI INSTANCE {get; private set;}
    
    [SerializeField] private Canvas unitShopCanvas;

    [SerializeField] private Button close_Btn;

    [SerializeField] private Button militiaSelection_Btn;
    [SerializeField] private Button archerSelection_Btn;
    [SerializeField] private Button lightCavalrySelection_Btn;
    [SerializeField] private Button knightSelection_Btn;
    [SerializeField] private Button catapultSelection_Btn;
    [SerializeField] private Button heavyCavalrySelection_Btn;

    private Color selectedButtonColor = Color.red;
    private Color defaultButtonColor = Color.white;

    private string selectedUnitType;
    private GameObject selectedUnitPrefab;

    private void  Awake() {
        if(INSTANCE == null) {
            INSTANCE = this;
            //Debug.Log("UnitShopUI instance created.");
        } else {
            Debug.Log("More than one UnitShopUI instance created.");
            Destroy(this);
            return;
        }
    }

    private void Start() {
        Hide();
    }

    //Set the initial select Unit as the Militia.
    public void InitialiseUnitShop() {
        SetSelected_Militia();
        ToggleButtonColor(militiaSelection_Btn);
    }

    //Button functions.
    public void CloseClicked() {
        //Debug.Log("Close clicked.");
        Hide();
    }

    public void MilitiaSelectionClicked() {
        //Debug.Log("Militia Selection clicked.");

        SetSelected_Militia();
        ToggleButtonColor(militiaSelection_Btn);

        //Debug.Log("Selected set as: " + selectedUnitType);
    }

    public void ArcherSelectionClicked() {
        //Debug.Log("Archer Selection clicked.");

        SetSelected_Archer();
        ToggleButtonColor(archerSelection_Btn);

        //Debug.Log("Selected set as: " + selectedUnitType);
    }

    public void LightCavalrySelectionClicked() {
        //Debug.Log("Light Cavalry Selection clicked.");

        SetSelected_LightCavalry();
        ToggleButtonColor(lightCavalrySelection_Btn);

        //Debug.Log("Selected set as: " + selectedUnitType);
    }

    public void KnightSelectionClicked() {
        //Debug.Log("Knight Selection clicked.");

        SetSelected_Knight();
        ToggleButtonColor(knightSelection_Btn);

        //Debug.Log("Selected set as: " + selectedUnitType);
    }

    public void CatapultSelectionClicked() {
        //Debug.Log("Catapult Selection clicked.");

        SetSelected_Catapult();
        ToggleButtonColor(catapultSelection_Btn);

        //Debug.Log("Selected set as: " + selectedUnitType);
    }

    public void HeavyCavalrySelectionClicked() {
        //Debug.Log("Heavy Cavalry Selection clicked.");

        SetSelected_HeavyCavalry();
        ToggleButtonColor(heavyCavalrySelection_Btn);

        //Debug.Log("Selected set as: " + selectedUnitType);
    }

    //Change the button color to red when clicked on, and the rest to white.
    private void ToggleButtonColor(Button selectedButton) {
        militiaSelection_Btn.GetComponent<Image>().color = defaultButtonColor;
        archerSelection_Btn.GetComponent<Image>().color = defaultButtonColor;
        lightCavalrySelection_Btn.GetComponent<Image>().color = defaultButtonColor;
        knightSelection_Btn.GetComponent<Image>().color = defaultButtonColor;
        catapultSelection_Btn.GetComponent<Image>().color = defaultButtonColor;
        heavyCavalrySelection_Btn.GetComponent<Image>().color = defaultButtonColor;

        selectedButton.GetComponent<Image>().color = selectedButtonColor;
    }

    //Display the Unit parameters of a hovered over Unit in the UI.
    public void OnHoverOverSelectionButton(Button hoveredButton) {
        if(hoveredButton == militiaSelection_Btn) {

            //Debug.Log("Militia Selection button hovered over.");
            PlayerUnitUI.INSTANCE.SetSelectedUnit(UnitManager.INSTANCE.GetMilitiaPrefab().GetComponent<UnitHandler>());

        } else if (hoveredButton == archerSelection_Btn) {

            //Debug.Log("Archer Selection button hovered over.");
            PlayerUnitUI.INSTANCE.SetSelectedUnit(UnitManager.INSTANCE.GetArcherPrefab().GetComponent<UnitHandler>());

        } else if (hoveredButton == lightCavalrySelection_Btn) {

            //Debug.Log("Light Cavalry Selection button hovered over.");
            PlayerUnitUI.INSTANCE.SetSelectedUnit(UnitManager.INSTANCE.GetLightCavalryPrefab().GetComponent<UnitHandler>());

        } else if (hoveredButton == knightSelection_Btn) {

            //Debug.Log("Knight Selection button hovered over.");
            PlayerUnitUI.INSTANCE.SetSelectedUnit(UnitManager.INSTANCE.GetKnightPrefab().GetComponent<UnitHandler>());

        } else if (hoveredButton == catapultSelection_Btn) {

            //Debug.Log("Catapult Selection button hovered over.");
            PlayerUnitUI.INSTANCE.SetSelectedUnit(UnitManager.INSTANCE.GetCatapultPrefab().GetComponent<UnitHandler>());

        } else if (hoveredButton == heavyCavalrySelection_Btn) {

            //Debug.Log("Heavy Cavalry Selection button hovered over.");
            PlayerUnitUI.INSTANCE.SetSelectedUnit(UnitManager.INSTANCE.GetHeavyCavalryPrefab().GetComponent<UnitHandler>());

        }
    }

    //Visibility functions.
    public void Show() {
        unitShopCanvas.enabled = true;
    }

    public void Hide() {
        unitShopCanvas.enabled = false;
    }

    //Getters.
    public string GetSelectedUnitType() {
        return selectedUnitType;
    }

    public GameObject GetSelectedUnitPrefab() {
        return selectedUnitPrefab;
    }

    //Identify which Player the Unit is for. Aka Setters.
    private void SetSelected_Militia() {
        //Debug.Log("Militia selected.");
        if(PlayerHandler.INSTANCE.IsPlayer1()) {
            selectedUnitType = UnitManager.INSTANCE.GetMilitiaPlayer1();
        } else {
            selectedUnitType = UnitManager.INSTANCE.GetMilitiaPlayer2();
        }

        selectedUnitPrefab = UnitManager.INSTANCE.GetMilitiaPrefab();
    }

    private void SetSelected_Archer() {
        //Debug.Log("Archer selected.");
        if(PlayerHandler.INSTANCE.IsPlayer1()) {
            selectedUnitType = UnitManager.INSTANCE.GetArcherPlayer1();
        } else {
            selectedUnitType = UnitManager.INSTANCE.GetArcherPlayer2();
        }

        selectedUnitPrefab = UnitManager.INSTANCE.GetArcherPrefab();
    }

    private void SetSelected_LightCavalry() {
        //Debug.Log("Light Cavalry selected.");
        if(PlayerHandler.INSTANCE.IsPlayer1()) {
            selectedUnitType = UnitManager.INSTANCE.GetLightCavalryPlayer1();
        } else {
            selectedUnitType = UnitManager.INSTANCE.GetLightCavalryPlayer2();
        }

        selectedUnitPrefab = UnitManager.INSTANCE.GetLightCavalryPrefab();
    }

    private void SetSelected_Knight() {
        //Debug.Log("Knight selected.");
        if(PlayerHandler.INSTANCE.IsPlayer1()) {
            selectedUnitType = UnitManager.INSTANCE.GetKnightPlayer1();
        } else {
            selectedUnitType = UnitManager.INSTANCE.GetKnightPlayer2();
        }

        selectedUnitPrefab = UnitManager.INSTANCE.GetKnightPrefab();
    }

    private void SetSelected_Catapult() {
        //Debug.Log("Catapult selected.");
        if(PlayerHandler.INSTANCE.IsPlayer1()) {
            selectedUnitType = UnitManager.INSTANCE.GetCatapultPlayer1();
        } else {
            selectedUnitType = UnitManager.INSTANCE.GetCatapultPlayer2();
        }

        selectedUnitPrefab = UnitManager.INSTANCE.GetCatapultPrefab();
    }

    private void SetSelected_HeavyCavalry() {
        //Debug.Log("Heavy Cavalry selected.");
        if(PlayerHandler.INSTANCE.IsPlayer1()) {
            selectedUnitType = UnitManager.INSTANCE.GetHeavyCavalryPlayer1();
        } else {
            selectedUnitType = UnitManager.INSTANCE.GetHeavyCavalryPlayer2();
        }

        selectedUnitPrefab = UnitManager.INSTANCE.GetHeavyCavalryPrefab();
    }

}
