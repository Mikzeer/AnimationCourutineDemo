using System.Collections.Generic;
using System;

[Serializable]
public class CardDataRTList
{
    public List<CardDataRT> cardDataList;

    public CardDataRTList()
    {
        cardDataList = new List<CardDataRT>();
    }

    public CardDataRTList(List<CardDataRT> dfCollection)
    {
        this.cardDataList = dfCollection;
    }
}