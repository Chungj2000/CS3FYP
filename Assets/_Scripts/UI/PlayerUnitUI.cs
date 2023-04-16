using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitUI : AbstractUnitUI {

    public static PlayerUnitUI INSTANCE {get; private set;}

    private void  Awake() {
        if(INSTANCE == null) {
            INSTANCE = this;
            //Debug.Log("PlayerUnitUI instance created.");
        } else {
            Debug.Log("More than one PlayerUnitUI instance created.");
            Destroy(gameObject);
            return;
        }
    }

    protected override void SetBackgroundColor() {
        if(PlayerHandler.INSTANCE.IsPlayer1()) {
            background.color = player1Color;
        } else {
            background.color = player2Color;
        }
    }

}
