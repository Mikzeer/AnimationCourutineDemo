namespace PositionerDemo
{
    public class FreeSpaceInBattlefieldCardFiltter : CardFiltter
    {
        private const int FILTTER_ID = 7;

        public FreeSpaceInBattlefieldCardFiltter() : base(FILTTER_ID)
        {
        }

        public override ICardTarget CheckTarget(ICardTarget cardTarget)
        {
            if (cardTarget.CardTargetType != CARDTARGETTYPE.BATTLEFIELD)
            {
                return null;
            }

            Tile tile = (Tile)cardTarget;

            if (tile != null)
            {
                if (tile.tileType == TILETYPE.BATTLEFILED && tile.IsOccupied() == false)
                {
                    return cardTarget;
                }
            }
            else
            {
                return null;
            }

            return null;
        }
    }
}
