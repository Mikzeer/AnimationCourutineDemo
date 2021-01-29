using System.Collections.Generic;

namespace PositionerDemo
{
    public class CardFiltterManager
    {
        public List<ICardTarget> FilterTargets(List<ICardTarget> cardTargets, Card card)
        {
            List<ICardTarget> filtterTargets = new List<ICardTarget>();

            // RECORREMOS LOS TARGETS POSIBLES
            for (int i = 0; i < cardTargets.Count; i++)
            {
                bool allPass = true;

                // APLICAMOS CADA FILTRO QUE TENGA LA CARTA
                for (int x = 0; x < card.CardData.cardTargetFiltters.Count; x++)
                {
                    ICardTarget cardTarget = card.CardData.cardTargetFiltters[x].CheckTarget(cardTargets[i]);

                    if (cardTarget == null)
                    {
                        allPass = false;
                    }
                }

                if (allPass)
                {
                    if (filtterTargets.Contains(cardTargets[i]) == false)
                    {
                        filtterTargets.Add(cardTargets[i]);
                    }
                }
            }

            return filtterTargets;
        }
    }
}
