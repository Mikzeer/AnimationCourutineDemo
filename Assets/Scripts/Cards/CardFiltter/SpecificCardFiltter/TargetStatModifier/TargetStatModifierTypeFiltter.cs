namespace PositionerDemo
{
    public class TargetStatModifierTypeFiltter : CardFiltter
    {
        private const int ABILITY_ID = 42;
        StatModifierComparisonTypeFiltter statModifierComparisonTypeFiltter;
        public TargetStatModifierTypeFiltter(STATMODIFIERTYPE statModifierType) : base(ABILITY_ID)
        {
            statModifierComparisonTypeFiltter = new StatModifierComparisonTypeFiltter(statModifierType);
        }

        public override ICardTarget CheckTarget(ICardTarget cardTarget)
        {
            IOcuppy ocuppier = cardTarget.GetOcuppy();
            if (ocuppier == null) return null;
            if (statModifierComparisonTypeFiltter.IsValidStatModifierType(ocuppier) == false) return null;
            return base.CheckTarget(cardTarget);
        }
    }
}