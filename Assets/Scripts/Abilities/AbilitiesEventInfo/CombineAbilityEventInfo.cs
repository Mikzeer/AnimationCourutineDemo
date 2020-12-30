using System.Collections.Generic;

namespace PositionerDemo
{
    public class CombineAbilityEventInfo : AbilityEventInfo
    {
        public Kimboko combiner; // QUIEN ES EL QUE COMBINA
        public List<Kimboko> kimbokoToCombine; // A QUIENES VA A COMBINAR
        public CombineAbilityEventInfo(Kimboko combiner, List<Kimboko> kimbokoToCombine)
        {
            this.combiner = combiner;
            this.kimbokoToCombine = kimbokoToCombine;
        }
    }
}
