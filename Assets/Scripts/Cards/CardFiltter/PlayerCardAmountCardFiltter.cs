using UnityEngine;

namespace PositionerDemo
{
    public class PlayerCardAmountCardFiltter : CardFiltter
    {
        // CHEQUEAR QUE EL RIVAL/JUGADOR TENGA CARTAS TARGETTYPE BASENEXUS 
        // CheckCardAmount => PLAYER
        protected int amountToFind = 2;

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

    [CreateAssetMenu(fileName = "PlayerCardAmountFiltter", menuName = "Cards/Filtter/ New Player Card Amount Filtter")]
    public class PlayerCardAmountCardFiltterScriptableObject : CardFiltterScriptableObject
    {
        // CHEQUEAR QUE EL RIVAL/JUGADOR TENGA CARTAS TARGETTYPE BASENEXUS 
        // CheckCardAmount => PLAYER
        public int amountToFind = 2;

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
