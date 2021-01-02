namespace PositionerDemo
{
    public class TargetAbilityModifierIDFiltter : CardFiltter
    {
        private const int ABILITY_ID = 41;
        AbilityModifierComparisonIDFiltter abilityModifierComparisonIDFiltter;
        public TargetAbilityModifierIDFiltter(int abilityID, int modifierID) : base(ABILITY_ID)
        {
            abilityModifierComparisonIDFiltter = new AbilityModifierComparisonIDFiltter(abilityID, modifierID);
        }

        public override ICardTarget CheckTarget(ICardTarget cardTarget)
        {
            IOcuppy ocuppier = cardTarget.GetOcuppy();
            if (ocuppier == null) return null;
            if (abilityModifierComparisonIDFiltter.IsValidStat(ocuppier) == false) return null;
            return base.CheckTarget(cardTarget);
        }
    }
}