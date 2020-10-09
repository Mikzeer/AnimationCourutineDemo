using PositionerDemo;
using System.Collections.Generic;

public class FilterMoveableWithBoardStrategyPattern
{
    public List<Tile> moveableTiles;

    public FilterMoveableWithBoardStrategyPattern(Tile from, MOVEDIRECTIONTYPE moveDirectionType, Tile[,] boardTiles, int movementAmount)
    {
        IFiltterMoveableTileWithBoardStrategy moveStratToUse = null;
        switch (moveDirectionType)
        {
            case MOVEDIRECTIONTYPE.CROSSLEFT:
            case MOVEDIRECTIONTYPE.CROSSRIGHT:
                moveStratToUse = new FilterMoveableCrossFrontWithBoard();
                break;
            case MOVEDIRECTIONTYPE.DIAGONAL:
                moveStratToUse = new FilterMoveableDiagonalWithBoard();
                break;
            case MOVEDIRECTIONTYPE.MULTI:
                moveStratToUse = new FilterMoveableAllDirectionWithBoard();
                break;
        }

        if (moveStratToUse == null)
        {
            moveableTiles = null;
            return;
        }

        ContextMoveableWithBoardFilter ctxMoveable = new ContextMoveableWithBoardFilter(moveStratToUse);
        moveableTiles = ctxMoveable.ExecuteStrategy(from, moveDirectionType, boardTiles, movementAmount);
    }
}



