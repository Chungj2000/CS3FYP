using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : AbstractAction {

    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float rotateSpeed = 30f;

    private int maxMoveDistance;
    private Vector3 moveToPosition;
    private bool isActive;

    protected override void Awake() {
        base.Awake();
        moveToPosition = transform.position;
        maxMoveDistance = unit.GetParamMOVE();
        isActive = false;
    }

    private void Update() {

        //If action is not active, do nothing.
        if(!isActive) {
            //Debug.Log("Move Action active: " + isActive);
            return;
        }

        //Stops the unit before the point.
        float stoppingDistance = .01f;

        //Move the unit until the distance between the end point and unit is less than the stopping distance.
        if(Vector3.Distance(transform.position, moveToPosition) > stoppingDistance) {
            //Get the normalized value for moving towards destination.
            Vector3 moveDirection = (moveToPosition - transform.position).normalized;

            //Smoothly rotate the unit to moving direction.
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);

            //Move the unit.
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

            //Once unit reaches the  target destination, UnitActionSystem is no longer busy.
            if(Vector3.Distance(transform.position, moveToPosition) <= stoppingDistance) {
                onActionComplete();
                //Debug.Log("Unit has reached their destination.");

                //Turn off move updates, until action is activated again.
                isActive = false;
            }
        } 

    }

    public override void PrepareAction(TilePosition position, Action onMoveComplete) {
        //Turn on action Updates.
        isActive = true;
        //Debug.Log("Move Action active: " + isActive);

        this.onActionComplete = onMoveComplete;

        //Debug.Log("Moving unit to new position.");
        this.moveToPosition = GridSystemHandler.INSTANCE.GetWorldPosition(position);
    }

    //Create a list of positions the unit can move to.
    public override List<TilePosition> ListValidActionPositions() {

        List<TilePosition> validMovePositionsList = new List<TilePosition>();

        TilePosition unitTilePosition = unit.getTilePosition();

        for(int x = -maxMoveDistance; x <= maxMoveDistance; x++) {
            for(int z = -maxMoveDistance; z <= maxMoveDistance; z++) {

                TilePosition offsetTilePosition = new TilePosition(x, z);
                TilePosition testTilePosition = unitTilePosition + offsetTilePosition;

                //Validate positions. Invalid tile positions are passed.
                if(!GridSystemHandler.INSTANCE.IsValidPosition(testTilePosition)) {
                    continue;
                }

                //Skip the current unit position.
                if(unitTilePosition == testTilePosition) {
                    continue;
                }

                //Skip tiles occupied with a unit;
                if(GridSystemHandler.INSTANCE.IsOccupiedByUnit(testTilePosition)) {
                    continue;
                }
                
                //Validate positions within a range of max move distance cost for diagonal movement.
                if((Mathf.Abs(x + z) <= maxMoveDistance) && (Mathf.Abs(x - z) <= maxMoveDistance)) {
                    validMovePositionsList.Add(testTilePosition);
                }

                //Debug.Log(testTilePosition);

            }
        }

        return validMovePositionsList;

    }

}
