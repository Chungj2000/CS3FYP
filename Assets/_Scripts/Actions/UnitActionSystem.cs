using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/*
 * Script used to handle unit and action selection.
 * Contains logic for determining what Unit is selected via mouse input.
 * Additionally handles what actions to take based on selected unit and their available actions.
 */
public class UnitActionSystem : MonoBehaviour {

    public static UnitActionSystem INSTANCE {get; private set;}
    
    [SerializeField] private UnitHandler selectedUnit;
    [SerializeField] private LayerMask unitLayerMask;

    public event EventHandler OnSelectedUnitStateChanged;

    private AbstractAction currentAction;
    private bool isBusy;

    private void  Awake() {
        if(INSTANCE == null) {
            INSTANCE = this;
            //Debug.Log("UnitActionSystem instance created.");
        } else {
            Debug.LogError("More than one UnitActionSystem instance created.");
            Destroy(this);
            return;
        }
    }

    private void Start() {
        SetSelectedUnit(selectedUnit);
    }

    private void Update() {

        //Prevent additional actions from being called if an action is already being executed.
        if(isBusy) {
            //Debug.Log("An action is already being run.");
            return;
        }

        //Prevent action when clicking through a UI component.
        if(EventSystem.current.IsPointerOverGameObject()) {
            return;
        }

        //Prevent action game is over.
        if(GameOverHandler.INSTANCE.CheckGameIsOver()) {
            return;
        }

        /**
         * Prevent Player 1 from interacting with units on Player 2 turn.
         *
        if(!TurnSystem.INSTANCE.IsPlayer1Turn()) {
            return;
        }
        **/

        //Assign a new move to position upon left clicking a target location.
        if(Input.GetMouseButtonDown(0)) {

            //Debug.Log("Mouse triggered.");

            if(TryHandleUnitSelection()) return;

            HandleCurrentAction();
        }

    }

    //Determine what Unit has been selected, and handle relevant logic e.g. UIs.
    private bool TryHandleUnitSelection() {

        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(mouseRay, out RaycastHit raycastHit, float.MaxValue, unitLayerMask)) {

            //Debug.Log("A Unit layer found.");

            //Assign unit selected unit if possible.
            if(raycastHit.transform.TryGetComponent<UnitHandler>(out UnitHandler unit)) {
                
                //Reselecting a selected unit updates visuals for targetable enemies for AttackActions.
                if(unit == selectedUnit) {

                    //Debug.Log("Unit already selected.");

                    //Determine available action based on whether the unit is a building or not.
                    if(unit.IsBuilding()) {

                        //Debug.Log("Unit is a building.");

                        SetCurrentAction(selectedUnit.GetSummonAction());

                        DisplayUnitShopUI();
                        
                    } else {

                        //Debug.Log("Unit is not a building.");

                        //If selectable targets for attack are found set current action to Attack else default to Move.
                        if(unit.GetAttackAction().ListValidActionPositions().Count > 0) {

                            //Logic for selecting and deselecting the attack action when reclicking the selected unit.
                            if(currentAction == selectedUnit.GetAttackAction()) {
                                //If already selected, deselect attack action.
                                SetCurrentAction(selectedUnit.GetMoveAction());
                            } else {
                                //Else select attack action.
                                SetCurrentAction(selectedUnit.GetAttackAction());
                            }
                            
                            //Debug.Log("Viable attack targets found. Current Action: " + currentAction);

                        } else {

                            SetCurrentAction(selectedUnit.GetMoveAction());
                            //Debug.Log("Unable to find viable targets. Current Action: " + currentAction);

                        }

                        /**
                        * Unit Test for listing enemy positions found.
                        for(int x = 0; x < currentAction.ListValidActionPositions().Count; x++) {
                            Debug.Log(currentAction.ListValidActionPositions()[x]);
                        }
                        **/
                        }

                    return false;
                    
                }

                //Player cannot select units when it's not their turn.
                if(PlayerHandler.INSTANCE.IsPlayer1() != TurnSystem.INSTANCE.IsPlayer1Turn()) {
                    //Debug.Log("Cannot select an enemy.");
                    return false;
                }
                
                //Debug.Log("New unit selected.");
                SetSelectedUnit(unit);

                //Display the selected unit in the UI.
                DisplayUnitUI();
                
                return true;
            }

        }

        return false;
    }

    //Keep track of what Unit is selected for action logic.
    private void SetSelectedUnit(UnitHandler unit) {

        selectedUnit = unit;
        //Debug.Log("Selected unit has been changed.");

        //Identify what default action to set the Unit to have based on if it's a building or not.
        if(!unit.IsBuilding()) {
            //Default action to move when selecting a new unit.
            SetCurrentAction(unit.GetMoveAction());

        } else {
            //Default action to summon for buildings.
            SetCurrentAction(unit.GetSummonAction());

            DisplayUnitShopUI();
        }

        OnSelectedUnitStateChanged?.Invoke(this, EventArgs.Empty);
    }

    //Identify what action to take based on the currently selected Unit, and their available actions.
    private void HandleCurrentAction() {

        TilePosition mouseTilePosition = GridSystemHandler.INSTANCE.GetTilePosition(MouseHandler.INSTANCE.GetMousePosition());

        //If the unit is a building allow summon action.
        if(selectedUnit.IsBuilding()) {

            Debug.Log("Readying summon.");

            if(TurnSystem.INSTANCE.IsPlayer1Turn() == PlayerHandler.INSTANCE.IsPlayer1() &&
                PlayerHandler.INSTANCE.IsPlayer1() == selectedUnit.IsOwnedByPlayer1() &&
                currentAction == selectedUnit.GetSummonAction()) {

                Debug.Log(currentAction);

                if(selectedUnit.GetSummonAction().IsValidActionTilePosition(mouseTilePosition)) {

                    SetBusy();
                    selectedUnit.GetSummonAction().PrepareAction(mouseTilePosition, ClearBusy);
                    Debug.Log("Summon performed.");
                }
            }
        }

        //If unit has already performed an attack action, prevent further actions.
        if(selectedUnit.IsAttackActionUsed()) {
            OnSelectedUnitStateChanged?.Invoke(this, EventArgs.Empty);

            //Debug.Log("Unit has already performed an attack action.");
            return;
        }

        //Allow attack actions if currently selecting a Player owned unit on their turn.
        //If current action is Attack execute attack logic.

        if(TurnSystem.INSTANCE.IsPlayer1Turn() == PlayerHandler.INSTANCE.IsPlayer1() &&
            PlayerHandler.INSTANCE.IsPlayer1() == selectedUnit.IsOwnedByPlayer1() &&
            currentAction == selectedUnit.GetAttackAction()) {

            //Debug.Log(currentAction);

            if(selectedUnit.GetAttackAction().IsValidActionTilePosition(mouseTilePosition)) {

                SetBusy();
                selectedUnit.GetAttackAction().PrepareAction(mouseTilePosition, ClearBusy);
                //Debug.Log("Attack performed.");

                //Selected unit can no longer attack or move for this turn.
                selectedUnit.SetAttackActionUsed();
            }
        }

        //If unit has already performed a move action, prevent further move actions.
        if(selectedUnit.IsMoveActionUsed()) {
            OnSelectedUnitStateChanged?.Invoke(this, EventArgs.Empty);
            
            //Debug.Log("Unit has already performed a move action.");
            return;
        }

        //If current action is Move execute attack logic.
        if(TurnSystem.INSTANCE.IsPlayer1Turn() == PlayerHandler.INSTANCE.IsPlayer1() &&
            PlayerHandler.INSTANCE.IsPlayer1() == selectedUnit.IsOwnedByPlayer1() &&
            currentAction == selectedUnit.GetMoveAction()) {

            //Debug.Log(currentAction);

            //Determine if the selected tile is valid, if so move the unit to the selected tile.
            if(selectedUnit.GetMoveAction().IsValidActionTilePosition(mouseTilePosition)) {

                SetBusy();
                selectedUnit.GetMoveAction().PrepareAction(mouseTilePosition, ClearBusy);
                //Debug.Log("Move performed.");

                //Selected unit can no longer move for this turn.
                selectedUnit.SetMoveActionUsed();
            }
        }

    }

    //Display the UnitUI for specified Player relationship.
    private void DisplayUnitUI() {
        if(PlayerHandler.INSTANCE.IsPlayer1() == selectedUnit.IsOwnedByPlayer1()) {
            PlayerUnitUI.INSTANCE.SetSelectedUnit(selectedUnit);
        } else {
            EnemyUnitUI.INSTANCE.SetSelectedUnit(selectedUnit);
        }
    }

    //Display the UnitShopUI if it is your unit.
    private void DisplayUnitShopUI() {
        if(selectedUnit.IsOwnedByPlayer1() == PlayerHandler.INSTANCE.IsPlayer1()) {
            UnitShopUI.INSTANCE.Show();
        }
    }

    //Getters & Setters.
    public AbstractAction GetCurrentAction() {
        return currentAction;
    }

    public UnitHandler GetSelectedUnit() {
        return selectedUnit;
    }

    public void SetCurrentAction(AbstractAction action) {
        currentAction = action;
    }


    //Setters for single active action logic.
    private void SetBusy() {
        isBusy = true;
        //Debug.Log("An action is being run.");
    }

    private void ClearBusy() {
        isBusy = false;
        //Debug.Log("Action is now available.");
    }

}
