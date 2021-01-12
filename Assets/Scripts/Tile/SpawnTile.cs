using UnityEngine;

namespace PositionerDemo
{
    public class SpawnTile : Tile
    {
        private const CARDTARGETTYPE cardTargetTYPE = CARDTARGETTYPE.SPAWN;
        private const TILETYPE tileTYPE = TILETYPE.SPAWN;
        public int PlayerID { get; private set; }

        public SpawnTile(Vector3 realPosition, int PosX, int PosY, int PlayerID) : base(realPosition, PosX, PosY)
        {
            this.CardTargetType = cardTargetTYPE;
            this.tileType = tileTYPE;
            this.PlayerID = PlayerID;
        }
    }

}