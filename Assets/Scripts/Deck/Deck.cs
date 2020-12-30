using PositionerDemo;
using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class Deck
{
    public int ID;
    public string name;
    public int totalCards;
    public Dictionary<CardData, DefaultCollectionDataDB> userDeck;
    public Dictionary<CardRarity, int> amountPerRarity;
    public List<DefaultCollectionDataDB> userDeckJson;

    public Deck(int ID)
    {
        this.ID = ID;
        userDeck = new Dictionary<CardData, DefaultCollectionDataDB>();
        amountPerRarity = new Dictionary<CardRarity, int>();
    }

    public void AddCard(CardData pCardData)
    {
        if (userDeck.ContainsKey(pCardData))
        {
            userDeck[pCardData].Amount++;
        }
        else
        {
            DefaultCollectionDataDB df = new DefaultCollectionDataDB("CardID" + pCardData.ID, 1);
            userDeck.Add(pCardData, df);
        }

        if (amountPerRarity.ContainsKey(pCardData.CardRarity))
        {
            amountPerRarity[pCardData.CardRarity]++;
        }
        else
        {
            amountPerRarity.Add(pCardData.CardRarity, 1);
        }
        totalCards++;
    }

    public void RemoveCard(CardData pCardData)
    {
        if (userDeck.ContainsKey(pCardData))
        {
            userDeck[pCardData].Amount--;
            if (userDeck[pCardData].Amount == 0)
            {
                userDeck.Remove(pCardData);
            }

            if (amountPerRarity.ContainsKey(pCardData.CardRarity))
            {
                amountPerRarity[pCardData.CardRarity]--;
                if (amountPerRarity[pCardData.CardRarity] == 0)
                {
                    amountPerRarity.Remove(pCardData.CardRarity);
                }
            }
            totalCards--;
        }
    }

    public int GetCardAmount(CardData pCardData)
    {
        for (int i = 0; i < userDeck.Count; i++)
        {
            var item = userDeck.ElementAt(i);
            var itemValue = item.Value;
            if (pCardData.ID == item.Key.ID) return itemValue.Amount;
        }
        return 0;
    }

    public int GerCardAmountByRarity(CardRarity pRarity)
    {
        if (amountPerRarity.ContainsKey(pRarity)) return amountPerRarity[pRarity];
        return 0;
    }

    public void GenerateUserDeckToJson()
    {
        userDeckJson = new List<DefaultCollectionDataDB>();
        foreach (KeyValuePair<CardData, DefaultCollectionDataDB> item in userDeck)
        {
            DefaultCollectionDataDB df = new DefaultCollectionDataDB(item.Value.ID, item.Value.Amount);
            userDeckJson.Add(df);

            if (amountPerRarity.ContainsKey(item.Key.CardRarity))
            {
                amountPerRarity[item.Key.CardRarity]++;
            }
            else
            {
                amountPerRarity.Add(item.Key.CardRarity, 1);
            }
        }
    }

}
