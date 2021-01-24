using PositionerDemo;

namespace MikzeerGame
{
    namespace UI
    {
        public class OcuppierStatInfo
        {
            public string statName { get; private set; }
            public string actualStat { get; private set; }
            public string maxStat { get; private set; }
            public OcuppierStatInfo(Stat stat)
            {
                statName = stat.StatType.ToString();
                actualStat = stat.ActualStatValue.ToString();
                maxStat = stat.MaxStatValue.ToString();
            }
        }
    }
}