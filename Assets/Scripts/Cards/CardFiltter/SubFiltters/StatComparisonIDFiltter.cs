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
            return ocuppy.Stats.ContainsKey(statID);
        }
    }
}
