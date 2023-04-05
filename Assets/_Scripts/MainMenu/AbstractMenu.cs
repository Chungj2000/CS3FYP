using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractMenu : MonoBehaviour {
    
    [SerializeField] private Canvas menuCanvas;

    protected void HideMenu() {
        menuCanvas.enabled = false;
    }

    public void ShowMenu() {
        menuCanvas.enabled = true;
    }

}
