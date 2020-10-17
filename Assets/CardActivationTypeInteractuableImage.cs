using PositionerDemo;
using UnityEngine.EventSystems;

public class CardActivationTypeInteractuableImage : InteractuableImage
{
    private DeckBuilderManager deckBuildManager;
    public ACTIVATIONTYPE actType;

    public void SetCardActivationTypeInteractuableImage(DeckBuilderManager deckBuildManager, ACTIVATIONTYPE actType, string text)
    {
        this.deckBuildManager = deckBuildManager;
        this.actType = actType;
        this.text.text = text;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        if (IsPressed)
        {
            deckBuildManager.OnFiltterByActivationTypeButtonPress(actType);
        }
        else
        {
            deckBuildManager.OnFiltterByActivationTypeButtonPress(ACTIVATIONTYPE.NONE);
        }
    }

}
