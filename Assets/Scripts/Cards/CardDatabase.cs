using System.Collections.Generic;

namespace PositionerDemo
{
    public static class CardDatabase
    {

        public static Dictionary<CardRarity, int> amountPerCardPerLevelPerDeck = new Dictionary<CardRarity, int>
        {
            {CardRarity.BASIC, 5} ,
            {CardRarity.COMMON, 4} ,
            {CardRarity.EPIC, 3} ,
            {CardRarity.LEGENDARY, 2} ,
            {CardRarity.RARE, 1} 
        };

        public static int limitOfCardsPerDeck { get { return 20;} private set {; } }

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

        public static Dictionary<int,int> GetMaximumCardPerLevelPerDeck()
        {
            Dictionary<int, int> maximumCardPerLevelPerDeck = new Dictionary<int, int>();

            maximumCardPerLevelPerDeck.Add(1, 5); // LEVEL 1 / MAX AMOUNT 5
            maximumCardPerLevelPerDeck.Add(2, 4);
            maximumCardPerLevelPerDeck.Add(3, 3);
            maximumCardPerLevelPerDeck.Add(4, 2);
            maximumCardPerLevelPerDeck.Add(5, 1); // LEVEL 5 / MAX AMOUNT 1

            return maximumCardPerLevelPerDeck;
        }

        public static Dictionary<CardRarity, int> GetAmountPerCardPerLevelPerDeck()
        {
            return amountPerCardPerLevelPerDeck;
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