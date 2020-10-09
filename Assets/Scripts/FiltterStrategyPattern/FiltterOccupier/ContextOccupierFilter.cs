using PositionerDemo;

public class ContextOccupierFilter
{
    private IFiltterOccupierStrategy strategy;
    public ContextOccupierFilter(IFiltterOccupierStrategy strategy)
    {
        this.strategy = strategy;
    }

    public IOcuppy ExecuteStrategy(Tile tile2D)
    {
        return strategy.FilterIOcuppy(tile2D);
    }
}



