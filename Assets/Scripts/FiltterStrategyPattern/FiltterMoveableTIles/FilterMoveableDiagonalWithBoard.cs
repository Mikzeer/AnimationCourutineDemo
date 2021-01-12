using PositionerDemo;
using System.Collections.Generic;

public class FilterMoveableDiagonalWithBoard : IFiltterMoveableTileWithBoardStrategy
{
    bool blockDirection = true;
    bool blockUpRight = false;
    bool blockUpLeft = false;
    bool blockDownRight = false;
    bool blockDownLeft = false;

    public List<Tile> FiltterMoveables(Tile from, MOVEDIRECTIONTYPE moveDirectionType, Tile[,] boardTiles, int movementAmount)
    {
        if (from == null) return null;
        if (movementAmount == 0) return null;

        List<Tile> moveableTiles = new List<Tile>();

        PositionerDemo.Position fromPosition = new PositionerDemo.Position(from.position.posX, from.position.posY);

        int maxX = boardTiles.GetLength(0);
        int maxY = boardTiles.GetLength(1);

        for (int i = 1; i <= movementAmount; i++)
        {
            // UP RIGHT
            if (blockDirection)
            {
                if (!blockUpRight)
                {
                    if (fromPosition.posY + i < maxY && fromPosition.posX + i < maxX)
                    {
                        if (!boardTiles[fromPosition.posX + i, fromPosition.posY + i].IsOccupied())
                        {
                            moveableTiles.Add(boardTiles[fromPosition.posX + i, fromPosition.posY + i]);
                        }
                        else
                        {
                            blockUpRight = true;
                        }
                    }
                }
            }
            else
            {
                if (fromPosition.posY + i < maxY && fromPosition.posX + i < maxX)
                {
                    if (!boardTiles[fromPosition.posX + i, fromPosition.posY + i].IsOccupied())
                    {
                        moveableTiles.Add(boardTiles[fromPosition.posX + i, fromPosition.posY + i]);
                    }
                }
            }

            // UP LEFT
            if (blockDirection)
            {
                if (!blockUpLeft)
                {
                    if (fromPosition.posY + i < maxY && fromPosition.posX - i >= 0)
                    {
                        if (!boardTiles[fromPosition.posX - i, fromPosition.posY + i].IsOccupied())
                        {
                            moveableTiles.Add(boardTiles[fromPosition.posX - i, fromPosition.posY + i]);
                        }
                        else
                        {
                            blockUpLeft = true;
                        }
                    }
                }
            }
            else
            {
                if (fromPosition.posY + i < maxY && fromPosition.posX - i >= 0)
                {
                    if (!boardTiles[fromPosition.posX - i, fromPosition.posY + i].IsOccupied())
                    {
                        moveableTiles.Add(boardTiles[fromPosition.posX - i, fromPosition.posY + i]);
                    }
                }
            }

            // DOWN RIGHT
            if (blockDirection)
            {
                if (!blockDownRight)
                {
                    if (fromPosition.posY - i >= 0 && fromPosition.posX + i < maxX)
                    {
                        if (!boardTiles[fromPosition.posX + i, fromPosition.posY - i].IsOccupied())
                        {
                            moveableTiles.Add(boardTiles[fromPosition.posX + i, fromPosition.posY - i]);
                        }
                        else
                        {
                            blockDownRight = true;
                        }
                    }
                }
            }
            else
            {
                if (fromPosition.posY - i >= 0 && fromPosition.posX + i < maxX)
                {
                    if (!boardTiles[fromPosition.posX + i, fromPosition.posY - i].IsOccupied())
                    {
                        moveableTiles.Add(boardTiles[fromPosition.posX + i, fromPosition.posY - i]);
                    }
                }
            }

            // DOWN LEFT
            if (blockDirection)
            {
                if (!blockDownLeft)
                {
                    if (fromPosition.posY - i >= 0 && fromPosition.posX - i >= 0)
                    {
                        if (!boardTiles[fromPosition.posX - i, fromPosition.posY - i].IsOccupied())
                        {
                            moveableTiles.Add(boardTiles[fromPosition.posX - i, fromPosition.posY - i]);
                        }
                        else
                        {
                            blockDownLeft = true;
                        }
                    }
                }
            }
            else
            {
                if (fromPosition.posY - i >= 0 && fromPosition.posX - i >= 0)
                {
                    if (!boardTiles[fromPosition.posX - i, fromPosition.posY - i].IsOccupied())
                    {
                        moveableTiles.Add(boardTiles[fromPosition.posX - i, fromPosition.posY - i]);
                    }
                }
            }

        }
        return moveableTiles;
    }
}



