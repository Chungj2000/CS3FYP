using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionSystem : MonoBehaviour
{

    public static UnitActionSystem INSTANCE {get; private set;}
    private bool isBusy;
    
    [SerializeField] private UnitHandler selectedUnit;
    [SerializeField] private LayerMask unitLayerMask;

    private void  Awake() {
        if(INSTANCE == null) {
            INSTANCE = this;
            Debug.Log("UnitActionSystem instance created.");
        } else {
            Debug.LogError("More than one UnitActionSystem instance created.");
            Destroy(gameObject);
            return;
        }
    }

    private void Update() {

        if(isBusy) {
            return;
        }

        //Assign a new move to position upon left clicking a target location.
        if(Input.GetMouseButtonDown(0)) {
            if(TryHandleUnitSelection()) return;

            TilePosition mouseTilePosition = GridSystemHandler.INSTANCE.GetTilePosition(MouseHandler.INSTANCE.GetMousePosition());

            if(selectedUnit.GetMoveAction().IsValidMoveAction(mouseTilePosition)) {
                SetBusy();
                selectedUnit.GetMoveAction().Move(mouseTilePosition, ClearBusy);
            }
        }
    }

    private bool TryHandleUnitSelection() {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(mouseRay, out RaycastHit raycastHit, float.MaxValue, unitLayerMask)) {

            //Assign unit selected unit if possible.
            if(raycastHit.transform.TryGetComponent<UnitHandler>(out UnitHandler unit)) {
                selectedUnit = unit;
                return true;
            }

        }

        return false;
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
