using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridDebugPrefab : MonoBehaviour
{
    
    [SerializeField] private TextMeshPro text;
    private GridTile gridTile;

    public void SetGridTile(GridTile gt) {
        gridTile = gt;
    }

    private void Update() {
        text.text = gridTile.ToString();
    }

}
