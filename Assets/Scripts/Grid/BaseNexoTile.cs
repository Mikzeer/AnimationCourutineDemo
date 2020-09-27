namespace PositionerDemo
{
    public class BaseNexoTile : Tile
    {
        private const CARDTARGETTYPE cardTargetTYPE = CARDTARGETTYPE.BASENEXO;
        private const TILETYPE tileTYPE = TILETYPE.BASENEXO;

        private int _playerID;
        public int PlayerID { get => _playerID; private set => _playerID = value; }

        public BaseNexoTile(Board2D grid, int PosX, int PosY, Player player) : base(grid, PosX, PosY)
        {
            this.CardTargetType = cardTargetTYPE;
            this.tileType = tileTYPE;
            this._isWalkeable = false;
            this.PlayerID = player.PlayerID;
            OcupyTile(player);
        }
    }

}