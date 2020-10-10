using UnityEngine;

namespace PositionerDemo
{
    [CreateAssetMenu(fileName = "UnitFriendFiltter", menuName = "Cards/Filtter/ New Unit Friend Card Filtter")]
    public class UnitFriendCardFiltterScriptableObject : CardFiltterScriptableObject
    {
        public override ICardTarget CheckTarget(ICardTarget cardTarget)
        {
            if (cardTarget.CardTargetType != CARDTARGETTYPE.UNIT)
            {
                return null;
            }

            Kimboko kimboko = (Kimboko)cardTarget;

            if (kimboko != null)
            {
                if (GameCreator.Instance.turnManager.GetActualPlayerTurn() == kimboko.ownerPlayer)
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
