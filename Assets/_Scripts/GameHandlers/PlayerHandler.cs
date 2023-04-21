using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *A Script that keeps track of which Player the client is that can be accessed at any given point.
 */
public class PlayerHandler : MonoBehaviour {

    public static PlayerHandler INSTANCE {get; private set;}
    
    private bool isPlayer1;

    private void  Awake() {
        if(INSTANCE == null) {
            INSTANCE = this;
            //Debug.Log("PlayerHandler instance created.");
        } else {
            Debug.LogError("More than one PlayerHandler instance created.");
            Destroy(this);
            return;
        }
    }

    //Setters and getters.
    public void SetAsPlayer1() {
        isPlayer1 = true;
        Debug.Log("You've been set to Player 1.");
    }

    public void SetAsPlayer2() {
        isPlayer1 = false;
        Debug.Log("You've been set to Player 2.");
    }

    public bool IsPlayer1() {
        return isPlayer1;
    }

}
