using System.Collections.Generic;
using UnityEngine;

namespace PositionerDemo
{
    public abstract class Tile : ICardTarget
    {
        public GameObjectAnimatorContainer goAnimContainer { get; private set; }
        public TILETYPE tileType { get; protected set; }
        public CARDTARGETTYPE CardTargetType { get; protected set; }
        public Position position { get; private set; }
        public Tile[,] NeighborsTilesMatrix { get; set; }
        private IOcuppy occupier;
        private Vector3 realWorldLocation;

        public Tile(Vector3 realWorldLocation, int PosX, int PosY)
        {
            this.realWorldLocation = realWorldLocation;
            position = new Position(PosX, PosY);
            NeighborsTilesMatrix = new Tile[3, 3];
        }

        public void OcupyTile(IOcuppy occupier)
        {
            this.occupier = occupier;
        }

        public IOcuppy GetOcuppy()
        {
            return occupier;
        }

        public bool IsOccupied()
        {
            return GetOcuppy() != null;
        }

        public void Vacate()
        {
            occupier = null;
        }

        public Vector3 GetRealWorldLocation()
        {
            return realWorldLocation;
        }

        public Vector2Int GetGridPosition()
        {
            return new Vector2Int(position.posX, position.posY);
        }

        public void SetGoAnimContainer(GameObjectAnimatorContainer goAnimCon)
        {
            goAnimContainer = goAnimCon;
        }
    }
}