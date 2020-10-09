using PositionerDemo;

public class FilterUnit : IFilterUnitStrategy
{
    public Kimboko FilterUnitStrat(IOcuppy occupier)
    {
        if (occupier != null && occupier.OccupierType == OCUPPIERTYPE.UNIT)
        {
            return (Kimboko)occupier;
            //Debug.Log("Estoy ocupada por un player " + selectedPlayer._ID);
        }

        return null;
    }
}



