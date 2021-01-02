namespace PositionerDemo
{
    public class TargetHealthStatIDFiltter : TargetStatIDFiltter
    {
        private const int STAT_ID = 0;
        private const int FILTTER_ID = 8;
        public TargetHealthStatIDFiltter() : base(STAT_ID, FILTTER_ID)
        {
        }
    }
}