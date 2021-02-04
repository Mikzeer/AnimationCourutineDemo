namespace PositionerDemo
{
    public class TargetAllyTeamFiltter : TargetTeamFiltter
    {
        private const bool IS_ALLY = true;
        private const int FILTTER_ID = 3;
        public TargetAllyTeamFiltter() : base(IS_ALLY, FILTTER_ID)
        {
        }
    }
}