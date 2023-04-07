using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AttackAction : AbstractAction {

    [SerializeField] private float rotateSpeed = 15f;

    private int attackRange;
    private int attackParameter;
    private Action onAttackComplete; // = 10;
    private UnitHandler targetedUnit;
    private bool isActive;

    protected override void Awake() {
        base.Awake();
        attackRange = unit.GetParamATK_RANGE();
        isActive = false;
    }

    private void Update() {

        //If action is not active, do nothing.
        if(!isActive) {
            //Debug.Log("Attack Action active: " + isActive);
            return;
        }

        if(Vector3.Angle(transform.forward, targetedUnit.GetWorldPosition()) > stoppingDistance) {

            //Update attack action on both clients.
            view.RPC(nameof(RPC_FaceTarget), RpcTarget.AllBuffered, null);

        }

    }

    [PunRPC]
    private void RPC_FaceTarget() {

        //Get the normalized value for facing the targetted unit.
        Vector3 faceTowards = (targetedUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;

        //Face the unit towards target.
        transform.forward = Vector3.Lerp(transform.forward, faceTowards, Time.deltaTime * rotateSpeed);
        //Debug.Log("Facing towards target - " + targetedUnit.GetWorldPosition() + "," + unit.GetWorldPosition());

        //Execute the attack.
        PerformAttack();
    }

    private void PerformAttack() {
        if(Vector3.Angle(transform.forward, targetedUnit.GetWorldPosition() - unit.GetWorldPosition()) <= stoppingDistance) {
            onActionComplete();
            //Debug.Log("Unit now facing target.");

            view.RPC(nameof(Attack), RpcTarget.AllBuffered, null);

            //Turn off move updates, until action is activated again.
            isActive = false;
        }
    }

    public override void PrepareAction(TilePosition position, Action onAttackComplete) {
        //Turn on action Updates.
        isActive = true;
        //Debug.Log("Attack Action active: " + isActive);

        this.onActionComplete = onAttackComplete;

        //Set targeted unit based on selected unit tile containing an enemy, for both clients.
        view.RPC(nameof(SetTarget), RpcTarget.AllBuffered, position.x, position.z);

        Debug.Log("Attacking: " + targetedUnit);
    }

    [PunRPC]
    private void SetTarget(int x, int z) {
        //RPCs cannot take custom types so recreate targetted TilePosition using primitives.
        TilePosition position = new TilePosition(x, z);
        targetedUnit = GridSystemHandler.INSTANCE.GetAUnitAtTilePosition(position);
    }

    //Create a list of positions the unit can attack.
    public override List<TilePosition> ListValidActionPositions() {

        List<TilePosition> validAttackPositionsList = new List<TilePosition>();

        TilePosition unitTilePosition = unit.getTilePosition();

        for(int x = -attackRange; x <= attackRange; x++) {
            for(int z = -attackRange; z <= attackRange; z++) {

                TilePosition offsetTilePosition = new TilePosition(x, z);
                TilePosition testTilePosition = unitTilePosition + offsetTilePosition;

                //Validate positions. Invalid tile positions are passed.
                if(!GridSystemHandler.INSTANCE.IsValidPosition(testTilePosition)) {
                    continue;
                }

                //No unit within range.
                if(!GridSystemHandler.INSTANCE.IsOccupiedByUnit(testTilePosition)) {
                    continue;
                }

                UnitHandler targetUnit = GridSystemHandler.INSTANCE.GetAUnitAtTilePosition(testTilePosition);

                //Ignore units of the same team.
                if(targetUnit.IsEnemy() == unit.IsEnemy()) {
                    continue;
                }

                if((Mathf.Abs(x + z) == attackRange) && (Mathf.Abs(x - z) == attackRange)) {
                    validAttackPositionsList.Add(testTilePosition);
                }

                //Debug.Log(testTilePosition);

            }
        }

        return validAttackPositionsList;

    }

    [PunRPC]
    private void Attack() {
        //Identify unit's attack parameter and pass it on as the base damage value.
        attackParameter = unit.GetParamATK();
        targetedUnit.TakeDamage(attackParameter);
    }

}
