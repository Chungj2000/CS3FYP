using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridVisualSingle : MonoBehaviour {
     
    [SerializeField] private MeshRenderer meshRenderer;

    public void ShowVisual() {
        meshRenderer.enabled = true;
    }

    public void HideVisual() {
        meshRenderer.enabled = false;
    }
}
