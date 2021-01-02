namespace PositionerDemo
{
    public class TargetSpawnTileTypeFiltter : TargetTileTypeFiltter
    {
        private const TILETYPE TILE_TYPE = TILETYPE.SPAWN;
        private const int FILTTER_ID = 5;
        public TargetSpawnTileTypeFiltter() : base(TILE_TYPE, FILTTER_ID)
        {
        }
    }



}