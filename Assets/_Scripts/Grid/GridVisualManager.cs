using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridVisualManager : MonoBehaviour {
    
    public static GridVisualManager INSTANCE {get; private set;}
    [SerializeField] private Transform gridVisualPf;

    private GridVisualSingle[,] gridVisualArray;

    private void  Awake() {
        if(INSTANCE == null) {
            INSTANCE = this;
            Debug.Log("GridVisualManager instance created.");
        } else {
            Debug.LogError("More than one GridVisualManager instance created.");
            Destroy(gameObject);
            return;
        }
    }

    private void Start() {

        gridVisualArray = new GridVisualSingle[GridSystemHandler.INSTANCE.GetWidth(), GridSystemHandler.INSTANCE.GetHeight()];

        //Generate a grid visual object on every valid grid tile.
        for (int x = 0; x < GridSystemHandler.INSTANCE.GetWidth(); x++) {
            for (int z = 0; z < GridSystemHandler.INSTANCE.GetHeight(); z++) {

                TilePosition tilePosition = new TilePosition(x, z);
                Transform gridVisualObject = Instantiate(gridVisualPf, GridSystemHandler.INSTANCE.GetWorldPosition(tilePosition), Quaternion.identity);

                gridVisualArray[x, z] = gridVisualObject.GetComponent<GridVisualSingle>();

            }
        }

    }

    public void HideAllGridVisuals() {

        for (int x = 0; x < GridSystemHandler.INSTANCE.GetWidth(); x++) {
            for (int z = 0; z < GridSystemHandler.INSTANCE.GetHeight(); z++) {

                gridVisualArray[x, z].HideVisual();

            }
        }

    }

    public void ShowValidGridVisuals(List<TilePosition> tilePositionList) {

        foreach(TilePosition tilePosition in tilePositionList) {
            gridVisualArray[tilePosition.x, tilePosition.z].ShowVisual();
        }

    }

    public void UpdateGridVisual() {
        HideAllGridVisuals();

        UnitHandler selectedUnit = UnitActionSystem.INSTANCE.GetSelectedUnit();
        ShowValidGridVisuals(selectedUnit.GetMoveAction().ListValidMovePositions());
    }

    private void Update() {
        UpdateGridVisual();
    }

}
