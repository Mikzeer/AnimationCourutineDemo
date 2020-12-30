using System.Collections.Generic;

namespace PositionerDemo
{
    public class UseCardAbilityEventInfo : AbilityEventInfo
    {
        public Card card { get; set; } // QUE CARD TOMO
        public Player cardUser; // QUIEN TOMO LA CARD
        public List<ICardTarget> cardTarget; // QUIENES SON LOS TARGETS

        public UseCardAbilityEventInfo(Player cardUser, Card card, List<ICardTarget> cardTarget)
        {
            this.cardUser = cardUser;
            this.card = card;
            this.cardTarget = cardTarget;
        }
    }
}
