using System.Collections.Generic;
using UnityEngine;

namespace PositionerDemo
{
    [CreateAssetMenu(fileName = "New Card", menuName = "Card")]
    public class CardScriptableObject : ScriptableObject
    {
        [SerializeField] private string cardName;
        [SerializeField] private string description;
        [SerializeField] private Sprite image;
        [SerializeField] private Sprite miniatureImage;
        [SerializeField] private int id;
        [SerializeField] private int amountPerDeck;
        [SerializeField] private int level;
        [SerializeField] private int darkPoints;
        [SerializeField] private bool isChainable;
        [SerializeField] private CARDTYPE cardType;
        [SerializeField] private ACTIVATIONTYPE activationType;
        [SerializeField] private List<CARDTARGETTYPE> posibleTargets;

        public string CardName { get { return cardName; } protected set { cardName = value; } }
        public string Description { get { return description; } protected set { description = value; } }
        public Sprite Image { get { return image; } protected set { image = value; } }
        public Sprite MiniatureImage { get { return miniatureImage; } protected set { miniatureImage = value; } }
        public int ID { get { return id; } protected set { id = value; } }
        public int AmountPerDeck { get { return amountPerDeck; } protected set { amountPerDeck = value; } }
        public int Level { get { return level; } protected set { level = value; } }
        public int DarkPoints { get { return darkPoints; } protected set { darkPoints = value; } }
        public bool IsChainable { get { return isChainable; } protected set { isChainable = value; } }
        public CARDTYPE CardType { get { return cardType; } protected set { cardType = value; } }
        public ACTIVATIONTYPE ActivationType { get { return activationType; } protected set { activationType = value; } }
        public List<CARDTARGETTYPE> PosibleTargets { get { return posibleTargets; } protected set { posibleTargets = value; } }
    }
}