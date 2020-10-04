namespace PositionerDemo
{
    public class AttackRangeStat : Stat
    {
        private const int _ID = 2;
        private const STATTYPE _StatType = STATTYPE.ATTACKRANGE;
        public AttackRangeStat(int actualStatValue, int maxStatValue) : base(actualStatValue, maxStatValue, _ID, _StatType)
        {
        }
    }
}
