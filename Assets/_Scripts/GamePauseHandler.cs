using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePauseHandler : MonoBehaviour {
    
    public static GamePauseHandler INSTANCE {get; private set;}

    [SerializeField] private Canvas pauseMenuCanvas;

    private InputActions pauseInput;
    private bool isPaused;

    private const string mainMenu = "MainMenu";

    private void  Awake() {

        //Start the game in an unpaused state.
        isPaused = false;

        pauseInput = new InputActions();
        pauseInput.Pause.Enable();

        if(INSTANCE == null) {
            INSTANCE = this;
            //Debug.Log("GamePauseHandler instance created.");
        } else {
            Debug.LogError("More than one GamePauseHandler instance created.");
            Destroy(gameObject);
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

    private void PauseMenuDisplay() {
        pauseMenuCanvas.enabled = isPaused;
    }

    public void TogglePause() {
        isPaused = !isPaused;
        PauseMenuDisplay();
    }

    public bool IsPaused() {
        return isPaused;
    }

    public void ResumeClicked() {
        Debug.Log("Resume clicked.");

        //Change pause state when resumed.
        TogglePause();
    }

    public void OptionsClicked() {
        Debug.Log("Options clicked.");
    }

    public void GiveUpClicked() {
        Debug.Log("Give Up clicked.");

        //Some game over logic.

        SceneManager.LoadSceneAsync(mainMenu);
    }

    public void QuitClicked() {
        Debug.Log("Quit clicked.");

        //Some game over logic.

        Application.Quit();
    }

}
