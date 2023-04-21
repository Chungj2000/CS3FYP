using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*
 * Script used to display and update visual tooltip pop-ups for hover over events of certain visual elements.
 */
public class ToolTipsUI : MonoBehaviour {
    
    [SerializeField] private Canvas toolTipsCanvas;
    [SerializeField] private TextMeshProUGUI toolTipsText;

    private ToolTips toolTips;

    private void Start() {
        Hide();
    }

    //Attached to visual components. Used to display and update text fields of the tooltip pop-up.
    public void OnHoverEnter(ToolTips tip) {
        Show();
        toolTips = tip;
        toolTipsText.text = toolTips.IdentifyUnitUI_Tip(toolTips.GetTip());
    }

    //Conceals tooltip pop-up when no longer hovering over a visual component.
    public void OnHoverExit() {
        Hide();
    }

    //Visibility functions.
    private void Show() {
        toolTipsCanvas.enabled = true;
    }

    private void Hide() {
        toolTipsCanvas.enabled = false;
    }

}
