using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ToolTipsUI : MonoBehaviour {
    
    [SerializeField] private Canvas toolTipsCanvas;
    [SerializeField] private TextMeshProUGUI toolTipsText;

    private ToolTips toolTips;

    private void Start() {
        Hide();
    }

    public void OnHoverEnter(ToolTips tip) {
        Show();
        toolTips = tip;
        toolTipsText.text = toolTips.IdentifyUnitUI_Tip(toolTips.GetTip());
    }

    public void OnHoverExit() {
        Hide();
    }

    private void Show() {
        toolTipsCanvas.enabled = true;
    }

    private void Hide() {
        toolTipsCanvas.enabled = false;
    }

}
