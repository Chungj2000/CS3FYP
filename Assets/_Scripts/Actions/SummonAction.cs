using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/*
 * Action Script used for summoning Units from the Fortress. Only available for Building type Units currently.
 */
public class SummonAction : AbstractAction {

    private int summonRange;
    private Action onAttackComplete; // = 10;

    private string summoningUnit;
    private GameObject summoningUnitPrefab;
    private GameObject summonedUnit;
    private Vector3 summonPosition;
    private Quaternion summonedRotationAngle;
    private int summonCost, playerGold;

    private int unitCount;

    private bool readyToSummon;
    private bool isActive;

    protected override void Awake() {
        base.Awake();
        summonRange = unit.GetParamATK_RANGE();
        isActive = false;
        readyToSummon = false;
    }

    private void Update() {

        //If action is not active, do nothing.
        if(!isActive) {
            //Debug.Log("Move Action active: " + isActive);
            return;
        }

        //Check whether Summon has been called via PrepareAction.
        if(readyToSummon) {

            //Debug.Log("About to summon.");

            readyToSummon = false;

            InitialiseSummon();

            //Check if the Player has enough gold to summon the unit.
            if(playerGold - summonCost >= 0) {

                Debug.Log("Player has enough gold to summon " + summoningUnit + ".");

                GoldManager.INSTANCE.SpendGoldForPlayer(PlayerHandler.INSTANCE.IsPlayer1(), summonCost);

                //Create the Unit for both clients, other client will be missing the reference.
                summonedUnit = PhotonNetwork.Instantiate(summoningUnit, summonPosition, summonedRotationAngle, 0);
                //Debug.Log("Unit summoned.");
                int summonedUnitId = summonedUnit.GetComponent<PhotonView>().ViewID;
                //ViewID needed for referencing.

                //Update summon for both clients.
                view.RPC(nameof(RPC_SummonAction), RpcTarget.AllBuffered, summonedUnitId, PlayerHandler.INSTANCE.IsPlayer1());

            } else {
                Debug.Log("Insufficient gold to summon " + summoningUnit + ".");
                onActionComplete();
            }

        } 

    }

    //Declare values needed to summon the selected Unit.
    private void InitialiseSummon() {

        Vector3 rotationVector;

        //Initialise values.
        if(PlayerHandler.INSTANCE.IsPlayer1()) {
            unitCount = UnitManager.INSTANCE.CountPlayer1Units();
            rotationVector = new Vector3(0,0,0);
            playerGold = GoldManager.INSTANCE.GetPlayer1TotalGold();
        } else {
            unitCount = UnitManager.INSTANCE.CountPlayer2Units();
            rotationVector = new Vector3(0,180,0);
            playerGold = GoldManager.INSTANCE.GetPlayer2TotalGold();
        }

        summonedRotationAngle = Quaternion.Euler(rotationVector.x, rotationVector.y, rotationVector.z);

        //How much the selected Unit will cost.
        summonCost = summoningUnitPrefab.GetComponent<UnitHandler>().GetParamGOLD_COST();

    }

    [PunRPC]
    private void RPC_SummonAction(int id, bool isPlayer1) {

        int updatedUnitCount;

        //Create Unit reference for both clients.
        summonedUnit = PhotonView.Find(id).gameObject;

        UnitManager.INSTANCE.AddUnitToList(summonedUnit);

        //Check value to validate whether Unit has been successfully added.
        if(isPlayer1) {
            updatedUnitCount = UnitManager.INSTANCE.CountPlayer1Units();
        } else {
            updatedUnitCount = UnitManager.INSTANCE.CountPlayer2Units();
        }

        //Ensure that the new Unit has been added to the UnitManager tracker so win conditions are accurate.
        if(unitCount < updatedUnitCount) {
            onActionComplete();

            //Prevent newly created unit from acting.
            summonedUnit.GetComponent<UnitHandler>().SetAttackActionUsed();

            isActive = false;
        }
    }

    public override void PrepareAction(TilePosition position, Action onSummonComplete) {

        //Debug.Log("Preparing action.");

        //Turn on action Updates.
        isActive = true;
        //Debug.Log("Summon Action active: " + isActive);

        //Determine what unit is being summoned via selected item in UnitShopUI.
        summoningUnit = UnitShopUI.INSTANCE.GetSelectedUnitType();
        summoningUnitPrefab = UnitShopUI.INSTANCE.GetSelectedUnitPrefab();

        summonPosition = GridSystemHandler.INSTANCE.GetWorldPosition(position);

        this.onActionComplete = onSummonComplete;

        readyToSummon = true;
    }

    //Create a list of positions the unit can attack.
    public override List<TilePosition> ListValidActionPositions() {

        List<TilePosition> validSummonPositionsList = new List<TilePosition>();

        TilePosition unitTilePosition = unit.getTilePosition();

        //Loop through all possible x & z values within a given range.
        for(int x = -summonRange; x <= summonRange; x++) {
            for(int z = -summonRange; z <= summonRange; z++) {

                //Create a tile position using the looped indexes from the range.
                TilePosition offsetTilePosition = new TilePosition(x, z);
                //Using the unit position as the center, create a valid til eposition within the assigned range.
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
                
                //Validate positions that a unit can be summoned in within ATK_RANGE.
                if((Mathf.Abs(x + z) == summonRange) || (Mathf.Abs(x - z) == summonRange)) {
                    //If all the conditions are met, add the current tile position as a valid position.
                    validSummonPositionsList.Add(testTilePosition);
                }

                //Debug.Log(testTilePosition);

            }
        }

        return validSummonPositionsList;

    }

}

