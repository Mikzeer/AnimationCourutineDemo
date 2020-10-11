using PositionerDemo;
using UnityEngine;

[CreateAssetMenu(fileName = "New CardLibrary", menuName = "Cards Library/ New Card Amount")]
public class CardAmountScriptableObject : ScriptableObject
{

    [SerializeField] private int amount;
    [SerializeField] private CardScriptableObject cardSO;
    public CardScriptableObject CardSO { get { return cardSO; } protected set { cardSO = value; } }
    public int Amount { get { return amount; } protected set { amount = value; } }
}