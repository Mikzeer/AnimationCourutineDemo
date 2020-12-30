using System.Collections.Generic;
using PositionerDemo;
using System.Linq;

public class CardCollectionSearchFiltter
{
    public List<CardData> FiltterCardsData(List<CardData> cDat, bool showCardPlayerDontOwn = false, Dictionary<string, int> quantityOfCardsUserHaveFromBDOnline = null, 
                                           CardRarity rarity = CardRarity.NONE,bool isChainable = false,bool isDarkCard = false, int darkPoints = -1,
                                           CARDTYPE cardType = CARDTYPE.NONE, ACTIVATIONTYPE activationType = ACTIVATIONTYPE.NONE, string keyword = "")
    {

        if (quantityOfCardsUserHaveFromBDOnline != null && showCardPlayerDontOwn == false)
        {
            cDat = FiltterCardDataWithCardsUserDontOwn(quantityOfCardsUserHaveFromBDOnline, cDat);
        }

        cDat = FiltterCardDataByRarity(rarity, cDat);
        cDat = FiltterCardDataByType(cardType, cDat);
        cDat = FiltterCardDataByActivationType(activationType, cDat);
        if (isChainable)
        {
            cDat = FiltterCardDataOnlyChainable(cDat);
        }
        if (isDarkCard || darkPoints > -1)
        {
            cDat = FiltterCardDataOnlyDark(cDat);
        }
        if (darkPoints > -1)
        {
            cDat = FiltterCardDataByDarkPointAmount(cDat, darkPoints);
        }
        cDat = FiltterCardDataByKeyword(cDat, keyword);

        var orderList = cDat.OrderBy(c => c.ID);
        List <CardData> cda = orderList.ToList();

        return cda;
    }

    public List<CardData> FiltterCardDataWithCardsUserDontOwn(Dictionary<string, int> quantityOfCardsUserHaveFromBDOnline, List<CardData> cards)
    {
        List<CardData> returnC = new List<CardData>();
        for (int i = 0; i < cards.Count; i++)
        {
            if (quantityOfCardsUserHaveFromBDOnline.ContainsKey("CardID" + cards[i].ID))
            {
                if (quantityOfCardsUserHaveFromBDOnline["CardID" + cards[i].ID] > 0)
                {
                    returnC.Add(cards[i]);
                }

            }
        }
        return returnC;
    }

    public List<CardData> FiltterCardDataByRarity(CardRarity rarity, List<CardData> cards)
    {
        if (rarity == CardRarity.NONE)
        {
            return cards;
        }

        List<CardData> returnC = new List<CardData>();
        for (int i = 0; i < cards.Count; i++)
        {
            if (cards[i].CardRarity == rarity)
            {
                returnC.Add(cards[i]);
            }
        }
        return returnC;
    }

    public List<CardData> FiltterCardDataByType(CARDTYPE type, List<CardData> cards)
    {
        if (type == CARDTYPE.NONE)
        {
            return cards;
        }

        List<CardData> returnC = new List<CardData>();
        for (int i = 0; i < cards.Count; i++)
        {
            if (cards[i].CardType == type)
            {
                returnC.Add(cards[i]);
            }
        }
        return returnC;
    }

    public List<CardData> FiltterCardDataByActivationType(ACTIVATIONTYPE acType, List<CardData> cards)
    {
        if (acType == ACTIVATIONTYPE.NONE)
        {
            return cards;
        }

        List<CardData> returnC = new List<CardData>();
        for (int i = 0; i < cards.Count; i++)
        {
            if (cards[i].ActivationType == acType)
            {
                returnC.Add(cards[i]);
            }
        }
        return returnC;
    }

    public List<CardData> FiltterCardDataOnlyChainable(List<CardData> cards)
    {
        List<CardData> returnC = new List<CardData>();
        for (int i = 0; i < cards.Count; i++)
        {
            if (cards[i].IsChainable)
            {
                returnC.Add(cards[i]);
            }
        }
        return returnC;
    }

    public List<CardData> FiltterCardDataOnlyDark(List<CardData> cards)
    {
        List<CardData> returnC = new List<CardData>();
        for (int i = 0; i < cards.Count; i++)
        {
            if (cards[i].IsDarkCard)
            {
                returnC.Add(cards[i]);
            }
        }
        return returnC;
    }

    public List<CardData> FiltterCardDataByDarkPointAmount(List<CardData> cards, int darkPoints)
    {
        List<CardData> returnC = new List<CardData>();
        for (int i = 0; i < cards.Count; i++)
        {
            if (cards[i].IsDarkCard && cards[i].DarkPoints == darkPoints)
            {
                returnC.Add(cards[i]);
            }
        }
        return returnC;
    }

    public List<CardData> FiltterCardDataByKeyword(List<CardData> cards, string keyword)
    {
        if (keyword == null || keyword == "" || keyword == string.Empty)
        {
            return cards;
        }

        List<CardData> returnC = new List<CardData>();
        for (int i = 0; i < cards.Count; i++)
        {
            for (int j = 0; j < cards[i].Tags.Length; j++)
            {
                if (cards[i].Tags[j] == keyword)
                {
                    returnC.Add(cards[i]);
                    break;
                }
            }
        }
        return returnC;
    }

    public CardDataRT GetCardDataRTByID(Dictionary<string, CardDataRT> cardCollectionLibraryFromBDOnline, string cardID)
    {
        if (cardCollectionLibraryFromBDOnline.ContainsKey(cardID))
        {
            return cardCollectionLibraryFromBDOnline[cardID];
        }
        return null;
    }

    public List<CardData> GetCardsDataWithCardRarity(List<CardData> cDat, CardRarity rarity)
    {
        return FiltterCardsData(cDat, false, null, rarity);
    }
}