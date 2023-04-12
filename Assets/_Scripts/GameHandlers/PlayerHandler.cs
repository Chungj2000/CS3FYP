using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour {

    public static PlayerHandler INSTANCE {get; private set;}
    
    private bool isPlayer1;

    private void  Awake() {
        if(INSTANCE == null) {
            INSTANCE = this;
            //Debug.Log("PlayerHandler instance created.");
        } else {
            Debug.LogError("More than one PlayerHandler instance created.");
            Destroy(gameObject);
            return;
        }
    }

    public void SetAsPlayer1() {
        isPlayer1 = true;
    }

    public void SetAsPlayer2() {
        isPlayer1 = false;
    }

    public bool IsPlayer1() {
        return isPlayer1;
    }

}
