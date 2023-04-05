using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayMenuHandler : AbstractMenu {

    MainMenuHandler mainMenu;
    private const string play = "GameScene";

    private void Awake() {
        //Start off as initially closed.
        HideMenu();
    }

    private void Start() {
        mainMenu = GetComponent<MainMenuHandler>();
    }

    public void CreateSessionClicked() {
        Debug.Log("Play clicked.");
        SceneManager.LoadSceneAsync(play);
    }

    public void BackToMainMenu() {
        Debug.Log("Going back to MainMenu view.");

        //Switch to MainMenu view.
        mainMenu.ShowMenu();
        this.HideMenu();
    }
    
}
