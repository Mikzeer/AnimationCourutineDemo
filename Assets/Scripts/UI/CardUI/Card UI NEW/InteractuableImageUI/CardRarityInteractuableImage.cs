using PositionerDemo;
using UnityEngine.EventSystems;

public class CardRarityInteractuableImage : InteractuableImage
{
    private CardCollectionSearchManager cardColelctionSearchManager;
    public CardRarity rarity;

    public void SetCardRarityInteractuableImage(CardCollectionSearchManager cardColelctionSearchManager, CardRarity rarity, string text)
    {
        this.cardColelctionSearchManager = cardColelctionSearchManager;
        this.rarity = rarity;
        this.text.text = text;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        if (IsPressed)
        {
            cardColelctionSearchManager.OnFiltterByRaraityButtonPress(rarity);
        }
        else
        {
            cardColelctionSearchManager.OnFiltterByRaraityButtonPress(CardRarity.NONE);
        }
    }
}
