using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHandler : MonoBehaviour {

    //Default Parameters for Units.
    [SerializeField] private int paramHP = 10;
    [SerializeField] private int paramATK = 5;
    [SerializeField] private int paramDEF = 2;
    [SerializeField] private int paramMOVE = 3;


    private TilePosition tilePosition;
    private MoveAction moveAction;
    private bool actionUsed;

    private void Awake() {
        moveAction = GetComponent<MoveAction>();
    }

    private void Start() {
        tilePosition = GridSystemHandler.INSTANCE.GetTilePosition(transform.position);
        GridSystemHandler.INSTANCE.AddUnitAtTilePosition(tilePosition, this);

        TurnSystem.INSTANCE.OnEndTurn += TurnSystem_OnEndTurn;
    }

    private void Update() {

        TilePosition newtilePosition = GridSystemHandler.INSTANCE.GetTilePosition(transform.position);

        if(newtilePosition != tilePosition) {
            GridSystemHandler.INSTANCE.UnitMovedTilePosition(this, tilePosition, newtilePosition);
            tilePosition = newtilePosition;
        }

    }

    public MoveAction GetMoveAction() {
        return moveAction;
    }

    public TilePosition getTilePosition() {
        return tilePosition;
    }

    public bool IsActionUsed() {
        return actionUsed;
    }

    public void SetActionUsed() {
        //Debug.Log("Unit has spent their action.");
        actionUsed = true;
    }

    public void ResetActionUsed() {
        Debug.Log("A unit has regained their action.");
        actionUsed = false;
    }

    private void TurnSystem_OnEndTurn(object sender, EventArgs e) {
        ResetActionUsed();
    }

    public int GetParamHP() {
        return paramHP;
    }

    public int GetParamATK() {
        return paramATK;
    }

    public int GetParamDEF() {
        return paramDEF;
    }

    public int GetParamMOVE() {
        return paramMOVE;
    }

}
