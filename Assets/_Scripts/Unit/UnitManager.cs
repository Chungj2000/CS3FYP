using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This class deals with all units on the field, whether it is instantiating or counting the units present for win condition checking.
 */

public class UnitManager : MonoBehaviour {

    public static UnitManager INSTANCE {get; private set;}

    [Header("Unit Prefabs")]
    [SerializeField] private GameObject militiaPrefab;
    [SerializeField] private GameObject archerPrefab;
    [SerializeField] private GameObject knightPrefab;
    [SerializeField] private GameObject lightCavalryPrefab;
    [SerializeField] private GameObject heavyCavalryPrefab;
    [SerializeField] private GameObject catapultPrefab;

    //Unit Pool [Player 1]
    private const string militiaPrefab_Player1 = "Militia_Player1";
    private const string archerPrefab_Player1 = "Archer_Player1";
    private const string knightPrefab_Player1 = "Knight_Player1";
    private const string lightCavalryPrefab_Player1 = "LightCavalry_Player1";
    private const string heavyCavalryPrefab_Player1 = "HeavyCavalry_Player1";
    private const string catapultPrefab_Player1 = "Catapult_Player1";

    //Unit Pool [Player 2]
    private const string militiaPrefab_Player2 = "Militia_Player2";
    private const string archerPrefab_Player2 = "Archer_Player2";
    private const string knightPrefab_Player2 = "Knight_Player2";
    private const string lightCavalryPrefab_Player2 = "LightCavalry_Player2";
    private const string heavyCavalryPrefab_Player2 = "HeavyCavalry_Player2";
    private const string catapultPrefab_Player2 = "Catapult_Player2";

    private List<GameObject> player1Units;
    private List<GameObject> player2Units;

    private void  Awake() {
        if(INSTANCE == null) {
            INSTANCE = this;
            //Debug.Log("UnitManager instance created.");
        } else {
            Debug.Log("More than one UnitManager instance created.");
            Destroy(this);
            return;
        }
    }

    private void Start() {
        player1Units = new List<GameObject>();
        player2Units = new List<GameObject>();

        ListActiveUnits();
        //CountPlayer1Units();
        //CountPlayer2Units();
    }

    //List out all units present on the field.
    private void ListActiveUnits() {
        //Were identifying units by Tag therefore only one set of prefabs is necessary here.
        FindUnits(militiaPrefab.tag);
        FindUnits(archerPrefab.tag);
        FindUnits(knightPrefab.tag);
        FindUnits(lightCavalryPrefab.tag);
        FindUnits(heavyCavalryPrefab.tag);
        FindUnits(catapultPrefab.tag);

        Debug.Log("All units have been added to a respective list.");
    }

    //Find units of a certain type and add it to the list of Player owned units that are active on the field.
    private void FindUnits(string tag) {

        GameObject[] foundUnits = GameObject.FindGameObjectsWithTag(tag);
        Debug.Log(tag + " found: " + foundUnits.Length);

        //Determine which Player owns the unit.
        foreach(GameObject unit in foundUnits) {
            if(unit.GetComponent<UnitHandler>().IsOwnedByPlayer1()) {
                player1Units.Add(unit);
                //Debug.Log("A unit owned by Player 1 found: " + unit);
            } else {
                player2Units.Add(unit);
                //Debug.Log("A unit owned by Player 2 found: " + unit);
            }
        }

        //Debug.Log("All units of type" + tag + " have been found.");

    }

    //Identify which team unit belongs to before removing the unit.
    public void RemoveUnitFromList(GameObject unit) {
        if(unit.GetComponent<UnitHandler>().IsOwnedByPlayer1()) {
            player1Units.Remove(unit);
            Debug.Log("Unit of Player 1 was removed: " + unit);
            CheckNoMorePlayer1Units();
            CheckFortressDeath(unit);
        } else {
            player2Units.Remove(unit);
            Debug.Log("Unit of Player 2 was removed: " + unit);
            CheckNoMorePlayer2Units();
            CheckFortressDeath(unit);
        }
    }

    public void AddUnitToList(GameObject unit) {
        if(unit.GetComponent<UnitHandler>().IsOwnedByPlayer1()) {
            player1Units.Add(unit);
            //Debug.Log("Unit of Player 1 was added: " + unit);
        } else {
            player2Units.Add(unit);
            //Debug.Log("Unit of Player 2 was added: " + unit);
        }
    }

    //Win condition test to see if Player still possess units.
    private void CheckNoMorePlayer1Units() {

        Debug.Log("Checking if Player 1 has more than 0 units.");

        if(CountPlayer1Units() == 0) {

            Debug.Log("Player 1 has no more units.");

            //Identify which Player the client is and determine a winner.
            if(PlayerHandler.INSTANCE.IsPlayer1()) {
                GameOverHandler.INSTANCE.DeclareGameOver(false);
            } else {
                GameOverHandler.INSTANCE.DeclareGameOver(true);
            }

        }

    }

    private void CheckNoMorePlayer2Units() {

        Debug.Log("Checking if Player 2 has more than 0 units.");

        if(CountPlayer2Units() == 0) {

            Debug.Log("Player 2 has no more units.");

            //Identify which Player the client is and determine a winner.
            if(PlayerHandler.INSTANCE.IsPlayer1()) {
                GameOverHandler.INSTANCE.DeclareGameOver(true);
            } else {
                GameOverHandler.INSTANCE.DeclareGameOver(false);
            }

        }

    }

    //Alternative win condition for destroying enemy fortress.
    private void CheckFortressDeath(GameObject unit) {

        //Check if it's a Fortress.
        if(unit.tag == "Fortress") {

            //Identify the owner of the Fortress.
            if(unit.GetComponent<UnitHandler>().IsOwnedByPlayer1() == PlayerHandler.INSTANCE.IsPlayer1()) {
                GameOverHandler.INSTANCE.DeclareGameOver(false);
            } else {
                GameOverHandler.INSTANCE.DeclareGameOver(true);
            }

        }

    }

    public int CountPlayer1Units() {
        Debug.Log("Player 1 owns this many units: " + player1Units.Count);
        return player1Units.Count;
    }

    public int CountPlayer2Units() {
        Debug.Log("Player 2 owns this many units: " + player2Units.Count);
        return player2Units.Count;
    }

    //Getters for Unit Prefabs.
    public GameObject GetMilitiaPrefab() {
        return militiaPrefab;
    }

    public GameObject GetArcherPrefab() {
        return archerPrefab;
    }

    public GameObject GetLightCavalryPrefab() {
        return lightCavalryPrefab;
    }

    public GameObject GetKnightPrefab() {
        return knightPrefab;
    }

    public GameObject GetCatapultPrefab() {
        return catapultPrefab;
    }

    public GameObject GetHeavyCavalryPrefab() {
        return heavyCavalryPrefab;
    }

    //Getters for Unit Types.
    public string GetMilitiaPlayer1() {
        return militiaPrefab_Player1;
    }

    public string GetMilitiaPlayer2() {
        return militiaPrefab_Player2;
    }

    public string GetArcherPlayer1() {
        return archerPrefab_Player1;
    }

    public string GetArcherPlayer2() {
        return archerPrefab_Player2;
    }

    public string GetKnightPlayer1() {
        return knightPrefab_Player1;
    }

    public string GetKnightPlayer2() {
        return knightPrefab_Player2;
    }

    public string GetLightCavalryPlayer1() {
        return lightCavalryPrefab_Player1;
    }

    public string GetLightCavalryPlayer2() {
        return lightCavalryPrefab_Player2;
    }

    public string GetHeavyCavalryPlayer1() {
        return heavyCavalryPrefab_Player1;
    }

    public string GetHeavyCavalryPlayer2() {
        return heavyCavalryPrefab_Player2;
    }

    public string GetCatapultPlayer1() {
        return catapultPrefab_Player1;
    }

    public string GetCatapultPlayer2() {
        return catapultPrefab_Player2;
    }

}
