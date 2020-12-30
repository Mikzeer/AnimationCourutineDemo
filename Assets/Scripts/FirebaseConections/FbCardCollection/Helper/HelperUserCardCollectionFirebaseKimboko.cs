using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class HelperUserCardCollectionFirebaseKimboko
{
    bool debugOn = true;
    FbUserCardCollectionCreation fbUserCardCollectionCreation;
    FbUserCardCollection fbUserCardCollection;
    FbUserCardCollectionUpdater fbUserCardCollectionUpdater;

    public HelperUserCardCollectionFirebaseKimboko()
    {
        fbUserCardCollection = new FbUserCardCollection();
        fbUserCardCollectionCreation = new FbUserCardCollectionCreation();
        fbUserCardCollectionUpdater = new FbUserCardCollectionUpdater();
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