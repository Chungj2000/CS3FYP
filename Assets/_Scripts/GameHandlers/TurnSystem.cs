using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class TurnSystem : MonoBehaviour {
    
    public static TurnSystem INSTANCE {get; private set;}

    public event EventHandler OnEndTurn;
    private int turnTracker = 1;
    private bool isPlayer1Turn = true;
    private const float maxTime = 120;
    private float timeLeft;
    private float seconds;
    private bool timerOn = false;
    private PhotonView view;

    private void  Awake() {

        view = GetComponent<PhotonView>();
        ResetTimer();

        if(INSTANCE == null) {
            INSTANCE = this;
            //Debug.Log("TurnSystem instance created.");
        } else {
            Debug.Log("More than one TurnSystem instance created.");
            Destroy(this);
            return;
        }
    }

    private void Update() {
        UpdateTime();
        TimerElapsed();
    }

    //Increment turn count and activate an event.
    public void EndTurnClicked() {
        view.RPC(nameof(NextTurn), RpcTarget.AllBuffered, isPlayer1Turn);
        PlayerUnitUI.INSTANCE.UpdateTotalGoldField();
        EnemyUnitUI.INSTANCE.UpdateTotalGoldField();
    }

    [PunRPC]
    private void NextTurn(bool playerTurn) {
        turnTracker++;
        //Debug.Log("Current turn: " + turnTracker);

        isPlayer1Turn = !playerTurn;
        Debug.Log("Currently is the turn of Player 1: " + isPlayer1Turn);

        //Generate Gold for the Player whose turn is next.
        GoldManager.INSTANCE.GenerateGoldForTurn();

        //Reset the timer for the other Player's turn.
        ResetTimer();
        
        OnEndTurn?.Invoke(this, EventArgs.Empty);
    }

    public int GetTurnTracker() {
        return turnTracker;
    }

    public bool IsPlayer1Turn() {
        return isPlayer1Turn;
    }

    //Reduce the timer by 1 every second.
    private void UpdateTime() {
        if(timerOn) {
            timeLeft -= Time.deltaTime;
            seconds = Mathf.FloorToInt(timeLeft % 120);
        }
    }

    //Check when timer reaches 0, in which case change turn.
    private void TimerElapsed() {
        if(timeLeft <= 0) {
            //Debug.Log("Timer has elapsed.");
            EndTurnClicked();
        }
    }

    private void ResetTimer() {
        timeLeft = maxTime;
        //Debug.Log("Timer has been reset.");
    }

    public void TurnOnTimer() {
        timerOn = true;
        //Debug.Log("Timer is now active.");
    }

    public void TurnOffTimer() {
        timerOn = false;
        //Debug.Log("Timer is now deactivated.");
    }

    public void UpdateTimerText(TextMeshProUGUI timer) {
        timer.text = seconds.ToString();
    }

}
