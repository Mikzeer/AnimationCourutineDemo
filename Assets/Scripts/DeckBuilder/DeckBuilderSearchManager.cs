using PositionerDemo;
using System.Collections.Generic;
using UnityEngine;

public class DeckBuilderSearchManager
{
    List<CardRarityInteractuableImage> cardRarityInteractuableImages = new List<CardRarityInteractuableImage>();
    CardRarity rarity = CardRarity.NONE;
    List<CardActivationTypeInteractuableImage> cardActivationTypeInteractuableImage = new List<CardActivationTypeInteractuableImage>();
    ACTIVATIONTYPE actType = ACTIVATIONTYPE.NONE;
    List<CardTypeInteractuableImage> cardTypeInteractuableImage = new List<CardTypeInteractuableImage>();
    CARDTYPE cardType = CARDTYPE.NONE;

    bool showCardPlayerDontOwn = false;

    bool onlyChainable = false;
    bool onlyDarkPoints = false;
    string tagKeywordo = string.Empty;
    
    public void SetInteractuableList(List<CardRarityInteractuableImage> cardRarityInteractuableImages, List<CardActivationTypeInteractuableImage> cardActivationTypeInteractuableImage, List<CardTypeInteractuableImage> cardTypeInteractuableImage)
    {
        this.cardRarityInteractuableImages = cardRarityInteractuableImages;
        this.cardActivationTypeInteractuableImage = cardActivationTypeInteractuableImage;
        this.cardTypeInteractuableImage = cardTypeInteractuableImage;
    }

    public void OnFiltterByRaraityButtonPress(CardRarity rarity)
    {
        if (rarity == CardRarity.NONE)
        {
            // ACA TENGO QUE SACAR TODOS LOS FILTROS A LA MIERDA Y DESELECCIONAR TODO
            foreach (CardRarityInteractuableImage i in cardRarityInteractuableImages)
            {
                i.backgroundImage.color = i.UnactiveColor;
                i.IsPressed = false;
            }

            // ACA SACAMOS TODOS LOS FILTROS A LA CHOTA Y MOSTRAMOS LAS CARD DE LA COLLECTION SIN FILTRO
            return;
        }

        //RemoveAllFilters();
        foreach (CardRarityInteractuableImage i in cardRarityInteractuableImages)
        {
            if (i.rarity != rarity)
            {
                i.backgroundImage.color = i.UnactiveColor;
                i.IsPressed = false;
            }
        }
              
        if (this.rarity != rarity)
        {
            this.rarity = rarity;
        }
        else
        {
            return;
        }        

        // ACA DEBERIAMOS HACER LA FILTRACION
    }

    public void OnFiltterByActivationTypeButtonPress(ACTIVATIONTYPE actType)
    {
        if (actType == ACTIVATIONTYPE.NONE)
        {
            // ACA TENGO QUE SACAR TODOS LOS FILTROS A LA MIERDA Y DESELECCIONAR TODO
            foreach (CardActivationTypeInteractuableImage i in cardActivationTypeInteractuableImage)
            {
                i.backgroundImage.color = i.UnactiveColor;
                i.IsPressed = false;
            }

            // ACA SACAMOS TODOS LOS FILTROS A LA CHOTA Y MOSTRAMOS LAS CARD DE LA COLLECTION SIN FILTRO
            return;
        }

        //RemoveAllFilters();
        foreach (CardActivationTypeInteractuableImage i in cardActivationTypeInteractuableImage)
        {
            if (i.actType != actType)
            {
                i.backgroundImage.color = i.UnactiveColor;
                i.IsPressed = false;
            }
        }

        if (this.actType != actType)
        {
            this.actType = actType;
        }
        else
        {
            return;
        }

        // ACA DEBERIAMOS HACER LA FILTRACION
    }

    public void OnFiltterByTypeButtonPress(CARDTYPE cardType)
    {
        if (cardType == CARDTYPE.NONE)
        {
            // ACA TENGO QUE SACAR TODOS LOS FILTROS A LA MIERDA Y DESELECCIONAR TODO
            foreach (CardTypeInteractuableImage i in cardTypeInteractuableImage)
            {
                i.backgroundImage.color = i.UnactiveColor;
                i.IsPressed = false;
            }

            // ACA SACAMOS TODOS LOS FILTROS A LA CHOTA Y MOSTRAMOS LAS CARD DE LA COLLECTION SIN FILTRO
            return;
        }

        //RemoveAllFilters();
        foreach (CardTypeInteractuableImage i in cardTypeInteractuableImage)
        {
            if (i.cardType != cardType)
            {
                i.backgroundImage.color = i.UnactiveColor;
                i.IsPressed = false;
            }
        }

        if (this.cardType != cardType)
        {
            this.cardType = cardType;
        }
        else
        {
            return;
        }

        // ACA DEBERIAMOS HACER LA FILTRACION
    }

    public void OnIsChainableToggleChange(bool isOn)
    {
        // ACA DEBERIAMOS HACER LA FILTRACION
    }

    public void OnDarkPointsToggleChange(bool isOn)
    {
        // ACA DEBERIAMOS HACER LA FILTRACION
    }

    public void OnSearchByTagButtonPress(string tagToSearch)
    {

        Debug.Log("tagToSearch " + tagToSearch);
        // ACA DEBERIAMOS HACER LA FILTRACION
    }

    public void OnClearSearchByTagButtonPress()
    {
        // ACA DEBERIAMOS HACER LA FILTRACION
    }

    public List<CardAsset> GetCards(bool showingCardsPlayerDoesNotOwn = false, bool includeAllRarities = true, bool includeAllCardTypes = true, bool includeAllActivationType = true,
                                bool isChainable = false, bool isDarkCard = false, int darkPoints = -1,
                                CardRarity rarity = CardRarity.BASIC, CARDTYPE cardType = CARDTYPE.NEUTRAL, ACTIVATIONTYPE activationType = ACTIVATIONTYPE.HAND, string keyword = "")
    {
        List<CardAsset> returnList = new List<CardAsset>();

        // obtain cards from collection that satisfy all the selected criteria
        List<CardAsset> cardsToChooseFrom = CardCollection.Instance.GetCards(showingCardsPlayerDoesNotOwn, includeAllRarities, includeAllCardTypes, includeAllActivationType,
            isChainable, isDarkCard, darkPoints, rarity, cardType, activationType, keyword);

        return returnList;
    }
}