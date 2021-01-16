using System.Collections.Generic;

namespace PositionerDemo
{
    public class CombineAbilityEventInfo : AbilityEventInfo
    {
        public Kimboko combiner; // QUIEN ES EL QUE COMBINA
        public Kimboko kimbokoToCombine; // A QUIEN VA A COMBINAR
        public Player player;
        public int IndexID;
        public CombineAbilityEventInfo(Kimboko combiner, Kimboko kimbokoToCombine, Player player, int IndexID)
        {
            this.combiner = combiner;
            this.kimbokoToCombine = kimbokoToCombine;
            this.player = player;
            this.IndexID = IndexID;
        }
    }
}
