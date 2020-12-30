using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class HelperGameCardCollectionFirebaseKimboko
{
    bool debugOn = true;

    FbGameCardCollection fbGameCardCollection;
    FbGameCardCollectionUpdater fbGameCardCollectionUpdater;

    public HelperGameCardCollectionFirebaseKimboko()
    {
        fbGameCardCollection = new FbGameCardCollection();
        fbGameCardCollectionUpdater = new FbGameCardCollectionUpdater();
    }

    public void Test()
    {
        fbGameCardCollection.UpdateLastGameCardCollectionUpdateTOERASELATERJUSTTOTEST();
    }

    public async Task<GameCollectionAuxiliar> GetGameCollectionFromFirebase(UserDB pUser)
    {
        List<CardDataRT> cardData = await fbGameCardCollection.GetGameCardCollection(pUser);

        Dictionary<string, CardDataRT> cardCollectionLibraryFromBDOnline = new Dictionary<string, CardDataRT>();
        List<CardData> cDat = new List<CardData>();
        foreach (CardDataRT ca in cardData)
        {
            if (!cardCollectionLibraryFromBDOnline.ContainsKey("CardID" + ca.ID))
                cardCollectionLibraryFromBDOnline.Add("CardID" + ca.ID, ca);

            CardData cAux = new CardData(ca);
            cDat.Add(cAux);
        }
        CardData[] allCardsDataArray = cDat.ToArray();

        GameCollectionAuxiliar gameCollectionAuxiliar = new GameCollectionAuxiliar(cardData, cDat, cardCollectionLibraryFromBDOnline, allCardsDataArray);

        if (debugOn) Debug.Log("GAME CARD COLLECTION LOADED FROM FIREBASE");

        return gameCollectionAuxiliar;
    }

    public async Task<GameCollectionAuxiliar> GetGameCollectionFromFirebase()
    {
        List<CardDataRT> cardData = await fbGameCardCollection.GetGameCardCollection();

        Dictionary<string, CardDataRT> cardCollectionLibraryFromBDOnline = new Dictionary<string, CardDataRT>();
        List<CardData> cDat = new List<CardData>();
        foreach (CardDataRT ca in cardData)
        {
            if (!cardCollectionLibraryFromBDOnline.ContainsKey("CardID" + ca.ID))
                cardCollectionLibraryFromBDOnline.Add("CardID" + ca.ID, ca);

            CardData cAux = new CardData(ca);
            cDat.Add(cAux);
        }
        CardData[] allCardsDataArray = cDat.ToArray();

        GameCollectionAuxiliar gameCollectionAuxiliar = new GameCollectionAuxiliar(cardData, cDat, cardCollectionLibraryFromBDOnline, allCardsDataArray);

        if (debugOn) Debug.Log("GAME CARD COLLECTION LOADED FROM FIREBASE");

        return gameCollectionAuxiliar;
    }

    public async Task<long> GetLastGameCollectionDownloadByUser(UserDB pUser)
    {
        long bdLastGameCollectionDownloadByUser = await fbGameCardCollectionUpdater.GetLastGameCardCollectionDownloadTimestampUser(pUser.Name.ToLower());
        return bdLastGameCollectionDownloadByUser;
    }

    public async Task<long> GetLastGameCollectionUpdate()
    {
        long bdLastGameCollectionUpdate = await fbGameCardCollectionUpdater.GetLastGameCardCollectionUpdateTimestamp();
        return bdLastGameCollectionUpdate;
    }

}
