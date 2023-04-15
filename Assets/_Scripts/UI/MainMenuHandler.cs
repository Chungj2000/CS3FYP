using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuHandler : AbstractMenu {
    
    private PlayMenuHandler playMenu;
    private OptionsMenuHandler optionsMenu;

    private void Awake() {
        ShowMenu();
    }

    private void Start() {
        playMenu = GetComponent<PlayMenuHandler>();
        optionsMenu = GetComponent<OptionsMenuHandler>();
    }

    //Button interactions.

    public void PlayClicked() {
        Debug.Log("Play clicked.");

        //Switch to PlayMenu view.
        playMenu.ShowMenu();
        this.HideMenu();
    }

    public void OptionsClicked() {
        Debug.Log("Options clicked.");

        //Switch to Options pop-up.
        optionsMenu.ShowMenu();
    }

    public void QuitClicked() {
        Debug.Log("Quit clicked.");
        Application.Quit();
    }

}
