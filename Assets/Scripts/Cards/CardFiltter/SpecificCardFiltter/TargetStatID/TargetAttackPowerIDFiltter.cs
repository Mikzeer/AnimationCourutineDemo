namespace PositionerDemo
{
    public class TargetAttackPowerIDFiltter : TargetStatIDFiltter
    {
        private const int STAT_ID = 3;
        private const int FILTTER_ID = 11;
        public TargetAttackPowerIDFiltter() : base(STAT_ID, FILTTER_ID)
        {
        }
    }
}