using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHandler : MonoBehaviour {

    //Default Parameters for Units.
    [Header("Unit Parameters")]
    [SerializeField] private static int paramMAX_HP = 10;
    [SerializeField] private int paramHP = paramMAX_HP;
    [SerializeField] private int paramATK = 5;
    [SerializeField] private int paramDEF = 2;
    [SerializeField] private int paramMOVE = 3;
    [SerializeField] private int paramATK_RANGE = 1;
    [SerializeField] private int paramGOLD_COST = 15;

    [Header("Advanced Combat Parameters")]
    [SerializeField] private CombatDataType.RangeType rangeType;
    [SerializeField] private CombatDataType.WeaponType weaponType;
    [SerializeField] private CombatDataType.ArmourType armourType;
    [SerializeField] private bool isBuilding = false;

    [Header("Player Customize Fields")]
    [SerializeField] private bool ownedByPlayer1;
    [SerializeField] private Material player1Material;
    [SerializeField] private Material player2Material;

    private SkinnedMeshRenderer[] skinMeshRenderers;
    private UnitAnimator unitAnimator;

    private int minimumDamage = 1;

    private TilePosition tilePosition;
    private MoveAction moveAction;
    private AttackAction attackAction;
    private SummonAction summonAction;
    private bool moveActionUsed = false;
    private bool attackActionUsed = false;

    public event EventHandler OnDamaged;

    private void Awake() {

        if(!isBuilding) {
            moveAction = GetComponent<MoveAction>();
            attackAction = GetComponent<AttackAction>();
        }

        if(isBuilding) {
            summonAction = GetComponent<SummonAction>();
        }

        skinMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        unitAnimator = GetComponent<UnitAnimator>();
    }

    private void Start() {
        tilePosition = GridSystemHandler.INSTANCE.GetTilePosition(transform.position);
        GridSystemHandler.INSTANCE.AddUnitAtTilePosition(tilePosition, this);

        //Differentiate between Player units using materials.
        if(ownedByPlayer1) {
            foreach(SkinnedMeshRenderer skin in skinMeshRenderers) {
                skin.material = player1Material;
            }  
        } else {
            foreach(SkinnedMeshRenderer skin in skinMeshRenderers) {
                skin.material = player2Material;
            }
        }

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

    public SummonAction GetSummonAction() {
        return summonAction;
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

        if(IsOwnedByPlayer1() != TurnSystem.INSTANCE.IsPlayer1Turn()) {

            //Player 1 units cannot perform actions on Player 2 turn and vice versa.
            SetAttackActionUsed();

            //Debug.Log("Action consumed.");

        } else {

            //Reset action of Player 1's unit when on Player 1's turn and vice versa.
            
            moveActionUsed = false;
            attackActionUsed = false;
            
            //Debug.Log("A unit has regained their actions.");

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

        //Only update the Unit UI when it is showing otherwise it will cause an error.
        if(PlayerUnitUI.INSTANCE.IsShowing() && EnemyUnitUI.INSTANCE.IsShowing()) {
            //Update the Unit UI health when taking damage.
            UpdateUnitUI();
        }

        CheckIsDead();

    }

    private void CheckIsDead() {
        if(paramHP <= 0) {
            KillUnit();
        }
    }

    //Remove all unit references and destroy the object.
    private void KillUnit() {
        //Debug.Log(transform + " has been killed.");

        //Remove the unit references.
        GridSystemHandler.INSTANCE.RemoveUnitAtTilePosition(tilePosition, this);
        UnitManager.INSTANCE.RemoveUnitFromList(this.gameObject);

        Destroy(gameObject.GetComponent<UnitStateVisual>());
        Destroy(gameObject);
    }

    private void UpdateUnitUI() {
        if(PlayerHandler.INSTANCE.IsPlayer1() == this.IsOwnedByPlayer1()) {
            PlayerUnitUI.INSTANCE.UpdateHealthField();
        } else {
            EnemyUnitUI.INSTANCE.UpdateHealthField();
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

    public int GetParamGOLD_COST() {
        return paramGOLD_COST;
    }

    public CombatDataType.RangeType GetRangeType() {
        return rangeType;
    }

    public CombatDataType.WeaponType GetWeaponType() {
        return weaponType;
    }

    public CombatDataType.ArmourType GetArmourType() {
        return armourType;
    }

    public bool IsOwnedByPlayer1() {
        return ownedByPlayer1;
    }

    public bool IsBuilding() {
        return isBuilding;
    }

    public UnitAnimator GetUnitAnimator() {
        return unitAnimator;
    }

}
