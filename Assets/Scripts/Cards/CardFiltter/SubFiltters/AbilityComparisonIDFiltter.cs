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
            return ocuppy.Abilities.ContainsKey(abilityID);
        }
    }
}
