using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New CardLibrary", menuName = "Cards Library/ New Card Library")]
public class CardLibraryScriptableObject : ScriptableObject
{
    [SerializeField] private List<CardAmountScriptableObject> cardsInLibrary;
    public List<CardAmountScriptableObject> CardsInLibrary { get { return cardsInLibrary; } protected set { cardsInLibrary = value; } }
}
