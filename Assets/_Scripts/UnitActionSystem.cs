using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionSystem : MonoBehaviour
{

    public static UnitActionSystem INSTANCE {get; private set;}
    public event EventHandler OnSelectedUnitChanged;
    private bool isBusy;
    
    [SerializeField] private UnitHandler selectedUnit;
    [SerializeField] private LayerMask unitLayerMask;

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

    private void Update() {

        //Prevent additional actions from being called if an action is already being executed.
        if(isBusy) {
            return;
        }

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
            if(selectedUnit.GetMoveAction().IsValidMoveAction(mouseTilePosition)) {

                SetBusy();
                selectedUnit.GetMoveAction().Move(mouseTilePosition, ClearBusy);

                //Selected unit can no longer move for this turn.
                selectedUnit.SetActionUsed();
            }
        }
    }

    private bool TryHandleUnitSelection() {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(mouseRay, out RaycastHit raycastHit, float.MaxValue, unitLayerMask)) {

            //Assign unit selected unit if possible.
            if(raycastHit.transform.TryGetComponent<UnitHandler>(out UnitHandler unit)) {
                //Debug.Log("New unit selected.");
                SetSelectedUnit(unit);
                return true;
            }

        }

        return false;
    }

    private void SetSelectedUnit(UnitHandler unit) {
        selectedUnit = unit;

        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    //Setters for single active action logic.
    private void SetBusy() {
        isBusy = true;
    }

    private void ClearBusy() {
        isBusy = false;
    }

    public UnitHandler GetSelectedUnit() {
        return selectedUnit;
    }

}
