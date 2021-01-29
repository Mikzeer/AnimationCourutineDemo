using System.Collections.Generic;
using UnityEngine;

public class CardData
{
    public int ID;
    public string CardName;
    public string Description;
    public bool IsChainable;
    public bool IsAvailable;
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
    public List<PositionerDemo.CardEffect> cardEffects;

    public CardData(CardDataRT pCard)
    {
        ID = pCard.ID;
        CardName = pCard.CardName;
        Description = pCard.Description;
        IsChainable = pCard.IsChainable;
        IsAvailable = pCard.IsAvailable;
        IsDarkCard = pCard.IsDarkCard;
        AmountPerDeck = pCard.AmountPerDeck;
        DarkPoints = pCard.DarkPoints;
        CardRarity = PositionerDemo.CardPropertiesDatabase.GetCardRarityTypeFromInt(pCard.IDCardRarity);
        CardType = PositionerDemo.CardPropertiesDatabase.GetCardTypeFromInt(pCard.IDCardType);
        ActivationType = PositionerDemo.CardPropertiesDatabase.GetActivationTypeFromInt(pCard.IDCardActivationType);
        Tags = pCard.Tags;
        CardImage = Helper.GetSpriteFromByteArray(pCard.frontImageBytes.ToArray());
        cardTargetTypes = PositionerDemo.CardPropertiesDatabase.GetListCardTargetTypeFromListInt(pCard.CardTargetType);
        cardTargetFiltters = PositionerDemo.CardPropertiesDatabase.GetListCardFiltterFromListInt(pCard.CardTargetFiltters);
        cardEffects = new List<PositionerDemo.CardEffect>();
    }
}