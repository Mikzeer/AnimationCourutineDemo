namespace PositionerDemo
{
    public class TargetAttackRangeStatAmountFiltter : TargetStatAmountFiltter
    {
        private const int STAT_ID = 2;
        private const int FILTTER_ID = 15;
        public TargetAttackRangeStatAmountFiltter(STATAMOUNTTYPE statAmountType, StatIResultData rDToCheckAgainst, COMPARATIONTYPE comparationType) : base(comparationType, FILTTER_ID)
        {
            rDToCheck = new StatIResultData(STAT_ID, statAmountType);
            this.rDToCheckAgainst = rDToCheckAgainst;
        }
    }

}