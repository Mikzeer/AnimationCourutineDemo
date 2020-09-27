using System.Collections.Generic;
using UnityEngine;

namespace PositionerDemo
{
    public abstract class Tile
    {
        private TILETYPE _tileType;
        public TILETYPE tileType { get => _tileType; protected set => _tileType = value; }
        private CARDTARGETTYPE _cardTargetType;
        public CARDTARGETTYPE CardTargetType { get => _cardTargetType; protected set => _cardTargetType = value; }
        private int _posX;
        public int PosX { get => _posX; private set => _posX = value; }
        private int _posY;
        public int PosY { get => _posY; private set => _posY = value; }
        private List<Tile> _neighborsTiles;
        public List<Tile> NeighborsTiles { get => _neighborsTiles; set => _neighborsTiles = value; }
        private Tile[,] _neighborsTilesMatrix;
        public Tile[,] NeighborsTilesMatrix { get => _neighborsTilesMatrix; set => _neighborsTilesMatrix = value; }

        private IOcuppy occupier;
        protected bool _isWalkeable;
        private Board2D grid;
        private Animator tileAnimator;
        private Transform tileTransform;

        public Tile(Board2D grid, int PosX, int PosY)
        {
            this.grid = grid;
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
            return grid.GetGridObjectRealWorldPositionByArrayPosition(PosX, PosY);
        }

        public Transform GetTransform()
        {
            return tileTransform;
        }

        public Animator GetAnimator()
        {
            return tileAnimator;
        }

    }

}