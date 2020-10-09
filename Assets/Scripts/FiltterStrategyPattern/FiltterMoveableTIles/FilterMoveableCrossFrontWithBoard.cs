using PositionerDemo;
using System.Collections.Generic;

public class FilterMoveableCrossFrontWithBoard : IFiltterMoveableTileWithBoardStrategy
{
    bool blockDirection = true;
    bool blockUp = false;
    bool blockLeft = false;
    bool blockRight = false;
    bool blockDown = false;

    public List<Tile> FiltterMoveables(Tile from, MOVEDIRECTIONTYPE moveDirectionType, Tile[,] boardTiles, int movementAmount)
    {
        if (from == null) return null;
        if (movementAmount == 0) return null;

        List<Tile> moveableTiles = new List<Tile>();

        Position fromPosition = new Position(from.PosX, from.PosY);

        int maxX = boardTiles.GetLength(0);
        int maxY = boardTiles.GetLength(1);

        // posX + 1 = UP
        // posX - 1 = DOWN
        // posY + 1 = RIGHT
        // posY - 1 = LEFT

        for (int i = 1; i <= movementAmount; i++)
        {

            if (moveDirectionType == MOVEDIRECTIONTYPE.CROSSRIGHT)
            {
                if (blockDirection)
                {
                    if (!blockRight)
                    {
                        if (fromPosition.posY + i < maxY)
                        {
                            if (!boardTiles[fromPosition.posX, fromPosition.posY + i].IsOccupied())
                            {
                                moveableTiles.Add(boardTiles[fromPosition.posX, fromPosition.posY + i]);
                            }
                            else
                            {
                                blockRight = true;
                            }
                        }
                    }
                }
                else
                {
                    if (fromPosition.posY + i < maxY)
                    {
                        if (!boardTiles[fromPosition.posX, fromPosition.posY + i].IsOccupied())
                        {
                            moveableTiles.Add(boardTiles[fromPosition.posX, fromPosition.posY + i]);
                        }
                    }
                }

            }
            else if (moveDirectionType == MOVEDIRECTIONTYPE.CROSSLEFT)
            {
                if (blockDirection)
                {
                    if (!blockLeft)
                    {
                        if (fromPosition.posY - i >= 0)
                        {
                            if (!boardTiles[fromPosition.posX, fromPosition.posY - i].IsOccupied())
                            {
                                moveableTiles.Add(boardTiles[fromPosition.posX, fromPosition.posY - i]);
                            }
                            else
                            {
                                blockLeft = true;
                            }
                        }
                    }
                }
                else
                {
                    if (fromPosition.posY - i >= 0)
                    {
                        if (!boardTiles[fromPosition.posX, fromPosition.posY - i].IsOccupied())
                        {
                            moveableTiles.Add(boardTiles[fromPosition.posX, fromPosition.posY - i]);
                        }
                    }
                }

            }
            // UP
            if (blockDirection)
            {
                if (!blockUp)
                {
                    if (fromPosition.posX + i < maxX)
                    {
                        if (!boardTiles[fromPosition.posX + i, fromPosition.posY].IsOccupied())
                        {
                            moveableTiles.Add(boardTiles[fromPosition.posX + i, fromPosition.posY]);
                        }
                        else
                        {
                            blockUp = true;
                        }
                    }
                }
            }
            else
            {
                if (fromPosition.posX + i < maxX)
                {
                    if (!boardTiles[fromPosition.posX + i, fromPosition.posY].IsOccupied())
                    {
                        moveableTiles.Add(boardTiles[fromPosition.posX + i, fromPosition.posY]);
                    }
                }
            }

            // DOWN
            if (blockDirection)
            {
                if (!blockDown)
                {
                    if (fromPosition.posX - i >= 0)
                    {
                        if (!boardTiles[fromPosition.posX - i, fromPosition.posY].IsOccupied())
                        {
                            moveableTiles.Add(boardTiles[fromPosition.posX - i, fromPosition.posY]);
                        }
                        else
                        {
                            blockDown = true;
                        }
                    }
                }
            }
            else
            {
                if (fromPosition.posX - i >= 0)
                {
                    if (!boardTiles[fromPosition.posX - i, fromPosition.posY].IsOccupied())
                    {
                        moveableTiles.Add(boardTiles[fromPosition.posX - i, fromPosition.posY]);
                    }
                }
            }

        }
        return moveableTiles;
    }
}



