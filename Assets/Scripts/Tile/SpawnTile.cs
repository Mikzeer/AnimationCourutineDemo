using UnityEngine;

namespace PositionerDemo
{
    public class SpawnTile : Tile
    {
        private const CARDTARGETTYPE cardTargetTYPE = CARDTARGETTYPE.SPAWN;
        private const TILETYPE tileTYPE = TILETYPE.SPAWN;

        private int _playerID;
        public int PlayerID { get => _playerID; private set => _playerID = value; }

        public SpawnTile(Board2D grid, int PosX, int PosY, int PlayerID) : base(grid, PosX, PosY)
        {
            this.CardTargetType = cardTargetTYPE;
            this.tileType = tileTYPE;
            this._isWalkeable = true;
            this.PlayerID = PlayerID;
        }

        public SpawnTile(Vector3 realPosition, int PosX, int PosY, int PlayerID) : base(realPosition, PosX, PosY)
        {
            this.CardTargetType = cardTargetTYPE;
            this.tileType = tileTYPE;
            this._isWalkeable = true;
            this.PlayerID = PlayerID;
        }
    }

}