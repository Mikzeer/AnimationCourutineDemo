namespace PositionerDemo
{
    public class PlayerCardAmountCardFiltter : CardFiltter
    {
        // CHEQUEAR QUE EL RIVAL/JUGADOR TENGA CARTAS TARGETTYPE BASENEXUS 
        // CheckCardAmount => PLAYER
        protected int amountToFind = 2;

        private const int FILTTER_ID = 6;

        public PlayerCardAmountCardFiltter() : base(FILTTER_ID)
        {
        }

        public override ICardTarget CheckTarget(ICardTarget cardTarget)
        {
            if (cardTarget.CardTargetType != CARDTARGETTYPE.BASENEXO) return null;

            Player player = (Player)cardTarget;

            if (player != null)
            {
                if (player.Deck.Count >= amountToFind)
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
