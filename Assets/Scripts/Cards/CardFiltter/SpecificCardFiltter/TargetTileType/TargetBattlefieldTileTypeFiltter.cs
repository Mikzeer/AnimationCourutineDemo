namespace PositionerDemo
{
    public class TargetBattlefieldTileTypeFiltter : TargetTileTypeFiltter
    {
        private const TILETYPE TILE_TYPE = TILETYPE.BATTLEFILED;
        private const int FILTTER_ID = 6;
        public TargetBattlefieldTileTypeFiltter() : base(TILE_TYPE, FILTTER_ID)
        {
        }
    }
}