using MikzeerGame;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CardCollectionVisualManager : MonoBehaviour
{
    [SerializeField] private GameObject cardSlotPrefab;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private RectTransform cardLibraryTransform;
    [SerializeField] private DeckBuilderManager deckBuilderManager;
    [SerializeField] private ScrollRect cardScrollRectParent;
    CardCollectionSlotCreator cardCollectionSlotCreator;
    CardCollectionCardDisplayCreator cardCollectionCardDisplayCreator;
    Dictionary<CardData, CardDisplaySlot> cardSlotUIDisplay;
    VisualFilttersManagers visualFilttersManagers;
    [SerializeField] private GameMenuManager gameMenuManager;
    bool isLoaded = false;
    private void Awake()
    {
        cardCollectionSlotCreator = new CardCollectionSlotCreator(cardSlotPrefab, cardLibraryTransform);
        cardCollectionCardDisplayCreator = new CardCollectionCardDisplayCreator(cardPrefab);
        cardSlotUIDisplay = new Dictionary<CardData, CardDisplaySlot>();
        visualFilttersManagers = new VisualFilttersManagers();
    }

    public void CreateCollectionVisualUI(CardData[] allExistingCards, Dictionary<string, int> userCollection)
    {
        //if (isLoaded) return;
        if (cardSlotUIDisplay != null)
        {
            foreach (KeyValuePair<CardData, CardDisplaySlot> csUI in cardSlotUIDisplay)
            {
                csUI.Value.cardSlotUI.DestroyCardSlotUI(csUI.Value.cardSlotUI.cardSlot);
            }
        }
        cardSlotUIDisplay.Clear();
        cardSlotUIDisplay = new Dictionary<CardData, CardDisplaySlot>();
        for (int i = 0; i < allExistingCards.Length; i++)
        {
            string ID = "CardID" + allExistingCards[i].ID;
            if (userCollection.ContainsKey(ID))
            {
                CreteSlotForCollection(allExistingCards[i], false, true);
            }
            else
            {
                CreteSlotForCollection(allExistingCards[i], false, false);
            }
        }
        isLoaded = true;
    }

    public void CreateDeckBuilderVisualUI(CardData[] allExistingCards, Dictionary<string, int> userCollection)
    {
        if (cardSlotUIDisplay != null)
        {
            foreach (KeyValuePair<CardData, CardDisplaySlot> csUI in cardSlotUIDisplay)
            {
                csUI.Value.cardSlotUI.DestroyCardSlotUI(csUI.Value.cardSlotUI.cardSlot);
            }
        }
        cardSlotUIDisplay.Clear();
        cardSlotUIDisplay = new Dictionary<CardData, CardDisplaySlot>();
        for (int i = 0; i < allExistingCards.Length; i++)
        {
            string ID = "CardID" + allExistingCards[i].ID;
            if (userCollection.ContainsKey(ID))
            {
                CreteSlotForCollection(allExistingCards[i], true, true);
            }
        }
    }

    private void CreteSlotForCollection(CardData cardData, bool isBuilding, bool isAvailable)
    {
        // CARD DATA // MAX AMOUNT PER DECK // USERS COLLECTION AMOUNT
        int maxPerDeck = cardData.AmountPerDeck;
        int userCollAmount = gameMenuManager.GetUserCollectionAmountByCardID("CardID" + cardData.ID);
        int cardsInDeck = 0;

        CardSlotUI cardSlotUI = cardCollectionSlotCreator.CreateCardSlotUI(userCollAmount, cardsInDeck, maxPerDeck);
        CardDisplay cardDisplay;
        if (isBuilding == true)
        {
            cardDisplay = cardCollectionCardDisplayCreator.CreateCardDisplayForDeckBuilder(cardData, cardSlotUI, deckBuilderManager.deckBuilderCreationManager, cardScrollRectParent);
        }
        else
        {
            cardDisplay = cardCollectionCardDisplayCreator.CreateCardDisplayForCollection(cardData, cardSlotUI);
        }

        if (isAvailable == false)
        {
            SetCardToNotAvailable(cardSlotUI);
            cardSlotUI.SetActive(false);
        }
        CardDisplaySlot cardDisplaySlot = new CardDisplaySlot(cardSlotUI, cardDisplay);
        cardSlotUIDisplay.Add(cardData, cardDisplaySlot);
    }

    private void SetCardToNotAvailable(CardSlotUI cardSlotUI)
    {
        cardSlotUI.SetIsAvailable(false);
    }

    private void SetCardToAvailable(CardSlotUI cardSlotUI)
    {
        cardSlotUI.SetIsAvailable(true);
    }

    public void Filtter(VisualFiltters pVFiltters, bool showOnlyOwned)
    {
        if (deckBuilderManager.IsEditing())
        {
            visualFilttersManagers.Filtter(pVFiltters, cardSlotUIDisplay, false);
        }
        else
        {
            visualFilttersManagers.Filtter(pVFiltters, cardSlotUIDisplay, showOnlyOwned);
        }

    }

    public void AddCardSlotUI(CardData cardData)
    {
        if (cardSlotUIDisplay.ContainsKey(cardData))
        {
            if (cardSlotUIDisplay[cardData].cardSlotUI.cardSlot.userCollectionAmount == 0 && cardSlotUIDisplay[cardData].cardSlotUI.cardSlot.deckAmount == 0)
            {
                SetCardToAvailable(cardSlotUIDisplay[cardData].cardSlotUI);
            }
            cardSlotUIDisplay[cardData].cardSlotUI.cardSlot.AddCardToUser();
        }
        else
        {
            Debug.Log("LA CARD NO SE PUEDE AGREGAR POR QUE NO EXISTE");                
        }
    }

    public CardDisplay CreateCardDisplayForRibbon(RectTransform transformParent)
    {
        CardDisplay cardDisplay = cardCollectionCardDisplayCreator.CreateOnlyDisply(transformParent);
        RectTransform cardDisplayRect = cardDisplay.GetComponent<RectTransform>();
        // Si, le tengo que cambiar el anclaje por que la UI de Unity es bastante confusa a veces.
        cardDisplayRect.anchorMin = Vector2.zero;
        cardDisplayRect.anchorMax = Vector2.zero;
        return cardDisplay;
    }

    public Dictionary<CardData, CardDisplaySlot> GetCardDisplaySlotDictionary()
    {
        return cardSlotUIDisplay;
    }
}