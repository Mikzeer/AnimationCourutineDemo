namespace PositionerDemo
{
    public class TargetMoveRangeStatIDFiltter : TargetStatIDFiltter
    {
        private const int STAT_ID = 1;
        private const int FILTTER_ID = 9;
        public TargetMoveRangeStatIDFiltter() : base(STAT_ID, FILTTER_ID)
        {
        }
    }
}