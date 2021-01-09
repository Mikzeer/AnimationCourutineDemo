using System.Collections.Generic;
using UnityEngine;

namespace PositionerDemo
{
    public abstract class Tile : ICardTarget
    {
        public TILETYPE tileType { get; protected set; }
        public CARDTARGETTYPE CardTargetType { get; protected set; }
        public int PosX { get; private set; }
        public int PosY { get; private set; }
        public List<Tile> NeighborsTiles { get; set; }
        public Tile[,] NeighborsTilesMatrix { get; set; }

        private IOcuppy occupier;
        protected bool _isWalkeable;
        private Board2D grid;
        private Animator tileAnimator;
        private Transform tileTransform;
        Vector3 realWorldLocation;
        public Tile(Board2D grid, int PosX, int PosY)
        {
            this.grid = grid;
            this.PosX = PosX;
            this.PosY = PosY;
            NeighborsTiles = new List<Tile>();
            NeighborsTilesMatrix = new Tile[3, 3];
        }

        public Tile(Vector3 realWorldLocation, int PosX, int PosY)
        {
            this.realWorldLocation = realWorldLocation;
            this.PosX = PosX;
            this.PosY = PosY;
            NeighborsTiles = new List<Tile>();
            NeighborsTilesMatrix = new Tile[3, 3];
        }

        public void SetGameObject(GameObject tilePrefab)
        {
            tileAnimator = tilePrefab.GetComponent<Animator>();
            tileTransform = tilePrefab.transform;
        }

        public void OcupyTile(IOcuppy occupier)
        {
            this.occupier = occupier;
        }

        public IOcuppy GetOccupier()
        {
            return occupier;
        }

        public void Vacate()
        {
            occupier = null;
        }

        public bool IsOccupied()
        {
            return occupier != null;
        }

        public bool IsWalkeable()
        {
            return _isWalkeable;
        }

        public Vector3 GetRealWorldLocation()
        {
            if (grid == null) return realWorldLocation;
            return grid.GetGridObjectRealWorldPositionByArrayPosition(PosX, PosY);
        }

        public Vector2 GetGridPosition()
        {
            return new Vector2(PosX, PosY);
        }

        public Transform GetTransform()
        {
            return tileTransform;
        }

        public Animator GetAnimator()
        {
            return tileAnimator;
        }

        public IOcuppy GetOcuppy()
        {
            return GetOccupier();
        }
    }

}