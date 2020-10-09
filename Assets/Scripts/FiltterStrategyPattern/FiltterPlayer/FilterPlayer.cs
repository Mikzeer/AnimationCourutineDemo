using PositionerDemo;

public class FilterPlayer : IFilterPlayerStrategy
{
    Player IFilterPlayerStrategy.FilterPlayer(IOcuppy occupier)
    {
        if (occupier != null && occupier.OccupierType == OCUPPIERTYPE.PLAYER)
        {
            return (Player)occupier;
            //Debug.Log("Estoy ocupada por un player " + selectedPlayer._ID);
        }

        return null;
    }
}



