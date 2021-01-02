namespace PositionerDemo
{
    public class TargetActionPointsStatAmountFiltter : TargetStatAmountFiltter
    {
        private const int STAT_ID = 4;
        private const int FILTTER_ID = 17;
        public TargetActionPointsStatAmountFiltter(STATAMOUNTTYPE statAmountType, StatIResultData rDToCheckAgainst, COMPARATIONTYPE comparationType) : base(comparationType, FILTTER_ID)
        {
            rDToCheck = new StatIResultData(STAT_ID, statAmountType);
            this.rDToCheckAgainst = rDToCheckAgainst;
        }
    }

}