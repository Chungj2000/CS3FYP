using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * The UnitUI for Player owned units that are selected.
 */
public class PlayerUnitUI : AbstractUnitUI {

    public static PlayerUnitUI INSTANCE {get; private set;}

    private void  Awake() {
        if(INSTANCE == null) {
            INSTANCE = this;
            //Debug.Log("PlayerUnitUI instance created.");
        } else {
            Debug.Log("More than one PlayerUnitUI instance created.");
            Destroy(this);
            return;
        }
    }

    //Update the UI color based on Player faction.
    protected override void SetBackgroundColor() {
        if(PlayerHandler.INSTANCE.IsPlayer1()) {
            background.color = player1Color;
        } else {
            background.color = player2Color;
        }
    }

    //Ensure text field of gold is correctly updated for client.
    public override void UpdateTotalGoldField() {
        if(PlayerHandler.INSTANCE.IsPlayer1()) {
            playerTOTAL_GOLD_Text.text = GoldManager.INSTANCE.GetPlayer1TotalGold().ToString();
        } else {
            playerTOTAL_GOLD_Text.text = GoldManager.INSTANCE.GetPlayer2TotalGold().ToString();
        }
    }

}
