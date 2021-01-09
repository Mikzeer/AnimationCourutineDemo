using UnityEngine;

namespace PositionerDemo
{
    public class BaseNexoTile : Tile
    {
        private const CARDTARGETTYPE cardTargetTYPE = CARDTARGETTYPE.BASENEXO;
        private const TILETYPE tileTYPE = TILETYPE.BASENEXO;
        public int PlayerID { get; private set; }

        public BaseNexoTile(Board2D grid, int PosX, int PosY, Player player) : base(grid, PosX, PosY)
        {
            this.CardTargetType = cardTargetTYPE;
            this.tileType = tileTYPE;
            this._isWalkeable = false;
            this.PlayerID = player.PlayerID;
            OcupyTile(player);
        }

        public BaseNexoTile(Vector3 realPositon, int PosX, int PosY, Player player) : base(realPositon, PosX, PosY)
        {
            this.CardTargetType = cardTargetTYPE;
            this.tileType = tileTYPE;
            this._isWalkeable = false;
            this.PlayerID = player.PlayerID;
            OcupyTile(player);
        }

    }

}