namespace PositionerDemo
{
    public class PlayerFoeCardFiltter : CardFiltter
    {
        private const int FILTTER_ID = 2;

        public PlayerFoeCardFiltter() : base(FILTTER_ID)
        {
        }

        public override ICardTarget CheckTarget(ICardTarget cardTarget)
        {
            if (cardTarget.CardTargetType != CARDTARGETTYPE.BASENEXO) return null;

            Player player = (Player)cardTarget;

            if (player != null)
            {
                if (GameCreator.Instance.turnManager.GetActualPlayerTurn() != player)
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
