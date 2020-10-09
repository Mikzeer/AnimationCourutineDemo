using PositionerDemo;

public class FilterUnitStrategyPattern
{
    IOcuppy occupier;
    public Kimboko unit;
    public FilterUnitStrategyPattern(IOcuppy occupier)
    {
        this.occupier = occupier;
        ContextUnitFilter ctxUnit = new ContextUnitFilter(new FilterUnit());
        unit = ctxUnit.ExecuteStrategy(occupier);
    }
}



