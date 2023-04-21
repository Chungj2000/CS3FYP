using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * A Script which deals with visual components of each GridTile of a Grid.
 * This keeps track of the GridSystemHandler and displays or hides the visual components of each GridVisual(Single) where context applicable.
 */
public class GridVisualManager : MonoBehaviour {
    
    public static GridVisualManager INSTANCE {get; private set;}
    [SerializeField] private Transform gridVisualPf;
    [SerializeField] private LayerMask GridVisualLayerMask;

    private GridVisualSingle[,] gridVisualArray;

    private void  Awake() {
        if(INSTANCE == null) {
            INSTANCE = this;
            //Debug.Log("GridVisualManager instance created.");
        } else {
            Debug.LogError("More than one GridVisualManager instance created.");
            Destroy(gameObject);
            return;
        }
    }

    //Initiate the GridVisuals.
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
    
    private void Update() {
        UpdateGridVisual();
        HoverGridTileVisual();
    }

    //Hide every GridVisual(Single) on the entire Grid.
    public void HideAllGridVisuals() {

        for (int x = 0; x < GridSystemHandler.INSTANCE.GetWidth(); x++) {
            for (int z = 0; z < GridSystemHandler.INSTANCE.GetHeight(); z++) {

                gridVisualArray[x, z].HideVisual();

            }
        }

    }

    //Show GridVisuals from a validated list of positions from the Grid via TilePositions.
    public void ShowValidGridVisuals(List<TilePosition> tilePositionList) {

        foreach(TilePosition tilePosition in tilePositionList) {
            gridVisualArray[tilePosition.x, tilePosition.z].ShowVisual();

            //Maintain default MeshRender color of GridVisualSingles for objects that are not mouse overed.
            GridTileVisualDefault(tilePosition);
        }

    }

    //Update grid visuals whenever an action of AbstractAction is called.
    public void UpdateGridVisual() {
        HideAllGridVisuals();

        AbstractAction currentAction = UnitActionSystem.INSTANCE.GetCurrentAction();
        ShowValidGridVisuals(currentAction.ListValidActionPositions());
    }

    //Change the MeshRender colour of a mouse overed GridVisualSingle.
    private void HoverGridTileVisual() {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(mouseRay, out RaycastHit raycastHit, float.MaxValue, GridVisualLayerMask)) {

            //Debug.Log("Visual found.");

            //Get the Visual object of the hovered over GridVisualSingle and transform it's colour.
            if(raycastHit.transform.TryGetComponent<GridVisualSingle>(out GridVisualSingle gridVisual)) {
                gridVisual.HoverGridVisual();
            }

        }
    }

    //Non-hovered GridVisualSingles will default to white for their MeshRender colour.
    private void GridTileVisualDefault(TilePosition tilePosition) {
        gridVisualArray[tilePosition.x, tilePosition.z].HoverGridVisualDefault();
    }

}
