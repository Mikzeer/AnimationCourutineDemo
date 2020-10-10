using UnityEngine;

namespace PositionerDemo
{
    [CreateAssetMenu(fileName = "PlayerFoeFiltter", menuName = "Cards/Filtter/ New Player Foe Card Filtter")]
    public class PlayerFoeCardFiltterScriptableObject : CardFiltterScriptableObject
    {
        public override ICardTarget CheckTarget(ICardTarget cardTarget)
        {
            if (cardTarget.CardTargetType != CARDTARGETTYPE.BASENEXO) return null;

            Player player = (Player)cardTarget;

            if (player != null)
            {
                if (GameCreator.Instance.turnManager.GetActualPlayerTurn() != player)
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
