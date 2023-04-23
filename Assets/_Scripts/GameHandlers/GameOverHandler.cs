using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

/*
 * Script used to determine whether win conditions have been met, and declaring a winner out of the two Players.
 * Additionally handles the visual UI components for winning and losing.
 */
public class GameOverHandler : MonoBehaviour {

    public static GameOverHandler INSTANCE {get; private set;}

    [SerializeField] private Canvas gameOverWinScreen;
    [SerializeField] private Canvas gameOverLoseScreen;

    private const string mainMenu = "MainMenu";
    
    private bool isVictor, isGameActive, gameIsOver;
    private AudioSource sfxSource;

    private void Awake() {
        sfxSource = GetComponent<AudioSource>();

        isGameActive = false;
        gameIsOver = false;

        if(INSTANCE == null) {
            INSTANCE = this;
            //Debug.Log("GameOverHandler instance created.");
        } else {
            Debug.LogError("More than one GameOverHandler instance created.");
            Destroy(this);
            return;
        }
    }

    private void Start() {
        //Conceal both screens when game begins.
        HideScreen(gameOverWinScreen);
        HideScreen(gameOverLoseScreen);
    }

    private void Update() {
        //If the game has begun check whenever a player leaves.
        if(isGameActive) {
            CheckIsGameStillActive();
        }
    }

    public void SetGameIsActive() {
        isGameActive = true;
    }

    private void CheckIsGameStillActive() {
        //If the Player count is less than 2, the other Player has left, therefore declare client as winner.
        if(PhotonNetwork.CurrentRoom.PlayerCount < 2) {
            //Debug.Log("A Player has left.");
            DeclareGameOver(true);
        }
    }

    //Announce GameOver state for both clients.
    [PunRPC]
    public void DeclareGameOver(bool victor) {
        isVictor = victor;
        IdentifyGameOverState();

        TurnSystem.INSTANCE.TurnOffTimer();

        gameIsOver = true;
    }

    //Identify whether the Player is the winner/loser.
    private void IdentifyGameOverState() {
        //If the game is over, do not allow further game over calls.
        if(gameIsOver) {
            //Debug.Log("Game is over.");
            return;
        }

        if(isVictor) {
            Debug.Log("Player is the winner.");

            //Display only the GameOverUI.
            GamePauseHandler.INSTANCE.TurnOffPause();
            DeclareClientWin();
        } else {
            Debug.Log("Player is the loser.");

            //Display only the GameOverUI.
            GamePauseHandler.INSTANCE.TurnOffPause();
            DeclareClientLose();
        }

        //Play audio clip for game over.
        SoundSystem.INSTANCE.PlaySFX(SoundSystem.INSTANCE.GetGameOverSFX());
    }

    //Declaration methods.
    private void DeclareClientWin() {
        ShowScreen(gameOverWinScreen);
    }

    private void DeclareClientLose() {
        ShowScreen(gameOverLoseScreen);
    }

    //UI visibility methods.
    private void HideScreen(Canvas screen) {
        screen.enabled = false;
    }

    private void ShowScreen(Canvas screen) {
        screen.enabled = true;
    }

    //Button functions.
    public void MainMenuClicked() {
        Debug.Log("Main Menu clicked.");
        PhotonNetwork.LeaveRoom();

        Debug.Log("Player has left the room.");
        SceneManager.LoadSceneAsync(mainMenu);
    }

    public void QuitClicked() {
        Debug.Log("Quit clicked.");

        PhotonNetwork.LeaveRoom();
        Debug.Log("Player has left the room.");

        Application.Quit();
    }

    //Getters.
    public bool CheckGameIsOver() {
        return gameIsOver;
    }

}
