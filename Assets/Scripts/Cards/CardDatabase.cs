using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace PositionerDemo
{
    public static class CardPropertiesDatabase
    {
        private static bool isAbilityModifierListGenerate = false;
        private static bool isFiltterListGenerate = false;
        public static int maxAmountOfCardsPerDeck { get; private set; }
        public static Dictionary<CardRarity, int> maxAmountPerRarityDictionary = new Dictionary<CardRarity, int>();
        private static Dictionary<int, CardFiltter> cardsFiltterDictionary = new Dictionary<int, CardFiltter>();
        private static Dictionary<int, AbilityModifier> cardsAbilityModifierDictionary = new Dictionary<int, AbilityModifier>();

        public static void SetCardDataLimits(CardDataLimit pcDataLimit)
        {
            maxAmountOfCardsPerDeck = pcDataLimit.MaxAmountPerDeck;
            for (int i = 0; i < pcDataLimit.MaxAmountPerRarity.cardDataLimitRarityAmount.Count; i++)
            {                
                CardRarity rarity = GetCardRarityTypeFromInt(pcDataLimit.MaxAmountPerRarity.cardDataLimitRarityAmount[i].ID);
                maxAmountPerRarityDictionary.Add(rarity, pcDataLimit.MaxAmountPerRarity.cardDataLimitRarityAmount[i].Amount);
            }
        }

        public static Dictionary<CardRarity, int> GetAmountPerCardPerLevelPerDeck()
        {
            return maxAmountPerRarityDictionary;
        }

        public static CardFiltter GetCardFilterFromID(int filtterID)
        {
            if (isFiltterListGenerate == false)
            {
                GetCardFiltterSubClassByReflection();
            }

            CardFiltter cFiltter = null;

            if (cardsFiltterDictionary.ContainsKey(filtterID))
            {
                cFiltter = cardsFiltterDictionary[filtterID];
            }

            return cFiltter;
        }

        public static AbilityModifier GetCardAbilityModifierFromID(int abilityModifierID)
        {
            if (isAbilityModifierListGenerate == false)
            {
                GetAbilityModifierSubClassByReflection();
            }

            AbilityModifier abilityModifier = null;

            if (cardsAbilityModifierDictionary.ContainsKey(abilityModifierID))
            {
                abilityModifier = cardsAbilityModifierDictionary[abilityModifierID];
            }

            return abilityModifier;
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

        private static void GetCardFiltterSubClassByReflection()
        {
            cardsFiltterDictionary = new Dictionary<int, CardFiltter>();

            var instances = from t in Assembly.GetExecutingAssembly().GetTypes()
                            where t.IsSubclassOf(typeof(CardFiltter)) && t.GetConstructor(Type.EmptyTypes) != null
                            select Activator.CreateInstance(t) as CardFiltter;
            //System.Type[] types = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
            //System.Type[] possible = (from System.Type type in types where type.IsSubclassOf(typeof(BaseClass)) select type).ToArray();
            foreach (var instance in instances)
            {
                cardsFiltterDictionary.Add(instance.ID, instance);
            }

            isFiltterListGenerate = true;
        }

        private static void GetAbilityModifierSubClassByReflection()
        {
            cardsAbilityModifierDictionary = new Dictionary<int, AbilityModifier>();

            var instances = from t in Assembly.GetExecutingAssembly().GetTypes()
                            where t.IsSubclassOf(typeof(AbilityModifier)) && t.GetConstructor(Type.EmptyTypes) != null
                            select Activator.CreateInstance(t) as AbilityModifier;
            //System.Type[] types = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
            //System.Type[] possible = (from System.Type type in types where type.IsSubclassOf(typeof(BaseClass)) select type).ToArray();
            foreach (var instance in instances)
            {
                cardsAbilityModifierDictionary.Add(instance.ID, instance);
            }

            isAbilityModifierListGenerate = true;
        }

        public static CardData ConvertCardDBToCardDataInGame(CardDataRT pcardDataRT)
        {
            CardData cData = new CardData(pcardDataRT);

            return cData;
        }
    }
}