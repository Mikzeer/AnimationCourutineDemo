using UnityEngine;

namespace PositionerDemo
{
    public class UnitFoeInBattlefieldCardFiltter : CardFiltter
    {
        private const int FILTTER_ID = 5;

        public UnitFoeInBattlefieldCardFiltter() : base(FILTTER_ID)
        {
        }

        public override ICardTarget CheckTarget(ICardTarget cardTarget)
        {
            if (cardTarget.CardTargetType != CARDTARGETTYPE.UNIT)
            {
                return null;
            }

            Kimboko kimboko = (Kimboko)cardTarget;

            if (kimboko != null)
            {
                if (GameCreator.Instance.turnManager.GetActualPlayerTurn() != kimboko.ownerPlayer)
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


