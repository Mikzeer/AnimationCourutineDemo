namespace PositionerDemo
{
    public class TargetAttackRangeStatAmountAgainstSimplFiltter : TargetStatAmountAgainstSimpleFiltter
    {
        private const int STAT_ID = 2;
        private const int FILTTER_ID = 20;
        public TargetAttackRangeStatAmountAgainstSimplFiltter(STATAMOUNTTYPE statAmountType, int simpleAmount, COMPARATIONTYPE comparationType) : base(comparationType, FILTTER_ID)
        {
            rDToCheck = new StatIResultData(STAT_ID, statAmountType);
            rDToCheckAgainst = new SimpleIResultData(simpleAmount);
        }
    }
}