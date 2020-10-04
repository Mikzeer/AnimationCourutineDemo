namespace PositionerDemo
{
    public class TakeCardAbilityEventInfo : AbilityEventInfo
    {             
        public Card card { get; set; } // QUE CARD TOMO
        public Player cardTaker; // QUIEN TOMO LA CARD

        public TakeCardAbilityEventInfo(Player cardTaker, Card card)
        {
            this.cardTaker = cardTaker;
            this.card = card;
        }
    }
}
