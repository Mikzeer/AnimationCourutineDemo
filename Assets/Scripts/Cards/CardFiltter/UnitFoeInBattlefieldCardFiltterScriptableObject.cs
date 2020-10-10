using UnityEngine;

namespace PositionerDemo
{
    [CreateAssetMenu(fileName = "UnitFoeInBattlefieldFiltter", menuName = "Cards/Filtter/ New Unit Foe In Battlefield Card Filtter")]
    public class UnitFoeInBattlefieldCardFiltterScriptableObject : CardFiltterScriptableObject
    {
        public override ICardTarget CheckTarget(ICardTarget cardTarget)
        {
            Kimboko kimboko = (Kimboko)cardTarget;

            if (kimboko != null)
            {
                if (AnimotionHandler.Instance.GetPlayer() != kimboko.ownerPlayer)
                {
                    // AHORA DEBERIA VER COMO CHOTA ME FIJO DONDE ESTA 
                    Vector3 position = kimboko.GetTransform().position;

                    Tile tile = GameCreator.Instance.board2D.GetGridObject(position);

                    if (tile != null)
                    {
                        if (tile.tileType == TILETYPE.BATTLEFILED)
                        {
                            return cardTarget;
                        }
                    }
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
