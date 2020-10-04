using UnityEngine;

namespace PositionerDemo
{
    public class PlayerFoeCardFiltter : CardFiltter
    {
        public override ICardTarget CheckTarget(ICardTarget cardTarget)
        {
            if (cardTarget.CardTargetType != CARDTARGETTYPE.BASENEXO) return null;

            Player player = (Player)cardTarget;

            if (player != null)
            {
                if (AnimotionHandler.Instance.GetPlayer() != player)
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

    [CreateAssetMenu(fileName = "PlayerFoeFiltter", menuName = "Cards/Filtter/ New Player Foe Card Filtter")]
    public class PlayerFoeCardFiltterScriptableObject : CardFiltterScriptableObject
    {
        public override ICardTarget CheckTarget(ICardTarget cardTarget)
        {
            if (cardTarget.CardTargetType != CARDTARGETTYPE.BASENEXO) return null;

            Player player = (Player)cardTarget;

            if (player != null)
            {
                if (AnimotionHandler.Instance.GetPlayer() != player)
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
