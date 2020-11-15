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
    Dictionary<CardRarity, int> maxAmountPerRarityDictionary;// Rarity (1/2/3/4/5) - int amount per deck per level
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

        maxAmountPerRarityDictionary = CardPropertiesDatabase.GetAmountPerCardPerLevelPerDeck();
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
        GameObject cardSlotUIPrefab = Instantiate(cardSlotPrefab, cardLibraryTransform);
        CardSlotUI cardSlotUI = cardSlotUIPrefab.GetComponent<CardSlotUI>();
        cardSlotUIPrefab.SetActive(false);
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
