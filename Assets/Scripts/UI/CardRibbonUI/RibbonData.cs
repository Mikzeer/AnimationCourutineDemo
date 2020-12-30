public class RibbonData
{
    public string name;
    public int amount;
    public CardData cardData;
    public CardSlotUI pCardSlotUI;
    public RibbonData(string name, int amount, CardData cardData, CardSlotUI pCardSlotUI)
    {
        this.name = name;
        this.amount = amount;
        this.cardData = cardData;
        this.pCardSlotUI = pCardSlotUI;
    }
}
