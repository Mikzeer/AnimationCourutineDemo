using System.Collections.Generic;

namespace PositionerDemo
{
    public class CardTargetManager
    {
        List<Tile> boardTiles;
        public CardTargetManager(Board2DManager board2D)
        {
            boardTiles = new List<Tile>();
            // RECORREMOS LA LISTA DE TILES
            for (int x = 0; x < board2D.GridArray.GetLength(0); x++)
            {
                for (int y = 0; y < board2D.GridArray.GetLength(1); y++)
                {
                    Tile actualTile = board2D.GetGridObject(x, y);
                    boardTiles.Add(actualTile);
                }
            }
        }

        public List<ICardTarget> GetPosibleTargets(Card card)
        {
            List<ICardTarget> foundTargets = new List<ICardTarget>();

            // RECORREMOS LA LISTA DE POSIBLES TARGET DE LA CARD
            for (int i = 0; i < card.CardData.cardTargetTypes.Count; i++)
            {
                // RECORREMOS LA LISTA DE TILES
                for (int z = 0; z < boardTiles.Count; z++)
                {
                    if (boardTiles[z].IsOccupied())
                    {
                        if (boardTiles[z].GetOcuppy().CardTargetType == card.CardData.cardTargetTypes[i])
                        {
                            if (foundTargets.Contains(boardTiles[z].GetOcuppy()) == false)
                            {
                                foundTargets.Add(boardTiles[z].GetOcuppy());
                            }
                        }
                    }
                    else
                    {
                        if (boardTiles[z].CardTargetType == card.CardData.cardTargetTypes[i])
                        {
                            if (foundTargets.Contains(boardTiles[z]) == false)
                            {
                                foundTargets.Add(boardTiles[z]);
                            }
                        }
                    }
                }
            }
            return foundTargets;
        }
    }
}
