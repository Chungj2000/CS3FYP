using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStateVisual : MonoBehaviour {
    
    [SerializeField] private UnitHandler unit;
    private MeshRenderer meshRenderer;

    //Visual states (Colour)
    private Color selectedUnitUnavailable = Color.red;
    private Color selectedUnitAvailable = Color.green;
    private Color unitAvailable = Color.white;

    private void Awake() {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start() {
        UnitActionSystem.INSTANCE.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
    }

    //Determine the visual state for the Unit Visual State mesh renderer object.
    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e) {

        if(UnitActionSystem.INSTANCE.GetSelectedUnit() == unit && !unit.IsMoveActionUsed()) {

            //When selected and has available action, turn green.
            meshRenderer.material.color = selectedUnitAvailable;

        } else if (UnitActionSystem.INSTANCE.GetSelectedUnit() == unit && unit.IsMoveActionUsed()) {

            //When selected and doesn't have available action, turn red.
            meshRenderer.material.color = selectedUnitUnavailable;

        } else {

            //Otherwise default white for none selected units.
            meshRenderer.material.color = unitAvailable;

        }

    }

}
