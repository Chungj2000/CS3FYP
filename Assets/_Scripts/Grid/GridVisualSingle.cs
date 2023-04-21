using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * A Script for handling logic of the visuals components for each GridTile.
 * Includes visibility functions and hover over effect toggles.
 */
public class GridVisualSingle : MonoBehaviour {
     
    [SerializeField] private MeshRenderer meshRenderer;

    public void ShowVisual() {
        meshRenderer.enabled = true;
    }

    public void HideVisual() {
        meshRenderer.enabled = false;
    }

    public void HoverGridVisual() {
        meshRenderer.material.color = Color.green;
    }

    public void HoverGridVisualDefault() {
        meshRenderer.material.color = Color.white;
    }

}
