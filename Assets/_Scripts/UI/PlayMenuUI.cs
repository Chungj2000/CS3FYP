using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayMenuUI : AbstractMainMenu {

    private MainMenuUI mainMenu;
    private CreateSessionHandler sessionHandler;

    private const string play = "GameScene";
    private const string characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const int sessionCharacterAmount = 8;

    private void Awake() {
        //Start off as initially closed.
        HideMenu();
    }

    private void Start() {
        mainMenu = GetComponent<MainMenuUI>();
        sessionHandler = GetComponent<CreateSessionHandler>();
    }

    public void CreateSessionClicked() {
        Debug.Log("Create Session clicked.");

        //Create a random key;
        //CreateRandomSessionKey();

        sessionHandler.CreateSession();
        
    }

    public void JoinSessionClicked() {
        Debug.Log("Join Session clicked.");
        sessionHandler.JoinSession();
    }

    public void BackToMainMenu() {
        Debug.Log("Going back to MainMenu view.");

        //Switch to MainMenu view.
        mainMenu.ShowMenu();
        this.HideMenu();
    }

    //Create a random session key using the alphabet with 8 characters.
    private void CreateRandomSessionKey() {

        Debug.Log("Generating a random key.");

        string sessionKey = "";

        for(int i = 0; i < sessionCharacterAmount; i++) {
            sessionKey += characters[UnityEngine.Random.Range(0, characters.Length)];
        }

        Debug.Log("Session key is: " + sessionKey);

        sessionHandler.GetCreateSessionKey().text = sessionKey;

    }
    
}
