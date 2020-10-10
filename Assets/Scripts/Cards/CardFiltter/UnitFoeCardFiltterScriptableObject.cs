using UnityEngine;

namespace PositionerDemo
{
    [CreateAssetMenu(fileName = "UnitFoeFiltter", menuName = "Cards/Filtter/ New Unit Foe Card Filtter")]
    public class UnitFoeCardFiltterScriptableObject : CardFiltterScriptableObject
    {
        public override ICardTarget CheckTarget(ICardTarget cardTarget)
        {
            Kimboko kimboko = (Kimboko)cardTarget;

            if (kimboko != null)
            {
                if (GameCreator.Instance.turnManager.GetActualPlayerTurn() != kimboko.ownerPlayer)
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
