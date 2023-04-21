using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * The UnitUI for Enemy owned units that are selected.
 */
public class EnemyUnitUI : AbstractUnitUI {
    
    public static EnemyUnitUI INSTANCE {get; private set;}

    private void  Awake() {
        if(INSTANCE == null) {
            INSTANCE = this;
            //Debug.Log("EnemyUnitUI instance created.");
        } else {
            Debug.Log("More than one EnemyUnitUI instance created.");
            Destroy(this);
            return;
        }
    }

    //Update the UI color based on Player faction.
    protected override void SetBackgroundColor() {
        if(PlayerHandler.INSTANCE.IsPlayer1()) {
            background.color = player2Color;
        } else {
            background.color = player1Color;
        }
    }

    //Ensure text field of gold is correctly updated for client.
    public override void UpdateTotalGoldField() {
        if(PlayerHandler.INSTANCE.IsPlayer1()) {
            playerTOTAL_GOLD_Text.text = GoldManager.INSTANCE.GetPlayer2TotalGold().ToString();
        } else {
            playerTOTAL_GOLD_Text.text = GoldManager.INSTANCE.GetPlayer1TotalGold().ToString();
        }
    }

}
