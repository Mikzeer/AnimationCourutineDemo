namespace PositionerDemo
{
    public class TargetAmountCardStateFiltterAgianstSimple : TargetCardStateFiltter
    {
        CARDSTATES cardState;
        private const int FILTTER_ID = 40;
        public TargetAmountCardStateFiltterAgianstSimple(CARDSTATES cardState, int amountToCheck, COMPARATIONTYPE comparationType) : base(comparationType, FILTTER_ID)
        {
            rdToCheckAgainst = new SimpleIResultData(amountToCheck);
            this.cardState = cardState;
        }

        public override ICardTarget CheckTarget(ICardTarget cardTarget)
        {
            cardTarget = base.CheckTarget(cardTarget);
            if (cardTarget == null) return null;
            Player player = (Player)cardTarget;

            switch (cardState)
            {
                case CARDSTATES.DECK:
                    cardDataToCheck = new SimpleIResultData(player.Deck.Count);
                    break;
                case CARDSTATES.HAND:
                    cardDataToCheck = new SimpleIResultData(player.PlayersHands.Count);
                    break;
                case CARDSTATES.CEMENTERY:
                    cardDataToCheck = new SimpleIResultData(player.Graveyard.Count);
                    break;
                case CARDSTATES.WAITFORUSE:
                    cardDataToCheck = new SimpleIResultData(0);
                    break;
                case CARDSTATES.WAITFORUSEWITHTARGET:
                    cardDataToCheck = new SimpleIResultData(0);
                    break;
                default:
                    cardDataToCheck = new SimpleIResultData(0);
                    break;
            }

            ResultDataValidator validator = new ResultDataValidator(comparationType, cardDataToCheck, rdToCheckAgainst);
            if (validator.IsValid() == false) return null;

            return cardTarget;
        }
    }
}