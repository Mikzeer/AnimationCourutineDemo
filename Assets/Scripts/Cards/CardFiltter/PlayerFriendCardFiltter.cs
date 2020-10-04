using UnityEngine;

namespace PositionerDemo
{
    public class PlayerFriendCardFiltter : CardFiltter
    {
        public override ICardTarget CheckTarget(ICardTarget cardTarget)
        {
            if (cardTarget.CardTargetType != CARDTARGETTYPE.BASENEXO) return null;

            Player player = (Player)cardTarget;

            if (player != null)
            {
                if (AnimotionHandler.Instance.GetPlayer() == player)
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

    [CreateAssetMenu(fileName = "PlayerFriendFiltter", menuName = "Cards/Filtter/ New Player Friend Card Filtter")]
    public class PlayerFriendCardFiltterScriptableObject : CardFiltterScriptableObject
    {
        public override ICardTarget CheckTarget(ICardTarget cardTarget)
        {
            if (cardTarget.CardTargetType != CARDTARGETTYPE.BASENEXO) return null;

            Player player = (Player)cardTarget;

            if (player != null)
            {
                if (AnimotionHandler.Instance.GetPlayer() == player)
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
