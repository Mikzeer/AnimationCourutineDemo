using System.Collections.Generic;

namespace PositionerDemo
{
    public class CardEffectManager
    {
        public void ApplyCardEfect(List<ICardTarget> cardTargets, Card card)
        {
            for (int i = 0; i < cardTargets.Count; i++)
            {
                for (int j = 0; j < card.CardData.cardEffects.Count; j++)
                {
                    card.CardData.cardEffects[j].OnCardEffectApply(cardTargets[i]);
                }
            }
        }
    }
}
