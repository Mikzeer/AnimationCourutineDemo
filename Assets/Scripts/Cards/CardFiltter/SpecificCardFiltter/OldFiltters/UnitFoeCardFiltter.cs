namespace PositionerDemo
{
    public class UnitFoeCardFiltter : CardFiltter
    {
        private const int FILTTER_ID = 4;

        public UnitFoeCardFiltter() : base(FILTTER_ID)
        {
        }

        public override ICardTarget CheckTarget(ICardTarget cardTarget)
        {
            if (cardTarget.CardTargetType != CARDTARGETTYPE.UNIT)
            {
                return null;
            }

            Kimboko kimboko = (Kimboko)cardTarget;

            if (kimboko != null)
            {
                if (GameCreator.Instance.turnManager.GetActualPlayerTurn() != kimboko.ownerPlayer)
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
