using System.Collections.Generic;
using System;
using UnityEngine.UI;

[Serializable]
public class CardDataRT
{
    public int ID;
    public string CardName;
    public string Description;
    public bool IsChainable;
    public bool IsAvailable;
    public bool IsDarkCard;
    public int AmountPerDeck;
    public int DarkPoints;

    // NO HACE FALTA QUE TENGAMOS UNA LISTA DE OBJETOS YA QUE PODEMOS TRAER ESTA INFO POR ID
    public int IDCardRarity;
    public int IDCardType;
    public int IDCardActivationType;

    public List<byte> frontImageBytes;
    public string[] Tags;
    public List<int> CardTargetType;
    public List<int> CardTargetFiltters;
    
    public CardDataRT()
    {

    }

    public CardDataRT(CardDataRT pCard)
    {
        ID = pCard.ID;
        CardName = pCard.CardName;
        Description = pCard.Description;
        IsChainable = pCard.IsChainable;
        IsAvailable = pCard.IsAvailable;
        IsDarkCard = pCard.IsDarkCard;
        AmountPerDeck = pCard.AmountPerDeck;
        DarkPoints = pCard.DarkPoints;
        IDCardRarity = pCard.IDCardRarity;
        IDCardType = pCard.IDCardType;
        IDCardActivationType = pCard.IDCardActivationType;
        frontImageBytes = pCard.frontImageBytes;
        Tags = pCard.Tags;

        CardTargetFiltters = new List<int>();

        for (int i = 0; i < pCard.CardTargetFiltters.Count; i++)
        {
            CardTargetFiltters.Add(pCard.CardTargetFiltters[i]);
        }

        CardTargetType = new List<int>();

        for (int i = 0; i < pCard.CardTargetType.Count; i++)
        {
            CardTargetType.Add(pCard.CardTargetType[i]);
        }
    }

    public Dictionary<string, System.Object> ToDictionary()
    {
        Dictionary<string, System.Object> result = new Dictionary<string, System.Object>();
        result["ID"] = ID;
        result["CardName"] = CardName;
        result["Description"] = Description;
        result["IsChainable"] = IsChainable;
        result["IsAvailable"] = IsAvailable;
        result["IsDarkCard"] = IsDarkCard;
        result["AmountPerDeck"] = AmountPerDeck;
        result["DarkPoints"] = DarkPoints;
        result["IDCardRarity"] = IDCardRarity;
        result["IDCardType"] = IDCardType;
        result["IDCardActivationType"] = IDCardActivationType;
        result["frontImageBytes"] = frontImageBytes;
        result["Tags"] = Tags;
        result["CardTargetType"] = CardTargetType;
        result["CardTargetFiltters"] = CardTargetFiltters;

        return result;
    }
}
