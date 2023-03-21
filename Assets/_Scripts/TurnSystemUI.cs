using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnSystemUI : MonoBehaviour
{
    
    [SerializeField] private Button endTurnBTN;
    [SerializeField] private TextMeshProUGUI turnCounterText;
    private const String player1TurnText = "[PLAYER 1]";
    private const String player2TurnText = "[PLAYER 2]";

    private void Start() {

        //Enable a listener for button interaction for End Turn.
        endTurnBTN.onClick.AddListener(() => {
            //Debug.Log("End Turn Button clicked.");
            TurnSystem.INSTANCE.NextTurn();
        });

        TurnSystem.INSTANCE.OnEndTurn += TurnSystem_OnEndTurn;

        //Instantiate turn counter on the start of the game.
        UpdateTurnCounter();
        //UpdateEndTurnButtonVisibility();
    }

    //Event for Updating Turn Counter upon button click execution.
    private void TurnSystem_OnEndTurn(object sender, EventArgs e) {
        //Debug.Log("A turn has ended.");
        UpdateTurnCounter();
        //UpdateEndTurnButtonVisibility();
    }

    private void UpdateTurnCounter() {

        //Update turn counter text when a turn is ended for each player.

        if(TurnSystem.INSTANCE.IsPlayer1Turn()) {
            //Player 1 turns needs a +1 increment for correct calculations on turn number.
            turnCounterText.text = player1TurnText + " TURN " + ((TurnSystem.INSTANCE.GetTurnTracker()/2) + 1);
        } else {
            turnCounterText.text = player2TurnText + " TURN " + (TurnSystem.INSTANCE.GetTurnTracker()/2);
        }
         
    }
    
    //Hide the end turn button when not player's turn.
    private void UpdateEndTurnButtonVisibility() {
        endTurnBTN.gameObject.SetActive(TurnSystem.INSTANCE.IsPlayer1Turn());
    }

}
