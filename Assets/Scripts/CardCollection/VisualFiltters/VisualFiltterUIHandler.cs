using PositionerDemo;
using System.Collections.Generic;

public class VisualFiltterUIHandler
{
    bool showOnlyOwned = true;
    public VisualFiltters vsFiltters { get; private set; }
    CardCollectionVisualManager cardCollectionVisualManager;
    public VisualFiltterUIHandler(CardCollectionVisualManager cardCollectionVisualManager)
    {
        vsFiltters = new VisualFiltters();
        this.cardCollectionVisualManager = cardCollectionVisualManager;
    }

    public void OnFiltterByRaraityButtonPress(List<CardRarityInteractuableImage> cardRarityInteractuableImages, CardRarity rarity)
    {
        if (vsFiltters.rarity == rarity) return;

        foreach (CardRarityInteractuableImage i in cardRarityInteractuableImages)
        {
            if (i.rarity != rarity)
            {
                i.backgroundImage.color = i.UnactiveColor;
                i.IsPressed = false;
            }
        }
        vsFiltters.rarity = rarity;
        Refresh();
    }

    public void OnFiltterByActivationTypeButtonPress(List<CardActivationTypeInteractuableImage> cardActivationTypeInteractuableImage, ACTIVATIONTYPE actType)
    {
        if (vsFiltters.actType == actType) return;

        foreach (CardActivationTypeInteractuableImage i in cardActivationTypeInteractuableImage)
        {
            if (i.actType != actType)
            {
                i.backgroundImage.color = i.UnactiveColor;
                i.IsPressed = false;
            }
        }
        vsFiltters.actType = actType;
        Refresh();
    }

    public void OnFiltterByTypeButtonPress(List<CardTypeInteractuableImage> cardTypeInteractuableImage, CARDTYPE cardType)
    {
        if (vsFiltters.cType == cardType) return;

        foreach (CardTypeInteractuableImage i in cardTypeInteractuableImage)
        {
            if (i.cardType != cardType)
            {
                i.backgroundImage.color = i.UnactiveColor;
                i.IsPressed = false;
            }
        }
        vsFiltters.cType = cardType;
        Refresh();
    }

    public void OnIsChainableToggleChange(bool isOn)
    {
        if (vsFiltters.onlyChainable == isOn) return;

        vsFiltters.onlyChainable = isOn;
        Refresh();
    }

    public void OnDarkPointsToggleChange(bool isOn)
    {
        if (vsFiltters.onlyDarkCards == isOn) return;

        vsFiltters.onlyDarkCards = isOn;
        Refresh();
    }

    public void OnSearchByTagButtonPress(string tagToSearch)
    {
        if (vsFiltters.tagKeyword == tagToSearch) return;

        vsFiltters.tagKeyword = tagToSearch;
        Refresh();
    }

    public void OnClearSearchByTagButtonPress()
    {
        vsFiltters.tagKeyword = string.Empty;
        Refresh();
    }

    public void OnOnlyUserOwnButtonPress(bool showOnlyOwned)
    {
        if (this.showOnlyOwned == showOnlyOwned) return;

        this.showOnlyOwned = showOnlyOwned;
        Refresh();
    }

    public void Refresh()
    {
        cardCollectionVisualManager.Filtter(vsFiltters, showOnlyOwned);
    }

    public void ClearAllFiltters()
    {
        vsFiltters.rarity = CardRarity.NONE;
        vsFiltters.actType = ACTIVATIONTYPE.NONE;
        vsFiltters.cType = CARDTYPE.NONE;
        vsFiltters.onlyChainable = false;
        vsFiltters.onlyDarkCards = false;
        vsFiltters.tagKeyword = string.Empty;
        vsFiltters.darkPoints = -1;
        this.showOnlyOwned = true;
        Refresh();
    }
}
