using UnityEngine;
namespace PositionerDemo
{
    public static class ArrayCardinalPosition
    {
        // x y 0   1  2
        // 0   NW  N  NE 
        // 1   W   C  E
        // 2   SW  S  SE
        public static readonly Vector2Int CENTER = new Vector2Int(1, 1);
        public static readonly Vector2Int SOUTH = new Vector2Int(1, 2);
        public static readonly Vector2Int NORTH = new Vector2Int(1, 0);
        public static readonly Vector2Int EAST = new Vector2Int(2, 1);
        public static readonly Vector2Int WEST = new Vector2Int(0, 1);
        public static readonly Vector2Int NORTHEAST = new Vector2Int(2, 0);
        public static readonly Vector2Int NORTHWEST = new Vector2Int(0, 0);
        public static readonly Vector2Int SOUTHEAST = new Vector2Int(2, 2);
        public static readonly Vector2Int SOUTHWEST = new Vector2Int(0, 2);
    }

}

