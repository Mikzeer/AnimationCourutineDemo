using System.Collections.Generic;
using System;

[Serializable]
public class CardDataLimitRarityAmountList
{
    public List<CardDataLimitRarityAmount> cardDataLimitRarityAmount;

    public CardDataLimitRarityAmountList()
    {
        cardDataLimitRarityAmount = new List<CardDataLimitRarityAmount>();
    }
}
