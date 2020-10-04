using System.Collections.Generic;
using UnityEngine;

namespace PositionerDemo
{
    [CreateAssetMenu(fileName = "New Card", menuName = "Cards/Properties/ New Card Properties")]
    public class CardPropertiesScriptableObject : ScriptableObject
    {
        [SerializeField] private int amountPerDeck;
        [SerializeField] private int level;
        [SerializeField] private int darkPoints;
        [SerializeField] private bool isChainable;
        [SerializeField] private CARDTYPE cardType;
        [SerializeField] private ACTIVATIONTYPE activationType;
        [SerializeField] private List<CARDTARGETTYPE> posibleTargets;

        public int AmountPerDeck { get { return amountPerDeck; } protected set { amountPerDeck = value; } }
        public int Level { get { return level; } protected set { level = value; } }
        public int DarkPoints { get { return darkPoints; } protected set { darkPoints = value; } }
        public bool IsChainable { get { return isChainable; } protected set { isChainable = value; } }
        public CARDTYPE CardType { get { return cardType; } protected set { cardType = value; } }
        public ACTIVATIONTYPE ActivationType { get { return activationType; } protected set { activationType = value; } }
        public List<CARDTARGETTYPE> PosibleTargets { get { return posibleTargets; } protected set { posibleTargets = value; } }
    }
}