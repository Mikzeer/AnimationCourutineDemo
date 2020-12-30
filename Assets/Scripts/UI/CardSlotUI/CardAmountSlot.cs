using UnityEngine;

public class CardAmountSlot : MonoBehaviour
{
    [SerializeField] private GameObject hasOne;
    [SerializeField] private GameObject hasInDeck;

    public void TurnOnHasOne()
    {
        hasOne.SetActive(true);
    }
    public void TurnOnHasInDeck()
    {
        hasInDeck.SetActive(true);
    }
    public void TurnOffHasInDeck()
    {
        hasInDeck.SetActive(false);
    }

}
