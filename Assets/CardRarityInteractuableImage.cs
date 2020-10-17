using PositionerDemo;
using UnityEngine.EventSystems;

public class CardRarityInteractuableImage : InteractuableImage
{
    private DeckBuilderManager deckBuildManager;
    public CardRarity rarity;

    public void SetCardRarityInteractuableImage(DeckBuilderManager deckBuildManager, CardRarity rarity, string text)
    {
        this.deckBuildManager = deckBuildManager;
        this.rarity = rarity;
        this.text.text = text;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        if (IsPressed)
        {
            deckBuildManager.OnFiltterByRaraityButtonPress(rarity);
        }
        else
        {
            deckBuildManager.OnFiltterByRaraityButtonPress(CardRarity.NONE);
        }
    }
}
