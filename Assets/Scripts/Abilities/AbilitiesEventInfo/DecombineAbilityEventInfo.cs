using System.Collections.Generic;
using UnityEngine;

namespace PositionerDemo
{
    public class DecombineAbilityEventInfo : AbilityEventInfo
    {
        public Kimboko decombiner; // QUIEN ES EL QUE DESCOMBINA
        Dictionary<Kimboko, Vector2> kimbokoToDecombineAndPositions;// QUIENES Y EN DONDE VAN A DESCOMBINAR
        public DecombineAbilityEventInfo(Kimboko decombiner, Dictionary<Kimboko, Vector2> kimbokoToDecombineAndPositions)
        {
            this.decombiner = decombiner;
            this.kimbokoToDecombineAndPositions = kimbokoToDecombineAndPositions;
        }
    }
}
