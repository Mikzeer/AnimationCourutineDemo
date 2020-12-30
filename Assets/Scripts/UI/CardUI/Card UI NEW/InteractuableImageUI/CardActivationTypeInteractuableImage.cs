using PositionerDemo;
using UnityEngine.EventSystems;

public class CardActivationTypeInteractuableImage : InteractuableImage
{
    private CardCollectionSearchManager cardColelctionSearchManager;
    public ACTIVATIONTYPE actType;

    public void SetCardActivationTypeInteractuableImage(CardCollectionSearchManager cardColelctionSearchManager, ACTIVATIONTYPE actType, string text)
    {
        this.cardColelctionSearchManager = cardColelctionSearchManager;
        this.actType = actType;
        this.text.text = text;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        if (IsPressed)
        {
            cardColelctionSearchManager.OnFiltterByActivationTypeButtonPress(actType);
        }
        else
        {
            cardColelctionSearchManager.OnFiltterByActivationTypeButtonPress(ACTIVATIONTYPE.NONE);
        }
    }

}
