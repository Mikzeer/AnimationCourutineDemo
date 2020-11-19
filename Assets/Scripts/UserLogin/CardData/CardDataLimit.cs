using System.Collections.Generic;
using System;

[Serializable]
public class CardDataLimit
{
    public int MaxAmountPerDeck;
    public CardDataLimitRarityAmountList MaxAmountPerRarity;

    public CardDataLimit()
    {
        MaxAmountPerRarity = new CardDataLimitRarityAmountList();
    }

    public Dictionary<string, System.Object> ToDictionary()
    {
        Dictionary<string, System.Object> result = new Dictionary<string, System.Object>();
        result["MaxAmountPerDeck"] = MaxAmountPerDeck;
        result["MaxAmountPerRarity"] = MaxAmountPerRarity;

        return result;
    }
}