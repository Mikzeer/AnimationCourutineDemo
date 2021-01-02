namespace PositionerDemo
{
    public abstract class TargetAbilityIDFiltter : CardFiltter
    {
        AbilityComparisonIDFiltter abilityComparisonIDFiltter;
        public TargetAbilityIDFiltter(int abilityID, int ID) : base(ID)
        {
            abilityComparisonIDFiltter = new AbilityComparisonIDFiltter(abilityID);
        }

        public override ICardTarget CheckTarget(ICardTarget cardTarget)
        {
            IOcuppy ocuppier = cardTarget.GetOcuppy();
            if (ocuppier == null) return null;
            if (abilityComparisonIDFiltter.IsValidStat(ocuppier) == false) return null;
            return base.CheckTarget(cardTarget);
        }
    }
}