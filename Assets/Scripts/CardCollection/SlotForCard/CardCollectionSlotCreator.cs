using UnityEngine;

public class CardCollectionSlotCreator
{
    private GameObject cardSlotPrefab;
    private RectTransform cardLibraryTransform;

    public CardCollectionSlotCreator(GameObject cardSlotPrefab, RectTransform cardLibraryTransform)
    {
        this.cardSlotPrefab = cardSlotPrefab;
        this.cardLibraryTransform = cardLibraryTransform;
    }

    public CardSlotUI CreateCardSlotUI(int libraryAmount, int deckAmount, int totalAmountPerDeck)
    {
        CardSlot cardSlot = new CardSlot(libraryAmount, deckAmount, totalAmountPerDeck);
        GameObject cardSlotUIPrefab = GameObject.Instantiate(cardSlotPrefab, cardLibraryTransform);
        CardSlotUI cardSlotUI = cardSlotUIPrefab.GetComponent<CardSlotUI>();
        cardSlotUI.InitializeCardSlot(cardSlot);
        return cardSlotUI;
    }
}
