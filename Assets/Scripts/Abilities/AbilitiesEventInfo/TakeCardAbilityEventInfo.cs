namespace PositionerDemo
{
    public class TakeCardAbilityEventInfo : AbilityEventInfo
    {             
        public Card card { get; set; } // QUE CARD TOMO
        public Player cardTaker; // QUIEN TOMO LA CARD
        public int cardIndex;
        public TakeCardAbilityEventInfo(Player cardTaker, Card card, int cardIndex)
        {
            this.cardTaker = cardTaker;
            this.card = card;
            this.cardIndex = cardIndex;
        }
    }

    public class DiscardCardAbilityEventInfo : AbilityEventInfo
    {
        public Card card { get; set; } // QUE CARD TOMO
        public Player cardDiscarder; // QUIEN DESCARTO LA CARD // UNO PUEDE DESCARTAR UNA CARTA POR UN EFECTO SIN USARLA

        public DiscardCardAbilityEventInfo(Player cardDiscarder, Card card)
        {
            this.cardDiscarder = cardDiscarder;
            this.card = card;
        }
    }
}
