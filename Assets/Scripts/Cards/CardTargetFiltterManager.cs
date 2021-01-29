using System.Collections.Generic;

namespace PositionerDemo
{
    public class CardTargetFiltterManager
    {
        TurnController turnController;
        CardTargetManager cardTargetManager;
        CardFiltterManager cardFiltterManager;

        public CardTargetFiltterManager(TurnController turnController, Board2DManager board2D)
        {
            this.turnController = turnController;
            cardTargetManager = new CardTargetManager(board2D);
            cardFiltterManager = new CardFiltterManager();
        }

        public List<ICardTarget> OnTryGetFiltterTargets(Card card)
        {
            // CHEQUEAR DE QUIEN ES EL TURNO
            // SI NO ES NUESTRO TURNO ENTONCES SOLO MOVEMOS LA CARTA PARA ACOMODARLA ASI QUE ACA NO PASA NADA
            if (card.ownerPlayer != turnController.CurrentPlayerTurn) return null;

            List<ICardTarget> foundTargets = new List<ICardTarget>();

            foundTargets = cardTargetManager.GetPosibleTargets(card);

            // SI LA LISTA ES IGUAL A 0 SOLO MOVEMOS LA CARTA PARA ACOMODARLA ASI QUE ACA NO PASA NADA
            if (foundTargets.Count == 0) return null;

            foundTargets = cardFiltterManager.FilterTargets(foundTargets, card);

            // SI LA LISTA ES IGUAL A 0 SOLO MOVEMOS LA CARTA PARA ACOMODARLA ASI QUE ACA NO PASA NADA
            if (foundTargets.Count == 0) return null;

            return foundTargets;
        }
    }
}
