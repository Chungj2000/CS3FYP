using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GoldManager : MonoBehaviour {

    public static GoldManager INSTANCE {get; private set;}

    private PhotonView view;
    
    private const int defaultGoldIncome = 25;
    private int player1TotalGold = 0;
    private int player2TotalGold = 0;

    private void  Awake() {

        view = GetComponent<PhotonView>();

        if(INSTANCE == null) {
            INSTANCE = this;
            //Debug.Log("GoldManager instance created.");
        } else {
            Debug.Log("More than one GoldManager instance created.");
            Destroy(this);
            return;
        }
    }

    public void GenerateGoldForTurn() {

        //Check whether it's currently the turn of Player 1 or 2.
        if(PlayerHandler.INSTANCE.IsPlayer1()) {
            Debug.Log("Generating Gold for Player 2.");
            GenerateGoldPlayer2(defaultGoldIncome);
        } else {
            Debug.Log("Generating Gold for Player 1.");
            GenerateGoldPlayer1(defaultGoldIncome);
        }

        UpdateUnitUI();

    }

    //Increase the gold of the current Player's turn by a given amount.
    private void GenerateGoldPlayer1(int goldAmount) {
        player1TotalGold += goldAmount;
        Debug.Log("Player gold = " + player1TotalGold);
        view.RPC(nameof(SetGoldPlayer1), RpcTarget.AllBuffered, player1TotalGold);
    }

    private void GenerateGoldPlayer2(int goldAmount) {
        player2TotalGold += goldAmount;
        Debug.Log("Player gold = " + player2TotalGold);
        view.RPC(nameof(SetGoldPlayer2), RpcTarget.AllBuffered, player2TotalGold);
        
    }

    //Reduce total gold by a given amount for the current Player.
    public void SpendGoldForPlayer(bool isPlayer1, int goldSpent) {

        //Check whether transaction is for Player 1 or 2.
        if(isPlayer1) {
            Debug.Log("Deducting Gold for Player 1.");
            SpendGoldPlayer1(goldSpent);
        } else {
            Debug.Log("Deducting Gold for Player 2.");
            SpendGoldPlayer2(goldSpent);
        }

    }

    private void SpendGoldPlayer1(int goldAmount) {
        player1TotalGold -= goldAmount;
        Debug.Log("Player gold = " + player1TotalGold);
        view.RPC(nameof(SetGoldPlayer1), RpcTarget.AllBuffered, player1TotalGold);
    }

    private void SpendGoldPlayer2(int goldAmount) {
        player2TotalGold -= goldAmount;
        Debug.Log("Player gold = " + player2TotalGold);
        view.RPC(nameof(SetGoldPlayer2), RpcTarget.AllBuffered, player2TotalGold);
    }

    private void UpdateUnitUI() {
        PlayerUnitUI.INSTANCE.UpdateTotalGoldField();
        EnemyUnitUI.INSTANCE.UpdateTotalGoldField();
    }

    //Getters.
    public int GetPlayer1TotalGold() {
        return player1TotalGold;
    }

    public int GetPlayer2TotalGold() {
        return player2TotalGold;
    }

    [PunRPC]
    private void SetGoldPlayer1(int amount) {
        player1TotalGold = amount;
        Debug.Log("Player 1 now has: " + player1TotalGold);
        UpdateUnitUI();
    }

    [PunRPC]
    private void SetGoldPlayer2(int amount) {
        player2TotalGold = amount;
        Debug.Log("Player 2 now has: " + player2TotalGold);
        UpdateUnitUI();
    }

}
