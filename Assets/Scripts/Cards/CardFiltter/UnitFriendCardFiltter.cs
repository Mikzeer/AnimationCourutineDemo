namespace PositionerDemo
{
    public class UnitFriendCardFiltter : CardFiltter
    {
        private const int FILTTER_ID = 3;

        public UnitFriendCardFiltter() : base(FILTTER_ID)
        {
        }

        public override ICardTarget CheckTarget(ICardTarget cardTarget)
        {
            Kimboko kimboko = (Kimboko)cardTarget;

            if (kimboko != null)
            {
                if (GameCreator.Instance.turnManager.GetActualPlayerTurn() == kimboko.ownerPlayer)
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
