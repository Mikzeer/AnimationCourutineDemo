namespace PositionerDemo
{
    public class TargetHealtStatAmountAgainstSimplFiltter : TargetStatAmountAgainstSimpleFiltter
    {
        private const int STAT_ID = 0;
        private const int FILTTER_ID = 18;
        public TargetHealtStatAmountAgainstSimplFiltter(STATAMOUNTTYPE statAmountType, int simpleAmount, COMPARATIONTYPE comparationType) : base(comparationType, FILTTER_ID)
        {
            rDToCheck = new StatIResultData(STAT_ID, statAmountType);
            rDToCheckAgainst = new SimpleIResultData(simpleAmount);
        }
    }
}