using PositionerDemo;

public class ContextPlayerFilter
{
    private IFilterPlayerStrategy strategy;
    public ContextPlayerFilter(IFilterPlayerStrategy strategy)
    {
        this.strategy = strategy;
    }

    public Player ExecuteStrategy(IOcuppy occupier)
    {
        return strategy.FilterPlayer(occupier);
    }
}



