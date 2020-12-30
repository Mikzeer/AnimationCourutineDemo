namespace PositionerDemo
{
    public class EvolveAbilityEventInfo : AbilityEventInfo
    {
        public Kimboko evolver; // QUIEN SE VA A COMBINAR
        public UNITTYPE startType; // QUE TIPO ERA
        public UNITTYPE evolvedType; // A QUE TIPO EVOLUCIONO
        public EvolveAbilityEventInfo(Kimboko evolver, UNITTYPE startType, UNITTYPE evolvedType)
        {
            this.evolver = evolver;
            this.evolvedType = evolvedType;
            this.startType = startType;
        }
    }
}
