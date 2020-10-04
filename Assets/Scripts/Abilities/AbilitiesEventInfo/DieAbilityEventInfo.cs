namespace PositionerDemo
{
    public class DieAbilityEventInfo : AbilityEventInfo
    {
        public IOcuppy dieOccupy; // QUIEN SE MURIO ID
        public IOcuppy killer; // QUIEN LO MATO ID
        public DieAbilityEventInfo(IOcuppy killer, IOcuppy dieOccupy)
        {
            this.killer = killer;
            this.dieOccupy = dieOccupy;
        }
    }
}
