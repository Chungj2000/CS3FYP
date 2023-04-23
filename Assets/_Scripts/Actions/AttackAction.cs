using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/*
 * Action Script for handling combat initiation.
 * Validates for targets within range of Unit attack range.
 * Subsequently attacks towards the direction of the targetted unit of the selected GridTile via TilePosition.
 */
public class AttackAction : AbstractAction {

    [SerializeField] private float rotateSpeed = 15f;

    private int attackRange;
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

            //Execute the attack.
            PerformAttack();

        }

    }

    //Make the Unit face the targetted unit on both clients.
    [PunRPC]
    private void RPC_FaceTarget() {

        //Get the normalized value for facing the targetted unit.
        Vector3 faceTowards = (targetedUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;

        //Face the unit towards target.
        transform.forward = Vector3.Lerp(transform.forward, faceTowards, Time.deltaTime * rotateSpeed);
        //Debug.Log("Facing towards target - " + targetedUnit.GetWorldPosition() + "," + unit.GetWorldPosition());
    }

    private void PerformAttack() {
        if(Vector3.Angle(transform.forward, targetedUnit.GetWorldPosition() - unit.GetWorldPosition()) <= stoppingDistance) {
            onActionComplete();
            //Debug.Log("Unit now facing target.");

            view.RPC(nameof(RPC_Attack), RpcTarget.AllBuffered, null);

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
        view.RPC(nameof(RPC_SetTarget), RpcTarget.AllBuffered, position.x, position.z);

        //Debug.Log("Attacking: " + targetedUnit);

        //Play the attack animation.
        //unit.GetUnitAnimator().PlayAttack();
    }

    //Ensure both clients know who the targetted unit is for the attack.
    [PunRPC]
    private void RPC_SetTarget(int x, int z) {
        //RPCs cannot take custom types so recreate targetted TilePosition using primitives.
        TilePosition position = new TilePosition(x, z);
        targetedUnit = GridSystemHandler.INSTANCE.GetAUnitAtTilePosition(position);
    }

    //Create a list of positions the unit can attack.
    public override List<TilePosition> ListValidActionPositions() {

        List<TilePosition> validAttackPositionsList = new List<TilePosition>();

        TilePosition unitTilePosition = unit.getTilePosition();

        //Loop through all possible x & z values within a given range.
        for(int x = -attackRange; x <= attackRange; x++) {
            for(int z = -attackRange; z <= attackRange; z++) {

                //Create a tile position using the looped indexes from the range.
                TilePosition offsetTilePosition = new TilePosition(x, z);
                //Using the unit position as the center, create a valid til eposition within the assigned range.
                TilePosition testTilePosition = unitTilePosition + offsetTilePosition;

                //Validate positions. Invalid tile positions are passed.
                if(!GridSystemHandler.INSTANCE.IsValidPosition(testTilePosition)) {
                    //Continue with the for loop.
                    continue;
                }

                //No unit within range.
                if(!GridSystemHandler.INSTANCE.IsOccupiedByUnit(testTilePosition)) {
                    continue;
                }

                //Get the unit within the current tile position.
                UnitHandler targetUnit = GridSystemHandler.INSTANCE.GetAUnitAtTilePosition(testTilePosition);

                //Ignore units of the same team.
                if(targetUnit.IsOwnedByPlayer1() == unit.IsOwnedByPlayer1()) {
                    continue;
                }

                //Note: Comment No Units in range, and ignore allied units for testing attack range.

                ValidateRangeOfAttack(x, z, validAttackPositionsList, testTilePosition, targetUnit);

                //Debug.Log(testTilePosition);

            }
        }

        return validAttackPositionsList;

    }

    //Determine what type of attack range the unit should have based on RangeType of the Unit.
    private void ValidateRangeOfAttack(int x, int z, List<TilePosition> validAttackPositionsList, TilePosition testTilePosition, UnitHandler targetUnit) {

        //Debug.Log("Unit is Type: " + unit.GetRangeType());

        if(unit.GetRangeType() == CombatDataType.RangeType.Precise) {

            //Debug.Log("Initiating a Precise attack.");

            //Attack a target within the perimeter of the ATK_RANGE. For melee, and circle ranged units.

            if((Mathf.Abs(x + z) == attackRange) || (Mathf.Abs(x - z) == attackRange)) {
                //If all the conditions are met, add the current tile position as a valid position.
                validAttackPositionsList.Add(testTilePosition);
            }

        } else if (unit.GetRangeType() == CombatDataType.RangeType.Area) {

            //Debug.Log("Initiating an Area attack.");
            
            //Attack a target within an area between 2 (out of melee) and the ATK_RANGE of the unit.

            if(((Mathf.Abs(x + z) <= attackRange) && 
                (Mathf.Abs(x - z) <= attackRange) &&
                (Mathf.Abs(x + z) > 1 || Mathf.Abs(x - z) > 1))) {
                //If all the conditions are met, add the current tile position as a valid position.
                validAttackPositionsList.Add(testTilePosition);
            }

        }
    }

    //Ensure both clients are updated for the targetted Unit taking damage.
    [PunRPC]
    private void RPC_Attack() {
        //Set the attacker and defender for damage calculations.
        CombatDataHandler.INSTANCE.SetAttackingUnit(unit);
        CombatDataHandler.INSTANCE.SetDefendingUnit(targetedUnit);
        targetedUnit.TakeDamage();
    }

}
