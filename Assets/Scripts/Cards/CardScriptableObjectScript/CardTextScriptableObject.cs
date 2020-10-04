using UnityEngine;

namespace PositionerDemo
{
    [CreateAssetMenu(fileName = "New Card", menuName = "Cards/Text/ New Card Text")]
    public class CardTextScriptableObject : ScriptableObject
    {
        [SerializeField] private string cardName;
        [SerializeField] private string description;

        public string CardName { get { return cardName; } protected set { cardName = value; } }
        public string Description { get { return description; } protected set { description = value; } }
    }


}