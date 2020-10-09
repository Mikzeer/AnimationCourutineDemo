using PositionerDemo;

public class FilterPlayerStrategyPattern
{
    IOcuppy occupier;
    public Player player;
    public FilterPlayerStrategyPattern(IOcuppy occupier)
    {
        this.occupier = occupier;
        ContextPlayerFilter ctxPlayer = new ContextPlayerFilter(new FilterPlayer());
        player = ctxPlayer.ExecuteStrategy(occupier);
    }
}



