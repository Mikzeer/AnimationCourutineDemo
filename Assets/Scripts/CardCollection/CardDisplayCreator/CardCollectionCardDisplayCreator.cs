using MikzeerGame;
using PositionerDemo;
using UnityEngine;
using UnityEngine.UI;

public class CardCollectionCardDisplayCreator
{
    private GameObject cardPrefab;

    public CardCollectionCardDisplayCreator(GameObject cardPrefab)
    {
        this.cardPrefab = cardPrefab;
    }

    public CardDisplay CreateCardDisplayForCollection(CardData cardData, CardSlotUI cardSlotUI)
    {
        CardDisplay cardDisplay = CreateSimpleDisplay(cardData, cardSlotUI);
        CardVisualCollectionUINEW dbUI = cardDisplay.gameObject.AddComponent<CardVisualCollectionUINEW>();
        dbUI.SetSlot(cardSlotUI);
        return cardDisplay;
    }

    public CardDisplay CreateCardDisplayForDeckBuilder(CardData cardData, CardSlotUI cardSlotUI, DeckBuilderCreationManager deckBuilderCreationManager, ScrollRect cardScrollRectParent)
    {
        CardDisplay cardDisplay = CreateSimpleDisplay(cardData, cardSlotUI);
        DeckBuilderCardOnlyClickUINEW dbUI = cardDisplay.gameObject.AddComponent<DeckBuilderCardOnlyClickUINEW>();
        dbUI.SetEvent(deckBuilderCreationManager.OnTryToAddCardToDeck, cardData, cardSlotUI);
        dbUI.SetScrollRect(cardScrollRectParent);
        return cardDisplay;
    }

    public CardDisplay CreateSimpleDisplay(CardData cardData, CardSlotUI cardSlotUI)
    {
        GameObject cardPrf = GameObject.Instantiate(cardPrefab, cardSlotUI.GetCardSlotTransform().GetChild(0));
        CardDisplay cardDisplay = cardPrf.GetComponent<CardDisplay>();
        cardDisplay.SetDisplay(cardData);
        cardSlotUI.SetCardDisplay(cardDisplay);
        return cardDisplay;
    }

    public CardDisplay CreateOnlyDisply(RectTransform transformParent)
    {
        GameObject cardPrf = GameObject.Instantiate(cardPrefab, transformParent);
        CardDisplay cardDisplay = cardPrf.GetComponent<CardDisplay>();
        return cardDisplay;
    }
}