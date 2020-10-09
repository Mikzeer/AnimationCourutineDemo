using PositionerDemo;

public class ContextUnitFilter
{
    private IFilterUnitStrategy strategy;
    public ContextUnitFilter(IFilterUnitStrategy strategy)
    {
        this.strategy = strategy;
    }

    public Kimboko ExecuteStrategy(IOcuppy occupier)
    {
        return strategy.FilterUnitStrat(occupier);
    }
}



