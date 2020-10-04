namespace PositionerDemo
{
    public class HealthStat : Stat
    {
        private const int _ID = 0;
        private const STATTYPE _StatType = STATTYPE.HEALTH;

        public HealthStat(int actualStatValue, int maxStatValue) : base(actualStatValue, maxStatValue, _ID, _StatType)
        {
        }
    }
}
