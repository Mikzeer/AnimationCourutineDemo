using System.Collections.Generic;
using UnityEngine;

public class CardData
{
    public int ID;
    public string CardName;
    public string Description;
    public bool IsChainable;
    public bool IsDarkCard;
    public int AmountPerDeck;
    public int DarkPoints;
    public string[] Tags;
    public PositionerDemo.CardRarity CardRarity;
    public PositionerDemo.CARDTYPE CardType;
    public PositionerDemo.ACTIVATIONTYPE ActivationType;
    public Sprite CardImage;
    public List<PositionerDemo.CARDTARGETTYPE> cardTargetTypes;
    public List<PositionerDemo.CardFiltter> cardTargetFiltters;

    public CardData(CardDataRT pCard)
    {
        ID = pCard.ID;
        CardName = pCard.CardName;
        Description = pCard.Description;
        IsChainable = pCard.IsChainable;
        IsDarkCard = pCard.IsDarkCard;
        AmountPerDeck = pCard.AmountPerDeck;
        DarkPoints = pCard.DarkPoints;
        CardRarity = PositionerDemo.CardDatabase.GetCardRarityTypeFromInt(pCard.IDCardRarity);
        CardType = PositionerDemo.CardDatabase.GetCardTypeFromInt(pCard.IDCardType);
        ActivationType = PositionerDemo.CardDatabase.GetActivationTypeFromInt(pCard.IDCardActivationType);
        Tags = pCard.Tags;
        CardImage = Helper.GetSpriteFromByteArray(pCard.frontImageBytes.ToArray());
        cardTargetTypes = PositionerDemo.CardDatabase.GetListCardTargetTypeFromListInt(pCard.CardTargetType);
        cardTargetFiltters = PositionerDemo.CardDatabase.GetListCardFiltterFromListInt(pCard.CardTargetFiltters);
    }

    public CardData(int ID)
    {
        this.ID = ID;
    }
}