using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnitUI : AbstractUnitUI {
    
    public static EnemyUnitUI INSTANCE {get; private set;}

    private void  Awake() {
        if(INSTANCE == null) {
            INSTANCE = this;
            //Debug.Log("EnemyUnitUI instance created.");
        } else {
            Debug.Log("More than one EnemyUnitUI instance created.");
            Destroy(gameObject);
            return;
        }
    }

    protected override void SetBackgroundColor() {
        if(PlayerHandler.INSTANCE.IsPlayer1()) {
            background.color = player2Color;
        } else {
            background.color = player1Color;
        }
    }

}
