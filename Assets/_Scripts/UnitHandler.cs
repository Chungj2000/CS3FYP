using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHandler : MonoBehaviour {

    //Default Parameters for Units.
    [SerializeField] private static int paramMAX_HP = 10;
    [SerializeField] private int paramHP = paramMAX_HP;
    [SerializeField] private int paramATK = 5;
    [SerializeField] private int paramDEF = 2;
    [SerializeField] private int paramMOVE = 3;
    [SerializeField] private int paramATK_RANGE = 1;

    [SerializeField] private bool isEnemy;

    private int minimumDamage = 1;

    private TilePosition tilePosition;
    private MoveAction moveAction;
    private AttackAction attackAction;
    private bool moveActionUsed;
    private bool attackActionUsed;

    public event EventHandler OnDamaged;

    private void Awake() {
        moveAction = GetComponent<MoveAction>();
        attackAction = GetComponent<AttackAction>();
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

    public AttackAction GetAttackAction() {
        return attackAction;
    }

    public TilePosition getTilePosition() {
        return tilePosition;
    }

    public Vector3 GetWorldPosition() {
        return transform.position;
    }

    public bool IsMoveActionUsed() {
        return moveActionUsed;
    }

    public void SetMoveActionUsed() {
        //Debug.Log("Unit has spent their move action.");
        moveActionUsed = true;
    }

    public bool IsAttackActionUsed() {
        return attackActionUsed;
    }

    public void SetAttackActionUsed() {
        //Debug.Log("Unit has spent their attack and move action.");
        attackActionUsed = true;

        //If a unit has attacked, they can no longer move.
        moveActionUsed = true;
    }

    public void ResetActionUsed() {

        if((!IsEnemy() && TurnSystem.INSTANCE.IsPlayer1Turn()) || 
            (IsEnemy() && !TurnSystem.INSTANCE.IsPlayer1Turn())) {

            //Reset action of Player 1's unit when on Player 1's turn and vice versa.
            
            moveActionUsed = false;
            attackActionUsed = false;
            
            //Debug.Log("A unit has regained their actions.");

        } else if ((!IsEnemy() && !TurnSystem.INSTANCE.IsPlayer1Turn()) || 
                    (IsEnemy() && TurnSystem.INSTANCE.IsPlayer1Turn())){

            //Player 1 units cannot perform actions on Player 2 turn and vice versa.
            SetAttackActionUsed();

            //Debug.Log("Action consumed.");

            return;

        } 

    }

    private void TurnSystem_OnEndTurn(object sender, EventArgs e) {
        ResetActionUsed();
    }

    public void TakeDamage(int damageValue) {

        if(damageValue - paramDEF <= 0) {
            //If enemy defence is greater than attack value, default the damage received to 1.
            paramHP -= minimumDamage;
            Debug.Log(transform + " has received minimum damage.");
        } else {
            paramHP -= Mathf.Abs(damageValue - paramDEF);
            Debug.Log(transform + " has received " + (paramDEF - damageValue) + " damage.");
        }

        OnDamaged?.Invoke(this, EventArgs.Empty);
        
        Debug.Log(transform + " has received damage. Current Unit HP at: " + paramHP);

        CheckIsDead();

    }

    private void CheckIsDead() {
        if(paramHP <= 0) {
            Debug.Log(transform + " has been killed.");
            //Remove the unit reference.
            GridSystemHandler.INSTANCE.RemoveUnitAtTilePosition(tilePosition, this);
            Destroy(gameObject);
        }
    }

    public int GetParamHP() {
        return paramHP;
    }

    public float GetCurrentHealth() {
        return (float) paramHP / paramMAX_HP;
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

    public int GetParamATK_RANGE() {
        return paramATK_RANGE;
    }

    public bool IsEnemy() {
        return isEnemy;
    }

}
