using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace PositionerDemo
{
    public static class CardDatabase
    {

        private static bool isFiltterListGenerate = false;

        private static List<CardFiltter> cardsFiltter;

        private static Dictionary<int, CardFiltter> cardsFiltterDictionary = new Dictionary<int, CardFiltter>();

        public static Dictionary<CardRarity, int> amountPerCardPerLevelPerDeck = new Dictionary<CardRarity, int>
        {
            {CardRarity.BASIC, 5} ,
            {CardRarity.COMMON, 4} ,
            {CardRarity.EPIC, 3} ,
            {CardRarity.LEGENDARY, 2} ,
            {CardRarity.RARE, 1} 
        };


        public static int limitOfCardsPerDeck { get { return 20;} private set {; } }

        public static int maxAmountOfCardsPerDeck { get; private set; }
        public static Dictionary<CardRarity, int> maxAmountPerRarityDictionary = new Dictionary<CardRarity, int>();


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

        public static void SetCardDataLimits(CardDataLimit pcDataLimit)
        {
            maxAmountOfCardsPerDeck = pcDataLimit.MaxAmountPerDeck;
            for (int i = 0; i < pcDataLimit.MaxAmountPerRarity.cardDataLimitRarityAmount.Count; i++)
            {                
                CardRarity rarity = GetCardRarityTypeFromInt(pcDataLimit.MaxAmountPerRarity.cardDataLimitRarityAmount[i].ID);
                maxAmountPerRarityDictionary.Add(rarity, pcDataLimit.MaxAmountPerRarity.cardDataLimitRarityAmount[i].Amount);
            }
        }

        public static CardFiltter GetCardFilterFromID(int filtterID)
        {
            if (isFiltterListGenerate == false)
            {
                GetCardFiltterSubClassByReflection();
            }

            CardFiltter cFiltter = cardsFiltter.Where(c => c.ID == filtterID).FirstOrDefault();

            if (cardsFiltterDictionary.ContainsKey(filtterID))
            {
                cFiltter = cardsFiltterDictionary[filtterID];
            }

            return cFiltter;
        }

        public static ACTIVATIONTYPE GetActivationTypeFromInt(int pIDActType)
        {
            //var myEnumMemberCount = Enum.GetNames(typeof(MyEnum)).Length;
            //YourEnum foo = (YourEnum)yourInt;
            int acTypeTotalCount = Enum.GetNames(typeof(PositionerDemo.ACTIVATIONTYPE)).Length;

            if (pIDActType >= acTypeTotalCount)
            {
                return ACTIVATIONTYPE.NONE;
            }

            ACTIVATIONTYPE acType = (ACTIVATIONTYPE)pIDActType;
            return acType;
        }

        public static CardRarity GetCardRarityTypeFromInt(int pIDRarity)
        {
            //var myEnumMemberCount = Enum.GetNames(typeof(MyEnum)).Length;
            //YourEnum foo = (YourEnum)yourInt;
            int rarTypeTotalCount = Enum.GetNames(typeof(CardRarity)).Length;

            if (pIDRarity >= rarTypeTotalCount)
            {
                return CardRarity.NONE;
            }

            CardRarity rarType = (CardRarity)pIDRarity;
            return rarType;
        }

        public static CARDTYPE GetCardTypeFromInt(int pIDType)
        {
            //var myEnumMemberCount = Enum.GetNames(typeof(MyEnum)).Length;
            //YourEnum foo = (YourEnum)yourInt;
            int typeTotalCount = Enum.GetNames(typeof(CARDTYPE)).Length;

            if (pIDType >= typeTotalCount)
            {
                return CARDTYPE.NONE;
            }

            CARDTYPE type = (CARDTYPE)pIDType;
            return type;
        }

        public static List<CARDTARGETTYPE> GetListCardTargetTypeFromListInt(List<int> pListIntTargetType)
        {
            int targetTypeTotalCount = Enum.GetNames(typeof(CARDTARGETTYPE)).Length;
            List<CARDTARGETTYPE> cardTargetType = new List<CARDTARGETTYPE>();
            for (int i = 0; i < pListIntTargetType.Count; i++)
            {
                if (pListIntTargetType[i] < targetTypeTotalCount)
                {

                    CARDTARGETTYPE targetType = (CARDTARGETTYPE)pListIntTargetType[i];
                    cardTargetType.Add(targetType);
                }
            }

            return cardTargetType;
        }
        
        public static List<CardFiltter> GetListCardFiltterFromListInt(List<int> pListIntFiltter)
        {
            List<CardFiltter> cardFiltters = new List<CardFiltter>();

            for (int i = 0; i < pListIntFiltter.Count; i++)
            {
                CardFiltter cFilt = GetCardFilterFromID(pListIntFiltter[i]);

                if (cFilt != null)
                {
                    cardFiltters.Add(cFilt);
                }
            }

            return cardFiltters;
        }

        public static void GetCardFiltterSubClassByReflection()
        {
            cardsFiltter = new List<CardFiltter>();
            cardsFiltterDictionary = new Dictionary<int, CardFiltter>();

            var instances = from t in Assembly.GetExecutingAssembly().GetTypes()
                            where t.IsSubclassOf(typeof(CardFiltter)) && t.GetConstructor(Type.EmptyTypes) != null
                            select Activator.CreateInstance(t) as CardFiltter;
            //System.Type[] types = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
            //System.Type[] possible = (from System.Type type in types where type.IsSubclassOf(typeof(BaseClass)) select type).ToArray();
            foreach (var instance in instances)
            {
                //instance.CheckTarget(null); // where Foo is a method of ISomething
                //Debug.Log("CARD FILTTER ID " + instance.ID);
                cardsFiltter.Add(instance);
                cardsFiltterDictionary.Add(instance.ID, instance);
            }

            isFiltterListGenerate = true;
        }

        public static CardData ConvertCardDBToCardDataInGame(CardDataRT pcardDataRT)
        {
            CardData cData = new CardData(pcardDataRT);

            return cData;
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