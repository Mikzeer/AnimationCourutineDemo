namespace PositionerDemo
{
    public class TargetAttackRangeIDFiltter : TargetStatIDFiltter
    {
        private const int STAT_ID = 2;
        private const int FILTTER_ID = 10;
        public TargetAttackRangeIDFiltter() : base(STAT_ID, FILTTER_ID)
        {
        }
    }
}