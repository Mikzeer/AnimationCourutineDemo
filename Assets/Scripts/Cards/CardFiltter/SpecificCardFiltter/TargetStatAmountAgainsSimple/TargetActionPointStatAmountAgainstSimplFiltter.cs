namespace PositionerDemo
{
    public class TargetActionPointStatAmountAgainstSimplFiltter : TargetStatAmountAgainstSimpleFiltter
    {
        private const int STAT_ID = 4;
        private const int FILTTER_ID = 22;
        public TargetActionPointStatAmountAgainstSimplFiltter(STATAMOUNTTYPE statAmountType, int simpleAmount, COMPARATIONTYPE comparationType) : base(comparationType, FILTTER_ID)
        {
            rDToCheck = new StatIResultData(STAT_ID, statAmountType);
            rDToCheckAgainst = new SimpleIResultData(simpleAmount);
        }
    }
}