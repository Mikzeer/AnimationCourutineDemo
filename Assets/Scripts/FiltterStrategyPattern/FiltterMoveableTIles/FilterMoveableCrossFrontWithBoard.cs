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
        PositionerDemo.Position fromPosition = new PositionerDemo.Position(from.position.posX, from.position.posY);

        int maxX = boardTiles.GetLength(0);
        int maxY = boardTiles.GetLength(1);
        //            GridArray = new Tile[this.columnsWidht, this.rowsHeight];
        //              this.columnsWidht = columnsWidht + 4;   11 columnas
        //           this.rowsHeight = rowsHeight;    5 filas
        //
        // maxX = 11;   x + 1 == right  x - 1 == left
        // maxY = 5;     y + 1 == up   y - 1 == down

        for (int i = 1; i <= movementAmount; i++)
        {

            if (moveDirectionType == MOVEDIRECTIONTYPE.CROSSRIGHT)
            {
                if (blockDirection)
                {
                    if (!blockRight)
                    {
                        if (fromPosition.posX + i < maxX)
                        {
                            if (!boardTiles[fromPosition.posX + i, fromPosition.posY].IsOccupied())
                            {
                                moveableTiles.Add(boardTiles[fromPosition.posX + i, fromPosition.posY]);
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
                    if (fromPosition.posX + i < maxX)
                    {
                        if (!boardTiles[fromPosition.posX + i, fromPosition.posY].IsOccupied())
                        {
                            moveableTiles.Add(boardTiles[fromPosition.posX + i, fromPosition.posY]);
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
                        if (fromPosition.posX - i >= 0)
                        {
                            if (!boardTiles[fromPosition.posX - i, fromPosition.posY].IsOccupied())
                            {
                                moveableTiles.Add(boardTiles[fromPosition.posX - i, fromPosition.posY]);
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
                    if (fromPosition.posX - i >= 0)
                    {
                        if (!boardTiles[fromPosition.posX - i, fromPosition.posY].IsOccupied())
                        {
                            moveableTiles.Add(boardTiles[fromPosition.posX - i, fromPosition.posY]);
                        }
                    }
                }

            }
            // UP
            if (blockDirection)
            {
                if (!blockUp)
                {
                    if (fromPosition.posY + i < maxY)
                    {
                        if (!boardTiles[fromPosition.posX, fromPosition.posY + i].IsOccupied())
                        {
                            moveableTiles.Add(boardTiles[fromPosition.posX, fromPosition.posY + i]);
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
                if (fromPosition.posY + i < maxY)
                {
                    if (!boardTiles[fromPosition.posX, fromPosition.posY + i].IsOccupied())
                    {
                        moveableTiles.Add(boardTiles[fromPosition.posX, fromPosition.posY + i]);
                    }
                }
            }

            // DOWN
            if (blockDirection)
            {
                if (!blockDown)
                {
                    if (fromPosition.posY - i >= 0)
                    {
                        if (!boardTiles[fromPosition.posX, fromPosition.posY - i].IsOccupied())
                        {
                            moveableTiles.Add(boardTiles[fromPosition.posX, fromPosition.posY - i]);
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
                if (fromPosition.posY - i >= 0)
                {
                    if (!boardTiles[fromPosition.posX, fromPosition.posY - i].IsOccupied())
                    {
                        moveableTiles.Add(boardTiles[fromPosition.posX, fromPosition.posY - i]);
                    }
                }
            }

        }
        return moveableTiles;
    }
}



