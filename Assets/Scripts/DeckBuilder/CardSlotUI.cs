using UnityEngine;
using UnityEngine.UI;

public class CardSlotUI : MonoBehaviour
{
    [SerializeField] private Text libraryAmount;
    [SerializeField] private Text deckAmount;
    [SerializeField] private Text totalAmountPerDeck;
    [SerializeField] private RectTransform cardInfoRect;

    RectTransform cardSlotRect;

    private void Awake()
    {
        cardSlotRect = GetComponent<RectTransform>();
    }

    public void InitializeCardSlot(CardSlot cardSlot)
    {
        libraryAmount.text = cardSlot.libraryAmount.ToString();
        deckAmount.text = cardSlot.deckAmount.ToString();
        totalAmountPerDeck.text = cardSlot.totalAmountPerDeck.ToString();
        cardSlot.OnCardInfoChange += ChangeSlotData;
    }

    public void ChangeSlotData(CardSlot cardSlot)
    {
        libraryAmount.text = cardSlot.libraryAmount.ToString();
        deckAmount.text = cardSlot.deckAmount.ToString();
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public RectTransform GetCardSlotTransform()
    {
        return cardSlotRect;
    }

}
