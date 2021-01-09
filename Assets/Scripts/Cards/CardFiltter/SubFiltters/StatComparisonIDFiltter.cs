namespace PositionerDemo
{
    public class StatComparisonIDFiltter
    {
        int statID;
        public StatComparisonIDFiltter(int statID)
        {
            this.statID = statID;
        }
        public bool IsValidStat(IOcuppy ocuppy)
        {
            STATTYPE statType = (STATTYPE)statID;
            return ocuppy.Stats.ContainsKey(statType);
        }
    }
}
