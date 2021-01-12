using PositionerDemo;
using System.Collections.Generic;

public class FilterMoveableAllDirectionWithBoard : IFiltterMoveableTileWithBoardStrategy
{
    bool blockDirection = true;
    bool blockUp = false;
    bool blockLeft = false;
    bool blockRight = false;
    bool blockDown = false;
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
            // RIGHT
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
            // LEFT
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



