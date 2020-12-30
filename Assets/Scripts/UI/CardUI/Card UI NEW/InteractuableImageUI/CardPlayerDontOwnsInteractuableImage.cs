using UnityEngine.EventSystems;

public class CardPlayerDontOwnsInteractuableImage : InteractuableImage
{
    private CardCollectionSearchManager cardColelctionSearchManager;
    public bool showOnlyOwned;

    public void SetCardPlayerDontOwnsInteractuableImage(CardCollectionSearchManager cardColelctionSearchManager, bool showOnlyOwned, string text)
    {
        this.cardColelctionSearchManager = cardColelctionSearchManager;
        this.showOnlyOwned = showOnlyOwned;
        this.text.text = text;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        if (IsPressed)
        {
            cardColelctionSearchManager.OnOnlyUserOwnButtonPress(false);
        }
        else
        {
            cardColelctionSearchManager.OnOnlyUserOwnButtonPress(true);
        }
    }
}