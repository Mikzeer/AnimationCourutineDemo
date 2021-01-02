namespace PositionerDemo
{
    public class TargetHealtStatAmountFiltter : TargetStatAmountFiltter
    {
        private const int STAT_ID = 0;
        private const int FILTTER_ID = 13;
        public TargetHealtStatAmountFiltter(STATAMOUNTTYPE statAmountType, StatIResultData rDToCheckAgainst, COMPARATIONTYPE comparationType) : base(comparationType, FILTTER_ID)
        {
            rDToCheck = new StatIResultData(STAT_ID, statAmountType);
            this.rDToCheckAgainst = rDToCheckAgainst;
        }
    }

}