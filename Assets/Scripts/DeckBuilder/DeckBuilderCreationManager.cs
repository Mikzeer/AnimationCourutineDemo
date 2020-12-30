using PositionerDemo;
using System;
using UnityEngine;

public class DeckBuilderCreationManager
{
    event Action<Deck> OnDeckChange;
    RibbonManager ribbonManager;
    int minCardPerDeck = 10;
    bool debugOn = false;
    public Deck auxDeck { get; private set; }
    public bool isEditing { get; private set; }
    public bool isDirty { get; private set; }

    public DeckBuilderCreationManager(Action<Deck> OnDeckChange, RibbonManager ribbonManager)
    {
        this.OnDeckChange = OnDeckChange;
        this.ribbonManager = ribbonManager;
        isEditing = false;
        isDirty = false;
    }

    private int GetDeckIDFromJson()
    {
        HelperCardCollectionJsonKimboko help = new HelperCardCollectionJsonKimboko();
        return help.GetDeckIDFromJson();
    }

    public void CreateNewDeck()
    {
        isDirty = false;
        isEditing = true;
        int ID = GetDeckIDFromJson();
        auxDeck = new Deck(ID);
        auxDeck.name = "DeckID" + ID;
    }

    private void SetDeckIDToJson(DeckID cDataList)
    {
        HelperCardCollectionJsonKimboko help = new HelperCardCollectionJsonKimboko();
        help.SetDeckIDToJson(cDataList);
    }

    public void OnTryToAddCardToDeck(CardData cardData, CardSlotUI pCardSlotUI)
    {
        if (isEditing == false) return;

        if (auxDeck == null)
        {
            if (debugOn) Debug.Log("NO AUX DECK");
            return;
        }

        // REHABILITAR CUANDO SE ACTUALIZEN BIEN LOS DATOS DE LA BD ONLINE 
        //if (cardData.IsAvailable == false)
        //{
        //    if (debugOn) Debug.Log("CARD NOT AVAILABLE YET");
        //    return;
        //}

        if (pCardSlotUI.cardSlot.userCollectionAmount <= 0)
        {
            if (debugOn) Debug.Log("NO QUEDAN DE ESAS CARDS EN LA COLECCION");
            return;
        }

        if (auxDeck.totalCards >= CardPropertiesDatabase.maxAmountOfCardsPerDeck)
        {
            if (debugOn) Debug.Log("SE SUPERO EL LIMITE DE CARDS POR MAZO");
            return;
        }

        if (auxDeck.GerCardAmountByRarity(cardData.CardRarity) >= CardPropertiesDatabase.GetAmountPerCardPerLevelPerDeck()[cardData.CardRarity])
        {
            if (debugOn) Debug.Log("SE SUPERO EL LIMITE DE CARDS POR TIPO DE RAREZA");
            return;
        }

        if (auxDeck.GetCardAmount(cardData) >= cardData.AmountPerDeck)
        {
            if (debugOn) Debug.Log("SE SUPERO EL LIMITE DE CARDS POR TIPO ESE TIPO DE CARD");
            return;
        }

        auxDeck.AddCard(cardData);
        pCardSlotUI.cardSlot.AddDeckAmount();
        RibbonData ribbonData = new RibbonData(cardData.CardName, pCardSlotUI.cardSlot.deckAmount, cardData, pCardSlotUI);
        ribbonManager.AddRibbon(ribbonData, this);
        if (debugOn) Debug.Log("CARD ADDED " + cardData.CardName);

        OnDeckChange?.Invoke(auxDeck);
        isDirty = true;
    }

    public void OnTryRemoveRibbonCardFromDeck(RibbonData ribbonData)
    {
        if (isEditing == false) return;

        if (auxDeck == null)
        {
            if (debugOn) Debug.Log("NO AUX DECK");
            return;
        }
        auxDeck.RemoveCard(ribbonData.cardData);
        ribbonData.pCardSlotUI.cardSlot.RestDeckAmount();
        if (auxDeck.GetCardAmount(ribbonData.cardData) <= 0)
        {
            ribbonManager.RemoveRibbon(ribbonData);
        }
        else
        {
            ribbonManager.RestRibbon(ribbonData);
        }
        OnDeckChange?.Invoke(auxDeck);
        isDirty = true;
    }

    public void SaveNewDeck()
    {
        DeckID cDataList = new DeckID(auxDeck.ID);
        SetDeckIDToJson(cDataList);
    }

    public bool IsDeckQuantityMoreThanTheMinimum()
    {
        if (auxDeck.totalCards < minCardPerDeck) return false;
        return true;
    }

    public void OnDeckNameEndEdit(string deckName)
    {
        isDirty = true;
        auxDeck.name = deckName;
    }

    public void Clear()
    {
        auxDeck = null;
        isEditing = false;
    }

    public void ModifyDeck(Deck userDeck)
    {
        isDirty = false;
        auxDeck = userDeck;
        isEditing = true;
        OnDeckChange?.Invoke(auxDeck);
    }

    public void OnTryDeleteUserDeck(Deck userDeck)
    {
        auxDeck = userDeck;
    }

}
