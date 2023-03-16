using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem {
    
    //private GridXbyZ<Tile> grid;
    [SerializeField] int gridHeight = 10;
    [SerializeField] int gridWidth = 10;
    [SerializeField] float tileSize = 2f;

    private GridTile[,] gridTiles;

    public GridSystem(int height, int width, float size) {
        gridHeight = height;
        gridWidth = width;
        tileSize = size;

        gridTiles = new GridTile[gridWidth, gridHeight];

        for(int x = 0; x < gridWidth; x++) {
            for(int z = 0; z < gridHeight; z++) {

                //Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z) + Vector3.right * 2f, Color.white, 1000);
                TilePosition tilePosition = new TilePosition(x, z);
                gridTiles[x, z] = new GridTile(this, tilePosition);

            }
        }
    }

    public Vector3 GetWorldPosition(TilePosition tilePosition) {
        return new Vector3(tilePosition.x, 0, tilePosition.z) * tileSize;
    }

    public TilePosition GetTilePosition(Vector3 worldPosition) {
        return new TilePosition(Mathf.RoundToInt(worldPosition.x / tileSize), Mathf.RoundToInt(worldPosition.z / tileSize));    
    }

    public void CreateDebugobjects(Transform debugPrefab) {
        for(int x = 0; x < gridWidth; x++) {
            for(int z = 0; z < gridHeight; z++) {

                TilePosition tilePosition = new TilePosition(x, z);

                Transform debugTransform = GameObject.Instantiate(debugPrefab, GetWorldPosition(tilePosition), Quaternion.identity);
                GridDebugPrefab gridDebugPrefab = debugTransform.GetComponent<GridDebugPrefab>();
                gridDebugPrefab.SetGridTile(GetGridTile(tilePosition));

            }
        }
    }

    public GridTile GetGridTile(TilePosition tilePosition) {
        return gridTiles[tilePosition.x, tilePosition.z];
    }

    public bool IsValidPosition(TilePosition position) {
        return position.x >= 0 && 
               position.z >= 0 && 
               position.x < gridWidth && 
               position.z < gridHeight;
    }

    public int GetHeight() {
        return gridHeight;
    }

    public int GetWidth() {
        return gridWidth;
    }

}
