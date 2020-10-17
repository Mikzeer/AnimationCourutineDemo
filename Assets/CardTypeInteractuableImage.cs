using PositionerDemo;
using UnityEngine.EventSystems;

public class CardTypeInteractuableImage : InteractuableImage
{
    private DeckBuilderManager deckBuildManager;
    public CARDTYPE cardType;

    public void SetCardTypeInteractuableImage(DeckBuilderManager deckBuildManager, CARDTYPE cardType, string text)
    {
        this.deckBuildManager = deckBuildManager;
        this.cardType = cardType;
        this.text.text = text;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        if (IsPressed)
        {
            deckBuildManager.OnFiltterByTypeButtonPress(cardType);
        }
        else
        {
            deckBuildManager.OnFiltterByTypeButtonPress(CARDTYPE.NONE);
        }
    }
}