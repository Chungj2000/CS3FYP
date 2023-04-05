using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenuHandler : AbstractMenu {

    private void Awake() {
        HideMenu();
    }

    public void BackToMainMenu() {
        Debug.Log("Closing Options pop-up.");

        //Hide the pop-up.
        this.HideMenu();
    }

    public void ApplyClicked() {
        Debug.Log("Apply clicked.");

        //Finalize and confirm changes to settings.
        ApplyChanges();

        //Hide the pop-up.
        this.HideMenu();
    }

    private void ApplyChanges() {
        Debug.Log("Settings applied.");
    }
    
}
