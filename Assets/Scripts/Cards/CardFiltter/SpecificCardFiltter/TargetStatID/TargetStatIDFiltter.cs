namespace PositionerDemo
{
    public abstract class TargetStatIDFiltter : CardFiltter
    {
        StatComparisonIDFiltter statComparisonIDFiltter;
        int statID;
        public TargetStatIDFiltter(int statID, int ID) : base(ID)
        {
            statComparisonIDFiltter = new StatComparisonIDFiltter(statID);
            this.statID = statID;
        }

        public override ICardTarget CheckTarget(ICardTarget cardTarget)
        {
            IOcuppy ocuppier = cardTarget.GetOcuppy();
            if (ocuppier == null) return null;
            if (statComparisonIDFiltter.IsValidStat(ocuppier) == false) return null;
            return base.CheckTarget(cardTarget);
        }
    }
}