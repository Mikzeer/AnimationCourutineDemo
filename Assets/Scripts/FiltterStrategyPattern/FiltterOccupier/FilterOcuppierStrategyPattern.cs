using PositionerDemo;

public class FilterOcuppierStrategyPattern
{
    Tile til2D;
    public IOcuppy occupier;
    public FilterOcuppierStrategyPattern(Tile til2D)
    {
        this.til2D = til2D;
        ContextOccupierFilter ctxOccupy = new ContextOccupierFilter(new FilterOccupier());
        occupier = ctxOccupy.ExecuteStrategy(til2D);
    }
}



