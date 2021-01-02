namespace PositionerDemo
{
    public class TargetMoveRangeStatAmountFiltter : TargetStatAmountFiltter
    {
        private const int STAT_ID = 1;
        private const int FILTTER_ID = 14;
        public TargetMoveRangeStatAmountFiltter(STATAMOUNTTYPE statAmountType, StatIResultData rDToCheckAgainst, COMPARATIONTYPE comparationType) : base(comparationType, FILTTER_ID)
        {
            rDToCheck = new StatIResultData(STAT_ID, statAmountType);
            this.rDToCheckAgainst = rDToCheckAgainst;
        }
    }

}