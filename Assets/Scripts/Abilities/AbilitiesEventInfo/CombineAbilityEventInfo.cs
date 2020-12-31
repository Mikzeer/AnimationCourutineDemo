using System.Collections.Generic;

namespace PositionerDemo
{
    public class CombineAbilityEventInfo : AbilityEventInfo
    {
        public Kimboko combiner; // QUIEN ES EL QUE COMBINA
        public Kimboko kimbokoToCombine; // A QUIEN VA A COMBINAR
        public CombineAbilityEventInfo(Kimboko combiner, Kimboko kimbokoToCombine)
        {
            this.combiner = combiner;
            this.kimbokoToCombine = kimbokoToCombine;
        }
    }
}
