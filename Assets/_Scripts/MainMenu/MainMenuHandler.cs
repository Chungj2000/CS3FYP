using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour {
    
    private const string newGame = "GameScene";

    public void PlayClicked() {
        Debug.Log("Play clicked.");
        SceneManager.LoadSceneAsync(newGame);
    }

    public void OptionsClicked() {
        Debug.Log("Options clicked.");
    }

    public void QuitClicked() {
        Debug.Log("Quit clicked.");
        Application.Quit();
    }

}
