using PositionerDemo;
using UnityEngine.EventSystems;

public class CardTypeInteractuableImage : InteractuableImage
{
    private CardCollectionSearchManager cardColelctionSearchManager;
    public CARDTYPE cardType;

    public void SetCardTypeInteractuableImage(CardCollectionSearchManager cardColelctionSearchManager, CARDTYPE cardType, string text)
    {
        this.cardColelctionSearchManager = cardColelctionSearchManager;
        this.cardType = cardType;
        this.text.text = text;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        if (IsPressed)
        {
            cardColelctionSearchManager.OnFiltterByTypeButtonPress(cardType);
        }
        else
        {
            cardColelctionSearchManager.OnFiltterByTypeButtonPress(CARDTYPE.NONE);
        }
    }
}