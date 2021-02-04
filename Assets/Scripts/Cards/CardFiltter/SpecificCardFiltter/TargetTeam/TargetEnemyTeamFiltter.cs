namespace PositionerDemo
{
    public class TargetEnemyTeamFiltter : TargetTeamFiltter
    {
        private const bool IS_ALLY = true;
        private const int FILTTER_ID = 4;
        public TargetEnemyTeamFiltter() : base(IS_ALLY, FILTTER_ID)
        {
        }
    }
}