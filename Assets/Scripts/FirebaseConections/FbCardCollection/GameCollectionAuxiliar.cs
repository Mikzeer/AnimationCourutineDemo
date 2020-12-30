using System.Collections.Generic;

public class GameCollectionAuxiliar
{
    public List<CardDataRT> cardData;
    public List<CardData> cDat;
    public Dictionary<string, CardDataRT> cardCollectionLibraryFromBDOnline;
    public CardData[] allCardsDataArray;

    public GameCollectionAuxiliar(List<CardDataRT> cardData, List<CardData> cDat, Dictionary<string, CardDataRT> cardCollectionLibraryFromBDOnline, CardData[] allCardsDataArray)
    {
        this.cardCollectionLibraryFromBDOnline = cardCollectionLibraryFromBDOnline;
        this.cardData = cardData;
        this.cDat = cDat;
        this.allCardsDataArray = allCardsDataArray;
    }
}
