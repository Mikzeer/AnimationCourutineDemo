using UnityEngine;

namespace PositionerDemo
{
    public class UnitFoeCardFiltter : CardFiltter
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
                if (AnimotionHandler.Instance.GetPlayer() != kimboko.ownerPlayer)
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

    [CreateAssetMenu(fileName = "UnitFoeFiltter", menuName = "Cards/Filtter/ New Unit Foe Card Filtter")]
    public class UnitFoeCardFiltterScriptableObject : CardFiltterScriptableObject
    {
        public override ICardTarget CheckTarget(ICardTarget cardTarget)
        {
            Kimboko kimboko = (Kimboko)cardTarget;

            if (kimboko != null)
            {
                if (AnimotionHandler.Instance.GetPlayer() != kimboko.ownerPlayer)
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
