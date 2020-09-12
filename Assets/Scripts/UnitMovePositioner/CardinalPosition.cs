using UnityEngine;
namespace PositionerDemo
{
    public static class CardinalPosition
    {
        // "x" crece de izquierda a derecha en world position
        // "y" crece de abajo hacia arriba en world postion
        public static readonly Vector3 CENTER = new Vector3(0, 0);
        public static readonly Vector3 SOUTH = new Vector3(0, -1);
        public static readonly Vector3 NORTH = new Vector3(0, 1);
        public static readonly Vector3 EAST = new Vector3(1, 0);
        public static readonly Vector3 WEST = new Vector3(-1, 0);
        public static readonly Vector3 NORTHEAST = new Vector3(1, 1);
        public static readonly Vector3 NORTHWEST = new Vector3(-1, 1);
        public static readonly Vector3 SOUTHEAST = new Vector3(1, -1);
        public static readonly Vector3 SOUTHWEST = new Vector3(-1, -1);
    }

}

