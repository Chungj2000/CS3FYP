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

    private void Start() {

        //Enable a listener for button interaction for End Turn.
        endTurnBTN.onClick.AddListener(() => {
            //Debug.Log("End Turn Button clicked.");
            TurnSystem.INSTANCE.NextTurn();
        });

        TurnSystem.INSTANCE.OnEndTurn += TurnSystem_OnEndTurn;

        //Instantiate turn counter on the start of the game.
        UpdateTurnCounter();
    }

    //Event for Updating Turn Counter upon button click execution.
    private void TurnSystem_OnEndTurn(object sender, EventArgs e) {
        //Debug.Log("Turn has ended.");
        UpdateTurnCounter();
    }

    private void UpdateTurnCounter() {
        //Update turn counter text when a turn is ended.
        turnCounterText.text = "TURN " + TurnSystem.INSTANCE.GetTurnTracker(); 
    }

}
