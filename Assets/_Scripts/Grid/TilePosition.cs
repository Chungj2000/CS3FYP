using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

/*
 * A custom data type for handling positions for GridTile.
 * This uses the world positions and converts them into TilePositions which are x and y values of a 2D array (Grid).
 * Also contains logic for handling calculations between TilePositions.
 */
public struct TilePosition : System.IEquatable<TilePosition> {

    public int x;
    public int z;

    public TilePosition(int x, int z) {
        this.x = x;
        this.z = z;
    }

    public override string ToString()
    {
        return "x: " + x + " | y: " + z;
    }

    public override int GetHashCode()
    {
        return System.HashCode.Combine(x, z);
    }

    public override bool Equals(object obj)
    {
        return obj is TilePosition position &&
            x == position.x &&
            z == position.z;
    }

    public bool Equals(TilePosition other) {
        return this == other;
    }

    //Bool equals logic.
    public static bool operator == (TilePosition a, TilePosition b) {
        return a.x == b.x && a.z == b.z;
    }

    //Bool not equals logic.
    public static bool operator != (TilePosition a, TilePosition b) {
        return !(a == b);
    }

    //Adding logic.
    public static TilePosition operator +(TilePosition a, TilePosition b) {
        return new TilePosition(a.x + b.x, a.z + b.z);
    }

    //Subtracting logic.
    public static TilePosition operator -(TilePosition a, TilePosition b) {
        return new TilePosition(a.x - b.x, a.z - b.z);
    }

}
