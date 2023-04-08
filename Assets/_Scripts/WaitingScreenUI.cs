using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class WaitingScreenUI : MonoBehaviourPunCallbacks {
    
    [SerializeField] private Canvas waitingScreen;
    [SerializeField] private WaitingScreenAnimation animation;

    private void Start() {
        ShowScreen();
    }

    private void Update() {
        //Debug.Log("Number of players: " + PhotonNetwork.CurrentRoom.PlayerCount);

        if(WaitingForPlayerToConnect()) {
            animation.TurnOnAnimation();
            ShowScreen();
        } else {
            animation.TurnOffAnimation();
            HideScreen();
            StartGame();
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

    public override void OnPlayerEnteredRoom(Player newPlayer) {
        Debug.Log("Player " + newPlayer.ActorNumber + " has joined.");
    }

}
