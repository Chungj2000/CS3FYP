using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    
    public static TurnSystem INSTANCE {get; private set;}
    public event EventHandler OnEndTurn;
    private int turnTracker = 1;

    private void  Awake() {
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
    public void NextTurn() {
        turnTracker++;
        //Debug.Log("Current turn: " + turnTracker);
        OnEndTurn?.Invoke(this, EventArgs.Empty);
    }

    public int GetTurnTracker() {
        return turnTracker;
    }

}
