using PositionerDemo;
using System.Collections.Generic;

public interface IFiltterMoveableTileWithBoardStrategy
{
    List<Tile> FiltterMoveables(Tile from, MOVEDIRECTIONTYPE moveDirectionType, Tile[,] boardTiles, int movementAmount);
}



