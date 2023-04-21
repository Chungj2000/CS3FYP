using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * An abstract class for MainMenu Scripts for base visibility functions.
 */
public abstract class AbstractMainMenu : MonoBehaviour {
    
    [SerializeField] private Canvas menuCanvas;

    protected void HideMenu() {
        menuCanvas.enabled = false;
    }

    public void ShowMenu() {
        menuCanvas.enabled = true;
    }

}
