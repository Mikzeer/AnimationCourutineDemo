namespace PositionerDemo
{
    public class TargetMoveRangeStatAmountAgainstSimplFiltter : TargetStatAmountAgainstSimpleFiltter
    {
        private const int STAT_ID = 1;
        private const int FILTTER_ID = 19;
        public TargetMoveRangeStatAmountAgainstSimplFiltter(STATAMOUNTTYPE statAmountType, int simpleAmount, COMPARATIONTYPE comparationType) : base(comparationType, FILTTER_ID)
        {
            rDToCheck = new StatIResultData(STAT_ID, statAmountType);
            rDToCheckAgainst = new SimpleIResultData(simpleAmount);
        }
    }
}