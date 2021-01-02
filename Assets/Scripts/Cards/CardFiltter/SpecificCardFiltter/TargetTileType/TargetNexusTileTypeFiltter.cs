namespace PositionerDemo
{
    public class TargetNexusTileTypeFiltter : TargetTileTypeFiltter
    {
        private const TILETYPE TILE_TYPE = TILETYPE.BASENEXO;
        private const int FILTTER_ID = 7;
        public TargetNexusTileTypeFiltter() : base(TILE_TYPE, FILTTER_ID)
        {
        }
    }



}