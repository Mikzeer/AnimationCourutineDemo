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
    Dictionary<int, int> maximumCardPerLevelPerDeck; // int CardLeve (1/2/3/4/5) - int amount per deck per level
    CardSlotManager cardSlotManager;
    CardSlotPage currentPage;

    public RectTransform cardRectTest;

    private void Awake()
    {
        maximumCardPerLevelPerDeck = PositionerDemo.CardDatabase.GetMaximumCardPerLevelPerDeck();
        CreateDeckBuilderPageInterface(playerCardLibrary);
    }

    private void CreateDeckBuilderPageInterface(CardLibraryScriptableObject playerCardLibrary)
    {
        int differentCardsInLibrary = playerCardLibrary.CardsInLibrary.Count;

        // TOTAL DE CARDS DIFERENTES EN LA BIBLIOTECA DEL JUGADOR / 15 LUGARES POR PAGINA

        float dif = differentCardsInLibrary / 15f;

        float maxSlotPages = Mathf.Ceil(dif);
        if (maxSlotPages < 1) maxSlotPages = 1;
        int mxSlt = Convert.ToInt32(maxSlotPages);
        cardSlotManager = new CardSlotManager(mxSlt);

        Debug.Log(Mathf.Ceil(10.2F));

        int actualLibraryIndex = 0;
        for (int i = 0; i < maxSlotPages; i++)
        {
            CardSlotPage cardSlotPage = CreateCardSlotPage(i);

            for (int n = 0; n < 15; n++)
            {
                if (actualLibraryIndex >= playerCardLibrary.CardsInLibrary.Count)
                {
                    break;
                }

                CardSlot cardSlot = CreateCardSlot(   playerCardLibrary.CardsInLibrary[actualLibraryIndex].Amount, 
                                                    0, 
                                                    playerCardLibrary.CardsInLibrary[actualLibraryIndex].CardSO.AmountPerDeck, 
                                                    actualLibraryIndex);

                CardSlotUI cardSlotUI = CreateCardSlotUI(cardSlot);

                for (int x = 0; x < playerCardLibrary.CardsInLibrary[actualLibraryIndex].Amount; x++)
                {
                    GameObject cardPrf = Instantiate(cardPrefab, cardSlotUI.GetCardSlotTransform().GetChild(0));
                    CardDisplay cardDisplay = cardPrf.GetComponent<CardDisplay>();

                    DragDropUI dragDropUI = cardPrf.GetComponent<DragDropUI>();
                    dragDropUI.SetEvent(cardSlot.AddDeckAmount, actualLibraryIndex);

                    cardDisplay.SetDisplay(playerCardLibrary.CardsInLibrary[actualLibraryIndex].CardSO);
                }

                // poner el CardPrefab como Hijo del CardSlotUI
                //GameObject cardPrf = Instantiate(cardPrefab, cardSlotUI.GetCardSlotTransform().GetChild(0));
                //CardDisplay cardDisplay = cardPrf.GetComponent<CardDisplay>();

                //DragDropUI dragDropUI = cardPrf.GetComponent<DragDropUI>();
                //dragDropUI.SetEvent(cardSlot.AddDeckAmount, actualLibraryIndex);

                //cardDisplay.SetDisplay(playerCardLibrary.CardsInLibrary[actualLibraryIndex].CardSO);

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

}
