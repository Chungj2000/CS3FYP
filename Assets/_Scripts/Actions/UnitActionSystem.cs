using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour {

    public static UnitActionSystem INSTANCE {get; private set;}
    
    [SerializeField] private UnitHandler selectedUnit;
    [SerializeField] private LayerMask unitLayerMask;

    public event EventHandler OnSelectedUnitChanged;

    private AbstractAction currentAction;
    private bool isBusy;

    private void  Awake() {
        if(INSTANCE == null) {
            INSTANCE = this;
            //Debug.Log("UnitActionSystem instance created.");
        } else {
            Debug.LogError("More than one UnitActionSystem instance created.");
            Destroy(gameObject);
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

        if(EventSystem.current.IsPointerOverGameObject()) {
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

            //If unit has already performed an action, prevent further action.
            if(selectedUnit.IsActionUsed()) {
                Debug.Log("Unit has already performed an action.");
                return;
            }

            TilePosition mouseTilePosition = GridSystemHandler.INSTANCE.GetTilePosition(MouseHandler.INSTANCE.GetMousePosition());

            //Determine if the selected tile is valid, if so move the unit to the selected tile.
            if(selectedUnit.GetMoveAction().IsValidActionTilePosition(mouseTilePosition)) {

                SetBusy();
                selectedUnit.GetMoveAction().PrepareAction(mouseTilePosition, ClearBusy);

                //Selected unit can no longer move for this turn.
                selectedUnit.SetActionUsed();
            }
        }
    }

    private bool TryHandleUnitSelection() {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(mouseRay, out RaycastHit raycastHit, float.MaxValue, unitLayerMask)) {

            //Debug.Log("A Unit layer found.");

            //Assign unit selected unit if possible.
            if(raycastHit.transform.TryGetComponent<UnitHandler>(out UnitHandler unit)) {
                
                //Reselecting a selected unit updates visuals for targetable enemies.
                if(unit == selectedUnit) {

                    //Debug.Log("Unit already selected.");
                    SetCurrentAction(selectedUnit.GetAttackAction());
                    
                    /**
                     * Unit Test for listing enemy positions found.
                    for(int x = 0; x < currentAction.ListValidActionPositions().Count; x++) {
                        Debug.Log(currentAction.ListValidActionPositions()[x]);
                    }
                    **/

                    return false;
                    
                }

                //Cannot select an enemy unit.
                if(unit.IsEnemy()) {
                    //Debug.Log("Cannot select an enemy.");
                    return false;
                }
                
                //Debug.Log("New unit selected.");
                SetSelectedUnit(unit);
                return true;
            }

        }

        return false;
    }

    private void SetSelectedUnit(UnitHandler unit) {

        selectedUnit = unit;
        //Debug.Log("Selected unit has been changed.");

        //Default action to move when selecting a new unit.
        SetCurrentAction(unit.GetMoveAction());

        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetCurrentAction(AbstractAction action) {
        currentAction = action;
    }

    public AbstractAction GetCurrentAction() {
        return currentAction;
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

    public UnitHandler GetSelectedUnit() {
        return selectedUnit;
    }

}
