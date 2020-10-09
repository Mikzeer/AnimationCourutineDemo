using System.Collections.Generic;

namespace PositionerDemo
{
    public static class CardDatabase
    {
        public static Dictionary<int, AbilityModifier> abilitiesModifier;

        public static AbilityModifier GetModifier(int modifierID)
        {
            AbilityModifier abMod = null;

            switch (modifierID)
            {
                case 0:
                    abMod = new DefendAbilityModifier();
                    break;
                case 1:
                    abMod = new ShieldAbilityModifier();
                    break;
                case 2:
                    abMod = new ChangeUnitClassAbilityModifier();
                    break;
                default:
                    break;
            }

            return abMod;
        }

        //// Add this method to the CharacterStat class
        //private int CompareModifierOrder(StatModifier a, StatModifier b)
        //{
        //    if (a.Order < b.Order)
        //        return -1;
        //    else if (a.Order > b.Order)
        //        return 1;
        //    return 0; // if (a.Order == b.Order)
        //}
    }
}