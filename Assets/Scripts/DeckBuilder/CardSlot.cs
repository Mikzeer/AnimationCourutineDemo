using System;
using UnityEngine;

public class CardSlot
{
    public int libraryAmount { get; protected set; }
    public int deckAmount { get; protected set; }
    public int totalAmountPerDeck { get; protected set; }
    public int cardIndexInLibrary { get; protected set; }
    public event Action<CardSlot> OnCardInfoChange;

    public CardSlot(int libraryAmount, int deckAmount, int totalAmountPerDeck, int cardIndexInLibrary)
    {
        this.libraryAmount = libraryAmount;
        this.deckAmount = deckAmount;
        this.totalAmountPerDeck = totalAmountPerDeck;
        this.cardIndexInLibrary = cardIndexInLibrary;
    }

    public void AddDeckAmount()
    {
        deckAmount++;
        libraryAmount--;
        OnCardInfoChange?.Invoke(this);
    }

    public void RestDeckAmount()
    {
        deckAmount--;
        libraryAmount++;
        OnCardInfoChange?.Invoke(this);
    }
}
