using UnityEngine;

namespace PositionerDemo
{
    public class BattlefieldTile : Tile
    {
        private const CARDTARGETTYPE cardTargetTYPE = CARDTARGETTYPE.BATTLEFIELD;
        private const TILETYPE tileTYPE = TILETYPE.BATTLEFILED;

        public BattlefieldTile(Board2D grid, int PosX, int PosY) : base(grid, PosX, PosY)
        {
            this.CardTargetType = cardTargetTYPE;
            this.tileType = tileTYPE;
            this._isWalkeable = true;
        }

        public BattlefieldTile(Vector3 realPosition, int PosX, int PosY) : base(realPosition, PosX, PosY)
        {
            this.CardTargetType = cardTargetTYPE;
            this.tileType = tileTYPE;
            this._isWalkeable = true;
        }
    }

}