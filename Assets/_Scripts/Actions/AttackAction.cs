using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : AbstractAction {
    
    private int attackRange;
    private Action onAttackComplete; // = 10;

    protected override void Awake() {
        base.Awake();
        attackRange = unit.GetParamATK_RANGE();
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

                validAttackPositionsList.Add(testTilePosition);

                //Debug.Log(testTilePosition);

            }
        }

        return validAttackPositionsList;

    }

    public override void PrepareAction(TilePosition position, Action onMoveComplete) {
        this.onActionComplete = onMoveComplete;
        //Debug.Log("Moving unit to new position.");
    }

}
