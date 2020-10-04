namespace PositionerDemo
{
    public class ActionPointStat : Stat
    {
        private const int _ID = 4;
        private const STATTYPE _StatType = STATTYPE.ACTIONPOINTS;

        public ActionPointStat(int actualStatValue, int maxStatValue) : base(actualStatValue, maxStatValue, _ID, _StatType)
        {
        }
    }
}
