using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class WaitingScreenUI : MonoBehaviourPunCallbacks {
    
    [SerializeField] private Canvas waitingScreen;
    [SerializeField] private WaitingScreenAnimation animation;

    private TurnSystemUI turnSystemUI;

    private void Awake() {
        turnSystemUI = GetComponent<TurnSystemUI>();
    }

    private void Start() {
        ShowScreen();
    }

    private void Update() {
        //Debug.Log("Number of players: " + PhotonNetwork.CurrentRoom.PlayerCount);

        if(WaitingForPlayerToConnect()) {
            animation.TurnOnAnimation();
            ShowScreen();
        } else {
            //Player 2 has joined therefore initiate game.
            Initialise();
        }
    }

    //Check whether all Players (2) are present.
    private bool WaitingForPlayerToConnect() {
        if(PhotonNetwork.CurrentRoom.PlayerCount != 2) {
            return true;
        } else {
            return false;
        }
    }

    private void ShowScreen() {
        waitingScreen.enabled = true;
    }

    private void HideScreen() {
        waitingScreen.enabled = false;
    }

    private void StartGame() {
        //Tell the GameOverHandler to start running Update.
        GameOverHandler.INSTANCE.SetGameIsActive();
        Destroy(this);
    }

    //Determine what Player the client is based on join order.
    private void DesignatePlayer() {
        //Debug.Log("Player is: " + PhotonNetwork.LocalPlayer.ActorNumber);
        if(PhotonNetwork.LocalPlayer.ActorNumber == 1) {
            Debug.Log("Client is Player 1.");
            PlayerHandler.INSTANCE.SetAsPlayer1();
        } else {
            Debug.Log("Client is Player 2.");
            PlayerHandler.INSTANCE.SetAsPlayer2();
        }
    }

    //Adjust visibility of UI and settings.
    private void Initialise() {
        animation.TurnOffAnimation();
        HideScreen();
        DesignatePlayer();
        turnSystemUI.ToggleButtonVisibility();
        StartGame();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) {
        Debug.Log("Player " + newPlayer.ActorNumber + " has joined.");
    }

}
