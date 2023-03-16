using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : MonoBehaviour
{

    private Vector3 moveToPosition;
    private UnitHandler unit;
    private Action onMoveComplete;
    [SerializeField] float moveSpeed = 4f;
    [SerializeField] float rotateSpeed = 30f;

    [SerializeField] int maxMoveDistance = 3;

    private void Awake() {
        moveToPosition = transform.position;
        unit = GetComponent<UnitHandler>();
    }

    private void Update() {

        //Stops the unit before the point.
        float stoppingDistance = .01f;

        //Move the unit until the distance between the end point and unit is less than the stopping distance.
        if(Vector3.Distance(transform.position, moveToPosition) > stoppingDistance) {
            Vector3 moveDirection = (moveToPosition - transform.position).normalized;

            //Move the unit.
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

            //Smoothly rotate the unit to moving direction.
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);

            //Once unit reaches the  target destination, UnitActionSystem is no longer busy.
            if(Vector3.Distance(transform.position, moveToPosition) <= stoppingDistance) {
                onMoveComplete();
            }
        } 

    }

    public void Move(TilePosition position, Action onMoveComplete) {
        this.onMoveComplete = onMoveComplete;
        this.moveToPosition = GridSystemHandler.INSTANCE.GetWorldPosition(position);
    }

    public List<TilePosition> ListValidMovePositions() {

        List<TilePosition> validMovePositionsList = new List<TilePosition>();

        TilePosition unitTilePosition = unit.getTilePosition();

        for(int x = -maxMoveDistance; x <= maxMoveDistance; x++) {
            for(int z = -maxMoveDistance; z <= maxMoveDistance; z++) {

                TilePosition offsetTilePosition = new TilePosition(x, z);
                TilePosition testTilePosition = unitTilePosition + offsetTilePosition;

                //Validate positions. Invalid tile positions are passed.
                if(!GridSystemHandler.INSTANCE.IsValidPosition(testTilePosition)) {
                    //Skip invalid tiles.
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
                
                validMovePositionsList.Add(testTilePosition);
                //Debug.Log(testTilePosition);

            }
        }

        return validMovePositionsList;

    }

    public bool IsValidMoveAction(TilePosition position) {
        List<TilePosition> validMoveActionList = ListValidMovePositions();
        return validMoveActionList.Contains(position);
    }

}
