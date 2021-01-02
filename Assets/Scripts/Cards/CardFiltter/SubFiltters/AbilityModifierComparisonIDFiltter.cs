namespace PositionerDemo
{
    public class AbilityModifierComparisonIDFiltter : AbilityComparisonIDFiltter
    {
        int abilityID;
        int modifierID;
        public AbilityModifierComparisonIDFiltter(int abilityID, int modifierID) : base(abilityID)
        {
            this.abilityID = abilityID;
            this.modifierID = modifierID;
        }
        public override bool IsValidStat(IOcuppy ocuppy)
        {
            if (base.IsValidStat(ocuppy))
            {
                if (ocuppy.Abilities[abilityID].IsModifierApply(modifierID)) return true;
            }
            return false;
        }
    }
}
