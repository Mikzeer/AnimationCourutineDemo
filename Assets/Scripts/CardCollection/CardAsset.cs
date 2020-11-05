using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PositionerDemo
{
    [CreateAssetMenu(fileName = "New Card Asset", menuName = "Card Asset/ New Card Asset")]
    public class CardAsset : ScriptableObject, IComparable<CardAsset>
    {
        [Header("General info")]
        public int ID;
        public string CardName;
        [TextArea(2, 3)] public string Description;
        public CardRarity CardRarity;
        [PreviewSprite] public Sprite CardImage;
        public CARDTYPE CardType;
        public ACTIVATIONTYPE ActivationType;
        public bool IsChainable;
        [TextArea(2, 3)] public string Tags;  // tags that can be searched as keywords
        public List<CARDTARGETTYPE> PosibleTargets;
        public List<CardFiltterScriptableObject> Filtters;

        public int AmountPerDeck;
        public int OverrideLimitOfThisCardInDeck = -1;

        [Header("Dark Card info")]
        public bool IsDarkCard;
        public int DarkPoints;

        public int CompareTo(CardAsset other)
        {
            if (other.ID < this.ID)
            {
                return 1;
            }
            else if (other.ID > this.ID)
            {
                return -1;
            }
            else
            {
                // if mana costs are equal sort in alphabetical order
                return name.CompareTo(other.name);
            }
        }

        // Define the is greater than operator.
        public static bool operator >(CardAsset operand1, CardAsset operand2)
        {
            return operand1.CompareTo(operand2) == 1;
        }

        // Define the is less than operator.
        public static bool operator <(CardAsset operand1, CardAsset operand2)
        {
            return operand1.CompareTo(operand2) == -1;
        }

        // Define the is greater than or equal to operator.
        public static bool operator >=(CardAsset operand1, CardAsset operand2)
        {
            return operand1.CompareTo(operand2) >= 0;
        }

        // Define the is less than or equal to operator.
        public static bool operator <=(CardAsset operand1, CardAsset operand2)
        {
            return operand1.CompareTo(operand2) <= 0;
        }
    }
}
