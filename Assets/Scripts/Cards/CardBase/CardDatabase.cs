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
        private static bool isCardListGenerate = false;
        public static int maxAmountOfCardsPerDeck { get; private set; }
        public static Dictionary<CardRarity, int> maxAmountPerRarityDictionary = new Dictionary<CardRarity, int>();
        private static Dictionary<int, CardFiltter> cardsFiltterDictionary = new Dictionary<int, CardFiltter>();
        private static Dictionary<int, AbilityModifier> cardsAbilityModifierDictionary = new Dictionary<int, AbilityModifier>();
        private static Dictionary<int, Card> cardDictionary = new Dictionary<int, Card>();


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
            int acTypeTotalCount = Enum.GetNames(typeof(ACTIVATIONTYPE)).Length;

            if (pIDActType -1 >= acTypeTotalCount || pIDActType - 1 < 0)
            {
                return ACTIVATIONTYPE.NONE;
            }

            ACTIVATIONTYPE acType = (ACTIVATIONTYPE)pIDActType-1;
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

            if (pIDType-1 >= typeTotalCount || pIDType-1 < 0)
            {
                return CARDTYPE.NONE;
            }

            CARDTYPE type = (CARDTYPE)pIDType-1;
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
                if (cardsAbilityModifierDictionary.ContainsKey(instance.ID) == false)
                {
                    cardsAbilityModifierDictionary.Add(instance.ID, instance);
                }                
            }

            isAbilityModifierListGenerate = true;
        }

        public static void GetCardSubClassByReflection()
        {
            cardDictionary = new Dictionary<int, Card>();

            // QUE TIPO ESTAMOS BUSCANDO
            Type parentType = typeof(Card);
            // OBTENEMOS EL ASSEMBLY ACTUAL
            Assembly assembly = Assembly.GetExecutingAssembly();
            // OBTENEMOS TODOS LOS TYPES QUE TIENE EL ASSEMBLY ACTUAL (MES DE 700 SEGURO)
            Type[] types = assembly.GetTypes();

            //IEnumerable<Type> subclasses = types.Where(t => t.IsSubclassOf(parentType));
            //foreach (Type type in subclasses)
            //{
            //    Debug.Log(type.Name);
            //}

            // VAMOS A BUSCAR EN TYPES QUE SON TODOS LOS TIPOS/CLASES/INTERFACES/ETC QUE EXISTEN EN ESTE ASSEMBLY
            var instances = from typ in types
                            // DONDE LA BASE SEA EL PARENT CARD
                            where typ.BaseType == parentType 
                            &&
                            // Y QUE SEA UNA SUBCLASE DE PARENT CARD
                            typ.IsSubclassOf(parentType) 
                            &&
                            // Y QUE SEA UNA CLASE... POR LAS DUDAS DE QUE NO ME TRAIGA UNA INTERFACE
                            typ.IsClass 
                            && 
                            // Y QUE NO SEA DE TIPO ABSTRACTO
                            !typ.IsAbstract 
                            // CON ESTO ULTIMO HACEMSO QUE LO QUE ESTAMOS SELECCIONADO SEA UNA NUEVA INSTANCIA "new Card(int ID)"
                            select Activator.CreateInstance(typ) as Card;

            //object instance = Activator.CreateInstance(someTypeObject);
            //((ISomeInterface)instance).Initialize(your, specific, parameters, here);

            foreach (var instance in instances)
            {
                if (cardDictionary.ContainsKey(instance.ID) == false)
                {
                    cardDictionary.Add(instance.ID, instance);
                }
            }

            isCardListGenerate = true;
        }

        public static Card GetCardFromID(int cardAID)
        {
            if (isCardListGenerate == false)
            {
                GetCardSubClassByReflection();
            }
            Card card = null;
            if (cardDictionary.ContainsKey(cardAID))
            {
                // USAMOS EL CLONE YA QUE QUEREMOS UNA NUEVA INSTANCIA DE ESA CARD
                // SI NO LA CLONARAMOS TENDRIAMOS LA MISMA CARD PARA LOS DOS JUGADORES SI ES QUE LOS DOS TIENE LA MISMA CARD
                // O TENDRIAMOS EN EL MISMO JUGADOR LA MISMA REFERENCIA A UNA CARD SI TIENE DOS CARDS IGUALES
                // Y CUALQUIER DATA QUE CAMBIEMOS EN UNA SE LE CAMBIARA EN LA OTRA 
                card = (Card)cardDictionary[cardAID].Clone();
            }

            return card;
        }
    }
}