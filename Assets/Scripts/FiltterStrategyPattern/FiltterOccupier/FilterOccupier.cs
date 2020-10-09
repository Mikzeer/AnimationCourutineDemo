using PositionerDemo;

public class FilterOccupier : IFiltterOccupierStrategy
{
    public IOcuppy FilterIOcuppy(Tile tile2D)
    {
        if (tile2D == null) return null;

        if (tile2D.IsOccupied())
        {
            return tile2D.GetOccupier();
        }
        return null;
    }
}



