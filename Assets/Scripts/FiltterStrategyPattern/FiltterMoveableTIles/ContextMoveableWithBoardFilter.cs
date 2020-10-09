using PositionerDemo;
using System.Collections.Generic;

public class ContextMoveableWithBoardFilter
{
    private IFiltterMoveableTileWithBoardStrategy strategy;
    public ContextMoveableWithBoardFilter(IFiltterMoveableTileWithBoardStrategy strategy)
    {
        this.strategy = strategy;
    }

    public List<Tile> ExecuteStrategy(Tile from, MOVEDIRECTIONTYPE moveDirectionType, Tile[,] boardTiles, int movementAmount)
    {
        return strategy.FiltterMoveables(from, moveDirectionType, boardTiles, movementAmount);
    }
}



