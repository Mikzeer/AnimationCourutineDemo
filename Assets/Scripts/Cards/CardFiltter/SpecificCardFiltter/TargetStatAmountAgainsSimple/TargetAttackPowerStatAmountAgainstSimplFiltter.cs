namespace PositionerDemo
{
    public class TargetAttackPowerStatAmountAgainstSimplFiltter : TargetStatAmountAgainstSimpleFiltter
    {
        private const int STAT_ID = 3;
        private const int FILTTER_ID = 21;
        public TargetAttackPowerStatAmountAgainstSimplFiltter(STATAMOUNTTYPE statAmountType, int simpleAmount, COMPARATIONTYPE comparationType) : base(comparationType, FILTTER_ID)
        {
            rDToCheck = new StatIResultData(STAT_ID, statAmountType);
            rDToCheckAgainst = new SimpleIResultData(simpleAmount);
        }
    }
}