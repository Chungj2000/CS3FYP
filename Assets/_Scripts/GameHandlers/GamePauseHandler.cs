using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/*
 * Script for visual components of the PauseMenuUI on the GameScene and the logic behind giving up.
 */
public class GamePauseHandler : MonoBehaviour {
    
    public static GamePauseHandler INSTANCE {get; private set;}

    [SerializeField] private Canvas pauseMenuCanvas;

    private PhotonView view;
    private InputActions pauseInput;
    private OptionsMenuUI options;
    private bool isPaused, hasNotGivenUp;

    private void  Awake() {

        view = GetComponent<PhotonView>();
        options = GetComponent<OptionsMenuUI>();

        //Start the game in an unpaused state.
        isPaused = false;
        hasNotGivenUp = true;

        pauseInput = new InputActions();
        pauseInput.Pause.Enable();

        if(INSTANCE == null) {
            INSTANCE = this;
            //Debug.Log("GamePauseHandler instance created.");
        } else {
            Debug.LogError("More than one GamePauseHandler instance created.");
            Destroy(this);
            return;
        }

    }

    private void Start() {
        PauseMenuDisplay();
    }

    private void Update() {
        //Change canvas display settings whenever pause keybind is pressed.
        if(pauseInput.Pause.Pause.triggered) {
            TogglePause();
        }
    }

    //Visibility functions.
    private void PauseMenuDisplay() {
        pauseMenuCanvas.enabled = isPaused;
    }

    //Switch between ON or OFF.
    public void TogglePause() {
        isPaused = !isPaused;
        PauseMenuDisplay();
    }

    //Forcefully disable pause menu instead of toggling.
    public void TurnOffPause() {
        isPaused = false;
        PauseMenuDisplay();
    }

    public bool IsPaused() {
        return isPaused;
    }

    //Button functions.
    public void ResumeClicked() {
        Debug.Log("Resume clicked.");

        //Change pause state when resumed.
        TogglePause();
    }

    public void OptionsClicked() {
        Debug.Log("Options clicked.");

        options.InitialiseOptions();
    }

    public void GiveUpClicked() {
        Debug.Log("Give Up clicked.");

        hasNotGivenUp = false;
        Debug.Log("Player has given up.");

        //Display GameOver UI for defeat.
        GameOverHandler.INSTANCE.DeclareGameOver(hasNotGivenUp);
        view.RPC(nameof(GameOverHandler.INSTANCE.DeclareGameOver), RpcTarget.AllBuffered, true);
    }

    public void QuitClicked() {
        Debug.Log("Quit clicked.");

        //Some game over logic.

        Application.Quit();
    }

}
