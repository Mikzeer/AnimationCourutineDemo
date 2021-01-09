using System.Collections.Generic;

namespace PositionerDemo
{
    public class StatModifierComparisonTypeFiltter
    {
        STATMODIFIERTYPE statModifierType;
        public StatModifierComparisonTypeFiltter(STATMODIFIERTYPE statModifierType)
        {
            this.statModifierType = statModifierType;
        }

        public bool IsValidStatModifierType(IOcuppy ocuppy)
        {
            foreach (KeyValuePair<STATTYPE,Stat> item in ocuppy.Stats)
            {
                var Value = item.Value;
                if (Value.UnitsModifiersList.Count > 0)
                {
                    for (int i = 0; i < Value.UnitsModifiersList.Count; i++)
                    {
                        if (Value.UnitsModifiersList[i].statModifierType == statModifierType)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
