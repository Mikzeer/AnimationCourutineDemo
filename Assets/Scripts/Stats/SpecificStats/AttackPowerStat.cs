namespace PositionerDemo
{
    public class AttackPowerStat : Stat
    {
        private const int _ID = 3;
        private const STATTYPE _StatType = STATTYPE.ATTACKPOW;

        public AttackPowerStat(int actualStatValue, int maxStatValue) : base(actualStatValue, maxStatValue, _ID, _StatType)
        {
        }
    }
}
