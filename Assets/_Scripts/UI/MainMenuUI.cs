using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Script solely responsible for the visual components of the MainMenuUI.
 */
public class MainMenuUI : AbstractMainMenu {
    
    private PlayMenuUI playMenu;
    private OptionsMenuUI optionsMenu;

    private void Awake() {
        ShowMenu();
    }

    private void Start() {
        playMenu = GetComponent<PlayMenuUI>();
        optionsMenu = GetComponent<OptionsMenuUI>();
    }

    //Button functions.
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
