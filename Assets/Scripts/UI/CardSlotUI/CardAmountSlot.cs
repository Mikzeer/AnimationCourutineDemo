using UnityEngine;

public class CardAmountSlot : MonoBehaviour
{
    [SerializeField] private GameObject hasOne = default;
    [SerializeField] private GameObject hasInDeck = default;

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
