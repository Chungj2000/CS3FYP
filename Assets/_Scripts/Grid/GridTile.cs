using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * A Script that serves as a data type for instantiating a Grid using a 2D array.
 * It keeps track of what items are present on the GridTile such as the Unit, and its position (TilePosition).
 */
public class GridTile {
    
    private GridSystem gridSystem;
    private TilePosition tilePosition;
    private List<UnitHandler> units;

    public GridTile(GridSystem gs, TilePosition tp) {
        gridSystem = gs;
        tilePosition = tp;
        units = new List<UnitHandler>();
    }

    public override string ToString()
    {
        string buildUnitString = "";
        foreach(UnitHandler unit in units) {
            buildUnitString += unit + "\n";
        }
        return tilePosition.ToString() + "\n" + buildUnitString;
    }

    //Data manipulation for Units present on GridTile.
    public void AddUnit(UnitHandler unit) {
        units.Add(unit);
    }

    public void RemoveUnit(UnitHandler unit) {
        units.Remove(unit);
    }

    //Getters.
    public List<UnitHandler> GetUnitList() {
        return units;
    }

    public bool HasAUnit() {
        return units.Count > 0;
    }

    public UnitHandler GetUnit() {
        if(HasAUnit()) {
            //Debug.Log("Unit found.");
            return units[0];
        } else {
            //Debug.Log("No Units found.");
            return null;
        }
    }

}
