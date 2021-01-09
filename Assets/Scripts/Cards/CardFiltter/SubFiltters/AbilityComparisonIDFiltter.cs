namespace PositionerDemo
{
    public class AbilityComparisonIDFiltter
    {
        int abilityID;
        public AbilityComparisonIDFiltter(int abilityID)
        {
            this.abilityID = abilityID;
        }
        public virtual bool IsValidStat(IOcuppy ocuppy)
        {
            ABILITYTYPE abilityType = (ABILITYTYPE)abilityID;
            return ocuppy.Abilities.ContainsKey(abilityType);
        }
    }
}
