using System.Collections.Generic;
using UnityEngine;

namespace PositionerDemo
{
    public class DecombineAbilityEventInfo : AbilityEventInfo
    {
        public Kimboko decombiner; // QUIEN ES EL QUE DESCOMBINA
        public List<Kimboko> kimbokoToDecombine; // A QUIENES VA A DESCOMBINAR
        public List<Vector2> tilesToDecombine; // EN QUE TILES VA A DESCOMBINARLOS
        public DecombineAbilityEventInfo(Kimboko decombiner, List<Kimboko> kimbokoToDecombine, List<Vector2> tilesToDecombine)
        {
            this.decombiner = decombiner;
            this.kimbokoToDecombine = kimbokoToDecombine;
            this.tilesToDecombine = tilesToDecombine;
        }
    }
}
