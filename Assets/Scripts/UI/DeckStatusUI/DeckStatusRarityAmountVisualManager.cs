using PositionerDemo;
using System.Collections.Generic;

public class DeckStatusRarityAmountVisualManager
{
    Dictionary<CardRarity, DeckStatusRarityAmountSlot> amountPerRarity;

    public DeckStatusRarityAmountVisualManager()
    {
        amountPerRarity = new Dictionary<CardRarity, DeckStatusRarityAmountSlot>();
    }

    public void AddRarity(CardRarity rarity, DeckStatusRarityAmountSlot statusSlot, int maxAmount)
    {
        if (amountPerRarity.ContainsKey(rarity) == false)
        {
            amountPerRarity.Add(rarity, statusSlot);
            amountPerRarity[rarity].SetMaxAmountAndRarity(maxAmount, rarity);
            statusSlot.UpdateActualAmount(0);
        }
    }

    public void RefreshRarityAmount(Dictionary<CardRarity, int> amountPerRarityInDeck)
    {
        foreach (KeyValuePair<CardRarity, DeckStatusRarityAmountSlot> rar in amountPerRarity)
        {
            if (amountPerRarityInDeck.ContainsKey(rar.Key))
            {
                rar.Value.UpdateActualAmount(amountPerRarityInDeck[rar.Key]);
            }
            else
            {
                rar.Value.UpdateActualAmount(0);
            }
        }
    }

    public void ClearAllActualAmount()
    {
        foreach (KeyValuePair<CardRarity, DeckStatusRarityAmountSlot> rar in amountPerRarity)
        {
            rar.Value.UpdateActualAmount(0);
        }
    }
}
