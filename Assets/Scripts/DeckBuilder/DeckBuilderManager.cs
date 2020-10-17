using MikzeerGame;
using PositionerDemo;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckBuilderManager : MonoBehaviour
{
    [SerializeField] private GameObject cardSlotPrefab;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private CardLibraryScriptableObject playerCardLibrary; // LA BIBILIOTECA DE CARDS ACTUAL DEL PLAYER
    [SerializeField] private CardLibraryScriptableObject playerCardDeck; // EL MAZO ACTUAL DEL PLAYER
    [SerializeField] private RectTransform cardLibraryTransform;
    [SerializeField] private Canvas canvas;

    Dictionary<int, int> maximumCardIDPerDeck; // int IDCard - int amount per deck of that card
    Dictionary<int, int> maximumCardPerLevelPerDeck; // int CardLevel (1/2/3/4/5) - int amount per deck per level
    CardSlotManager cardSlotManager;
    CardSlotPage currentPage;

    private float maxCardPerPage = 8; // 15

    DeckBuilderSearchManager searchManager;

    [SerializeField] private List<CardRarityInteractuableImage> cardRarityInteactImage;
    [SerializeField] private List<CardActivationTypeInteractuableImage> cardActivationTypeInteractImage;
    [SerializeField] private List<CardTypeInteractuableImage> cardTypeInteractuableImage;

    [SerializeField] private Toggle chainToggle;
    [SerializeField] private Toggle darkToggle;
    [SerializeField] private InputField tagInputFiled;

    private void Awake()
    {
        CreateInteractuableImages();

        maximumCardPerLevelPerDeck = CardDatabase.GetMaximumCardPerLevelPerDeck();
        CreateDeckBuilderPageInterface(playerCardLibrary);

        chainToggle.onValueChanged.AddListener(OnIsChainableToggleChange);
        darkToggle.onValueChanged.AddListener(OnDarkPointsToggleChange);
        tagInputFiled.onEndEdit.AddListener(OnSearchByTagButtonPress);
    }

    public void CreateInteractuableImages()
    {
        searchManager = new DeckBuilderSearchManager();
        // 5 de Lvl
        // 3 de actTYpe
        // 3 de Type
        for (int i = 0; i < cardRarityInteactImage.Count; i++)
        {
            switch (i)
            {
                case 0:
                    cardRarityInteactImage[i].SetCardRarityInteractuableImage(this, CardRarity.BASIC, "1");
                    break;
                case 1:
                    cardRarityInteactImage[i].SetCardRarityInteractuableImage(this, CardRarity.COMMON, "2");
                    break;
                case 2:
                    cardRarityInteactImage[i].SetCardRarityInteractuableImage(this, CardRarity.EPIC, "3");
                    break;
                case 3:
                    cardRarityInteactImage[i].SetCardRarityInteractuableImage(this, CardRarity.LEGENDARY, "4");
                    break;
                case 4:
                    cardRarityInteactImage[i].SetCardRarityInteractuableImage(this, CardRarity.RARE, "5");
                    break;
                default:
                    cardRarityInteactImage[i].SetCardRarityInteractuableImage(this, CardRarity.BASIC, "1");
                    break;
            }
        }

        for (int i = 0; i < cardActivationTypeInteractImage.Count; i++)
        {
            switch (i)
            {
                case 0:
                    cardActivationTypeInteractImage[i].SetCardActivationTypeInteractuableImage(this, ACTIVATIONTYPE.HAND, "H");
                    break;
                case 1:
                    cardActivationTypeInteractImage[i].SetCardActivationTypeInteractuableImage(this, ACTIVATIONTYPE.AUTOMATIC, "A");
                    break;
                case 2:
                    cardActivationTypeInteractImage[i].SetCardActivationTypeInteractuableImage(this, ACTIVATIONTYPE.AUTOMATICNOTDESCARTABLE, "An");
                    break;
                default:
                    cardActivationTypeInteractImage[i].SetCardActivationTypeInteractuableImage(this, ACTIVATIONTYPE.HAND, "H");
                    break;
            }
        }

        for (int i = 0; i < cardTypeInteractuableImage.Count; i++)
        {
            switch (i)
            {
                case 0:
                    cardTypeInteractuableImage[i].SetCardTypeInteractuableImage(this, CARDTYPE.BUFF, "Bf");
                    break;
                case 1:
                    cardTypeInteractuableImage[i].SetCardTypeInteractuableImage(this, CARDTYPE.NERF, "Nf");
                    break;
                case 2:
                    cardTypeInteractuableImage[i].SetCardTypeInteractuableImage(this, CARDTYPE.NEUTRAL, "Nt");
                    break;
                default:
                    cardTypeInteractuableImage[i].SetCardTypeInteractuableImage(this, CARDTYPE.BUFF, "Bf");
                    break;
            }
        }

        searchManager.SetInteractuableList(cardRarityInteactImage, cardActivationTypeInteractImage, cardTypeInteractuableImage);
    }

    private void CreateDeckBuilderPageInterface(CardLibraryScriptableObject playerCardLibrary)
    {
        int differentCardsInLibrary = playerCardLibrary.CardsInLibrary.Count;
        // TOTAL DE CARDS DIFERENTES EN LA BIBLIOTECA DEL JUGADOR / 8 LUGARES POR PAGINA
        float dif = differentCardsInLibrary / maxCardPerPage;
        float maxSlotPages = Mathf.Ceil(dif);
        if (maxSlotPages < 1) maxSlotPages = 1;
        int mxSlt = Convert.ToInt32(maxSlotPages);
        cardSlotManager = new CardSlotManager(mxSlt);
        //Debug.Log(Mathf.Ceil(10.2F));

        int actualLibraryIndex = 0;
        for (int i = 0; i < maxSlotPages; i++)
        {
            CardSlotPage cardSlotPage = CreateCardSlotPage(i);

            for (int n = 0; n < maxCardPerPage; n++)
            {
                if (actualLibraryIndex >= playerCardLibrary.CardsInLibrary.Count)
                {
                    break;
                }

                CardSlot cardSlot = CreateCardSlot( playerCardLibrary.CardsInLibrary[actualLibraryIndex].Amount, 
                                                    0, 
                                                    playerCardLibrary.CardsInLibrary[actualLibraryIndex].CardSO.AmountPerDeck, 
                                                    actualLibraryIndex);

                CardSlotUI cardSlotUI = CreateCardSlotUI(cardSlot);

                for (int x = 0; x < playerCardLibrary.CardsInLibrary[actualLibraryIndex].Amount; x++)
                {
                    GameObject cardPrf = Instantiate(cardPrefab, cardSlotUI.GetCardSlotTransform().GetChild(0));
                    CardDisplay cardDisplay = cardPrf.GetComponent<CardDisplay>();

                    //DragDropUI dragDropUI = cardPrf.GetComponent<DragDropUI>();
                    //dragDropUI.SetEvent(cardSlot.AddDeckAmount, actualLibraryIndex);

                    cardDisplay.SetDisplay(playerCardLibrary.CardsInLibrary[actualLibraryIndex].CardSO);
                }

                cardSlotPage.AddNewSlot(cardSlot, cardSlotUI);
                actualLibraryIndex++;
            }

            cardSlotManager.AddCardSlotPage(cardSlotPage);
        }
        ShowCurrentPage();
    }

    private CardSlotPage CreateCardSlotPage(int pageIndex)
    {
        CardSlotPage cardSlotPage = new CardSlotPage(pageIndex);

        return cardSlotPage;
    }

    private CardSlot CreateCardSlot(int libraryAmount, int deckAmount, int totalAmountPerDeck, int cardIndexInLibrary)
    {
        CardSlot cardSlot = new CardSlot(libraryAmount, deckAmount, totalAmountPerDeck, cardIndexInLibrary);
        return cardSlot;
    }

    private CardSlotUI CreateCardSlotUI(CardSlot cardSlot)
    {
        GameObject cardUiPrefab = Instantiate(cardSlotPrefab, cardLibraryTransform);
        CardSlotUI cardSlotUI = cardUiPrefab.GetComponent<CardSlotUI>();
        cardUiPrefab.SetActive(false);
        cardSlotUI.InitializeCardSlot(cardSlot);
        return cardSlotUI;
    }

    private void ShowCurrentPage()
    {
        if (currentPage == null)
        {
            currentPage = cardSlotManager.GetCurrentPage();
        }

        for (int x = 0; x < currentPage.rows; x++)
        {
            for (int y = 0; y < currentPage.columns; y++)
            {
                if (currentPage.GetSlot(x, y) == null)
                {
                    return;
                }

                currentPage.GetSlotUI(x, y).SetActive(true);
            }
        }
    }

    private void HideCurrentPage()
    {
        for (int x = 0; x < currentPage.rows; x++)
        {
            for (int y = 0; y < currentPage.columns; y++)
            {
                if (currentPage.GetSlot(x, y) == null)
                {
                    return;
                }
                currentPage.GetSlotUI(x, y).SetActive(false);
            }
        }
    }

    public void NextPage()
    {
        CardSlotPage cardSlotPage = cardSlotManager.NextPage();

        if (currentPage == cardSlotPage)
        {
            Debug.Log("The same NextPage");
            return;
        }
        else
        {
            HideCurrentPage();
            currentPage = cardSlotPage;
            ShowCurrentPage();
        }
    }

    public void PreviousPage()
    {
        CardSlotPage cardSlotPage = cardSlotManager.PreviousPage();

        if (currentPage == cardSlotPage)
        {
            Debug.Log("The same PreviousPage");
            return;
        }
        else
        {
            HideCurrentPage();
            currentPage = cardSlotPage;
            ShowCurrentPage();
        }
    }

    public void OnCardDropOnDeck(int cardID)
    {

    }

    public void OnFiltterByRaraityButtonPress(CardRarity rarity)
    {
        // ACA HACEMOS LA FILTRACION
        searchManager.OnFiltterByRaraityButtonPress(rarity);
    }

    public void OnFiltterByActivationTypeButtonPress(ACTIVATIONTYPE actType)
    {
        // ACA HACEMOS LA FILTRACION
        searchManager.OnFiltterByActivationTypeButtonPress(actType);
    }

    public void OnFiltterByTypeButtonPress(CARDTYPE cardType)
    {
        // ACA HACEMOS LA FILTRACION
        searchManager.OnFiltterByTypeButtonPress(cardType);
    }

    public void OnIsChainableToggleChange(bool isOn)
    {
        searchManager.OnIsChainableToggleChange(isOn);
    }

    public void OnDarkPointsToggleChange(bool isOn)
    {
        searchManager.OnDarkPointsToggleChange(isOn);
    }

    public void OnSearchByTagButtonPress(string tagToSearch)
    {
        searchManager.OnSearchByTagButtonPress(tagToSearch);
    }

    public void OnClearSearchByTagButtonPress()
    {
        searchManager.OnClearSearchByTagButtonPress();
    }
}

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