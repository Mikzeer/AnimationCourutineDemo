using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class HelperCardCollectionFirebaseKimboko
{
    bool debugOn = true;

    FbGameCardCollection fbGameCardCollection;
    FbUserCardCollectionCreation fbUserCardCollectionCreation;
    FbUserCardCollection fbUserCardCollection;
    FbGameCardCollectionUpdater fbGameCardCollectionUpdater;
    FbUserCardCollectionUpdater fbUserCardCollectionUpdater;

    public HelperCardCollectionFirebaseKimboko()
    {
        fbGameCardCollection = new FbGameCardCollection();
        fbUserCardCollection = new FbUserCardCollection();
        fbUserCardCollectionCreation = new FbUserCardCollectionCreation();
        fbGameCardCollectionUpdater = new FbGameCardCollectionUpdater();
        fbUserCardCollectionUpdater = new FbUserCardCollectionUpdater();
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

    public async Task<UserCollectionAuxiliar> CreateAndReturnNewUserCollection(UserDB pUser)
    {
        List<DefaultCollectionDataDB> dfCollection = await fbUserCardCollectionCreation.CreateNewUserCollection(pUser);
        Dictionary<string, int> quantityOfCardsUserHaveFromBDOnline = new Dictionary<string, int>();
        foreach (DefaultCollectionDataDB ca in dfCollection)
        {
            if (!quantityOfCardsUserHaveFromBDOnline.ContainsKey(ca.ID))
                quantityOfCardsUserHaveFromBDOnline.Add(ca.ID, ca.Amount);
        }

        UserCollectionAuxiliar userCollectionAuxiliar = new UserCollectionAuxiliar(dfCollection, quantityOfCardsUserHaveFromBDOnline);



        if (debugOn) Debug.Log("DEFAULT USER COLLECTION LOADED FROM FIREBASE");
        return userCollectionAuxiliar;
    }

    public async Task<UserCollectionAuxiliar> GetUserCollectionFromFirebase(UserDB pUser)
    {
        List<DefaultCollectionDataDB> dfCollection = await fbUserCardCollection.GetUserCardCollection(pUser);
        Dictionary<string, int> quantityOfCardsUserHaveFromBDOnline = new Dictionary<string, int>();
        foreach (DefaultCollectionDataDB ca in dfCollection)
        {
            if (!quantityOfCardsUserHaveFromBDOnline.ContainsKey(ca.ID))
                quantityOfCardsUserHaveFromBDOnline.Add(ca.ID, ca.Amount);
        }

        UserCollectionAuxiliar userCollectionAuxiliar = new UserCollectionAuxiliar(dfCollection, quantityOfCardsUserHaveFromBDOnline);

        if (debugOn) Debug.Log("USER CARD COLLECTION LOADED FROM DB ONLINE");

        return userCollectionAuxiliar;
    }

    public async Task<bool> AddNewCardToUserCollection(DefaultCollectionDataDB dfCollCardToAdd, UserDB pUser)
    {
        bool isLoaded = await fbUserCardCollection.SetNewCardToUserCardCollection(dfCollCardToAdd, pUser);
        return isLoaded;
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

    public async Task<long> GetLastUserCollectionDownloadByUser(UserDB pUser)
    {
        long bdLastUserCollectionDownloadByUser = await fbUserCardCollectionUpdater.GetLastUserCardCollectionDownloadTimestampUser(pUser.Name.ToLower());
        return bdLastUserCollectionDownloadByUser;
    }

    public async Task<long> GetLastUserCollectionModificationByUser(UserDB pUser)
    {
        long bdLastUserCollectionModification = await fbUserCardCollectionUpdater.GetLastUserCardCollectioModificationTimestampUser(pUser.Name.ToLower());
        return bdLastUserCollectionModification;
    }

    public void RestCardAmountFromCardCollection(DefaultCollectionDataDB pCardData, UserDB pUserDB)
    {
        DefaultCollectionDataDB dfColl = new DefaultCollectionDataDB(pCardData.ID, 1);
        fbUserCardCollection.RestCardAmountFromCardCollection(dfColl, pUserDB);
    }

}