using System;
using UnityEngine;

public class CardSlot
{
    public int userCollectionAmount { get; protected set; }
    public int deckAmount { get; protected set; }
    public int maxAmountPerDeck { get; protected set; }
    public event Action<CardSlot> OnCardInfoChange;

    public CardSlot(int userCollectionAmount, int deckAmount, int maxAmountPerDeck)
    {
        this.userCollectionAmount = userCollectionAmount;
        this.deckAmount = deckAmount;
        this.maxAmountPerDeck = maxAmountPerDeck;
    }

    public void AddDeckAmount()
    {
        deckAmount++;
        userCollectionAmount--;
        OnCardInfoChange?.Invoke(this);
    }

    public void AddDeckAmount(int amount)
    {
        deckAmount+= amount;
        userCollectionAmount-= amount;
        OnCardInfoChange?.Invoke(this);
    }

    public void RestDeckAmount()
    {
        deckAmount--;
        userCollectionAmount++;
        OnCardInfoChange?.Invoke(this);
    }

    public void AddCardToUser()
    {
        userCollectionAmount++;
        OnCardInfoChange?.Invoke(this);
    }
}