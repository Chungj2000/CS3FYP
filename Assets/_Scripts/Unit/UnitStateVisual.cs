using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Script for toggling and manipulating the Unit State Visual that every Unit Prefab has under their feet.
 * This is manipulated to display which Unit is selected as well as their available actions.
 */
public class UnitStateVisual : MonoBehaviour {
    
    [SerializeField] private UnitHandler unit;
    private MeshRenderer meshRenderer;

    //Visual states (Colour)
    private Color selectedUnit_NoActionsAvailable = Color.red;
    private Color selectedUnit_MovedButCanAttack = Color.yellow;
    private Color selectedUnit_CanMove = Color.green;
    private Color selectedUnit_IsEnemy = Color.white;

    private void Awake() {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start() {
        UnitActionSystem.INSTANCE.OnSelectedUnitStateChanged += UnitActionSystem_OnSelectedUnitStateChanged;
    }

    //Determine the visual state for the Unit Visual State mesh renderer object of a selected unit.
    private void UnitActionSystem_OnSelectedUnitStateChanged(object sender, EventArgs e) {

        if (UnitActionSystem.INSTANCE.GetSelectedUnit() == unit && 
                (unit.IsOwnedByPlayer1() != PlayerHandler.INSTANCE.IsPlayer1())) {

            //Display a white selected visual if the unit is an enemy.

            meshRenderer.enabled = true;
            meshRenderer.material.color = selectedUnit_IsEnemy;

        } else if(UnitActionSystem.INSTANCE.GetSelectedUnit() == unit && !unit.IsMoveActionUsed()) {

            //Display a green selected visual when the selected unit can still move.

            meshRenderer.enabled = true;
            meshRenderer.material.color = selectedUnit_CanMove;

        } else if (UnitActionSystem.INSTANCE.GetSelectedUnit() == unit && 
                unit.IsMoveActionUsed() && !unit.IsAttackActionUsed()) {

            //Display a yellow selected visual when the selected unit has moved but can still attack.

            meshRenderer.enabled = true;
            meshRenderer.material.color = selectedUnit_MovedButCanAttack;

        } else if (UnitActionSystem.INSTANCE.GetSelectedUnit() == unit && 
                unit.IsMoveActionUsed() && unit.IsAttackActionUsed()) {

            //Display a red selected visual when the selected unit has no more actions available.

            meshRenderer.enabled = true;
            meshRenderer.material.color = selectedUnit_NoActionsAvailable;

        } else {

            //Hide selected visual for any units not selected.
            meshRenderer.enabled = false;

        }

    }

    //Unsubsribe event when Unit is gameObject is killed/destroyed.
    private void OnDestroy() {
        //Debug.Log("Event unsubscribed.");
        UnitActionSystem.INSTANCE.OnSelectedUnitStateChanged -= UnitActionSystem_OnSelectedUnitStateChanged;
    }

}
