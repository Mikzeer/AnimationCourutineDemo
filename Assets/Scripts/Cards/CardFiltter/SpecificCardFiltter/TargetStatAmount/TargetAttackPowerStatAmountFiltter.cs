namespace PositionerDemo
{
    public class TargetAttackPowerStatAmountFiltter : TargetStatAmountFiltter
    {
        private const int STAT_ID = 3;
        private const int FILTTER_ID = 16;
        public TargetAttackPowerStatAmountFiltter(STATAMOUNTTYPE statAmountType, StatIResultData rDToCheckAgainst, COMPARATIONTYPE comparationType) : base(comparationType, FILTTER_ID)
        {
            rDToCheck = new StatIResultData(STAT_ID, statAmountType);
            this.rDToCheckAgainst = rDToCheckAgainst;
        }
    }

}