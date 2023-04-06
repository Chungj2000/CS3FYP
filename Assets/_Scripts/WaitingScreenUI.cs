using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WaitingScreenUI : MonoBehaviour {
    
    [SerializeField] private Canvas waitingScreen;
    [SerializeField] private WaitingScreenAnimation animation;

    private void Start() {
        ShowScreen();
    }

    private void Update() {
        Debug.Log("Number of players: " + PhotonNetwork.CurrentRoom.PlayerCount);

        if(WaitingForPlayerToConnect()) {
            ShowScreen();
        } else {
            animation.ToggleIsAnimating();
            HideScreen();
            //StartGame();
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
        Destroy(this);
    }

}
