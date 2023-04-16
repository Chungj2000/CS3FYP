using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldManager : MonoBehaviour {

    public static GoldManager INSTANCE {get; private set;}
    
    private const int defaultGoldIncome = 50;
    private int player1TotalGold = 0;
    private int player2TotalGold = 0;

    private void  Awake() {
        if(INSTANCE == null) {
            INSTANCE = this;
            //Debug.Log("GoldManager instance created.");
        } else {
            Debug.Log("More than one GoldManager instance created.");
            Destroy(gameObject);
            return;
        }
    }

    public void GenerateGoldForTurn(bool isPlayer1Turn) {

        //Check whether it's currently the turn of Player 1 or 2.
        if(isPlayer1Turn == PlayerHandler.INSTANCE.IsPlayer1()) {
            Debug.Log("Generating Gold for Player 1.");
            GenerateGoldPlayer1(defaultGoldIncome);
        } else {
            Debug.Log("Generating Gold for Player 2.");
            GenerateGoldPlayer2(defaultGoldIncome);
        }

        UpdateUnitUI();

    }

    //Increase the gold of the current Player's turn by a given amount.
    private void GenerateGoldPlayer1(int goldAmount) {
        player1TotalGold += goldAmount;
        Debug.Log("Player gold = " + player1TotalGold);
    }

    private void GenerateGoldPlayer2(int goldAmount) {
        player2TotalGold += goldAmount;
        Debug.Log("Player gold = " + player2TotalGold);
    }

    //Reduce total gold by a given amount for the current Player.
    public void SpendGoldForPlayer(bool isPlayer1Turn, int goldSpent) {

        //Check whether transaction is for Player 1 or 2.
        if(isPlayer1Turn == PlayerHandler.INSTANCE.IsPlayer1()) {
            Debug.Log("Deducting Gold for Player 1.");
            SpendGoldPlayer1(goldSpent);
        } else {
            Debug.Log("Deducting Gold for Player 2.");
            SpendGoldPlayer2(goldSpent);
        }

        UpdateUnitUI();

    }

    private void SpendGoldPlayer1(int goldAmount) {
        player1TotalGold -= goldAmount;
        Debug.Log("Player gold = " + player1TotalGold);
    }

    private void SpendGoldPlayer2(int goldAmount) {
        player2TotalGold -= goldAmount;
        Debug.Log("Player gold = " + player2TotalGold);
    }

    private void UpdateUnitUI() {
        PlayerUnitUI.INSTANCE.UpdateTotalGoldField();
        EnemyUnitUI.INSTANCE.UpdateTotalGoldField();
    }

    //Getters.
    public int GetTotalGold() {
        //Identify whether client is Player 1 or 2.
        if(PlayerHandler.INSTANCE.IsPlayer1()) {
            return GetPlayer1TotalGold();
        } else {
            return GetPlayer2TotalGold();
        }
    }

    private int GetPlayer1TotalGold() {
        return player1TotalGold;
    }

    private int GetPlayer2TotalGold() {
        return player2TotalGold;
    }

}
