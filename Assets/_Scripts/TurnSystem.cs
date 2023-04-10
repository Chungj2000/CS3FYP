using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class TurnSystem : MonoBehaviour {
    
    public static TurnSystem INSTANCE {get; private set;}

    public event EventHandler OnEndTurn;
    private int turnTracker = 1;
    private bool isPlayer1Turn = true;
    private PhotonView view;

    private void  Awake() {

        view = GetComponent<PhotonView>();

        if(INSTANCE == null) {
            INSTANCE = this;
            //Debug.Log("TurnSystem instance created.");
        } else {
            Debug.Log("More than one TurnSystem instance created.");
            Destroy(gameObject);
            return;
        }
    }

    //Increment turn count and activate an event.
    public void EndTurnClicked() {
        view.RPC(nameof(NextTurn), RpcTarget.AllBuffered, null);
    }

    [PunRPC]
    public void NextTurn() {
        turnTracker++;
        //Debug.Log("Current turn: " + turnTracker);

        isPlayer1Turn = !isPlayer1Turn;
        //Debug.Log("Currently is the turn of Player 1: " + isPlayer1Turn);
        
        OnEndTurn?.Invoke(this, EventArgs.Empty);
    }

    public int GetTurnTracker() {
        return turnTracker;
    }

    public bool IsPlayer1Turn() {
        return isPlayer1Turn;
    }

}
