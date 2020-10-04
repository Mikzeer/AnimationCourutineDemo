using UnityEngine;

namespace PositionerDemo
{
    public class UnitFoeInBattlefieldCardFiltter : CardFiltter
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
                    // AHORA DEBERIA VER COMO CHOTA ME FIJO DONDE ESTA 
                    Vector3 position = kimboko.GetTransform().position;

                    Tile tile = AnimotionHandler.Instance.GetBoard().GetGridObject(position);

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

                    Tile tile = AnimotionHandler.Instance.GetBoard().GetGridObject(position);

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
