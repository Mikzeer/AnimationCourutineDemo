
namespace PositionerDemo
{
    public abstract class TargetTileTypeFiltter : CardFiltter
    {
        TILETYPE tileType;
        CARDTARGETTYPE cardTargetType;
        public TargetTileTypeFiltter(TILETYPE tileType, int ID) : base(ID)
        {
            this.tileType = tileType;
            switch (tileType)
            {
                case TILETYPE.BASENEXO:
                    cardTargetType = CARDTARGETTYPE.BASENEXO;
                    break;
                case TILETYPE.SPAWN:
                    cardTargetType = CARDTARGETTYPE.SPAWN;
                    break;
                case TILETYPE.BATTLEFILED:
                    cardTargetType = CARDTARGETTYPE.BATTLEFIELD;
                    break;
                default:
                    cardTargetType = CARDTARGETTYPE.BATTLEFIELD;
                    break;
            }
        }

        public override ICardTarget CheckTarget(ICardTarget cardTarget)
        {
            if (cardTarget.CardTargetType != cardTargetType) return null;
            Tile tile = (Tile)cardTarget;
            if (tile == null) return null;
            if (tile.tileType != tileType) return null;
            return base.CheckTarget(cardTarget);
        }
    }
}