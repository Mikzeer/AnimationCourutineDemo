namespace PositionerDemo
{
    public class MoveRangeStat : Stat
    {
        private const int _ID = 1;
        private const STATTYPE _StatType = STATTYPE.MOVERANGE;
        public MoveRangeStat(int actualStatValue, int maxStatValue) : base(actualStatValue, maxStatValue, _ID, _StatType)
        {
        }
    }
}
