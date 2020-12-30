using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;
using System.Collections;

public class CardCollectionManager
{
    private CardData[] allCardsDataArray; // ALL THE CARDS THAT EXIST IN THE GAME
    private Dictionary<string, CardDataRT> cardCollectionLibraryFromBDOnline = new Dictionary<string, CardDataRT>(); // id // carddata
    private Dictionary<string, int> quantityOfCardsUserHaveFromBDOnline = new Dictionary<string, int>(); // id / amount
    private bool isLoaded = false;
    private HelperCardCollectionJsonKimboko helperCardCollectionJsonKimboko;
    private HelperUserCardCollectionFirebaseKimboko helperUserCardCollectionFirebaseKimboko;
    private HelperGameCardCollectionFirebaseKimboko helperGameCardCollectionFirebaseKimboko;
    private GameMenuManager gameMenuManager;
    public static Action OnCardCollectionLoad;
    public CardCollectionManager(GameMenuManager owner)
    {
        this.gameMenuManager = owner;

        helperCardCollectionJsonKimboko = new HelperCardCollectionJsonKimboko();
        helperUserCardCollectionFirebaseKimboko = new HelperUserCardCollectionFirebaseKimboko();
        helperGameCardCollectionFirebaseKimboko = new HelperGameCardCollectionFirebaseKimboko();
    }

    public async void CreateNewUserCollections(UserDB pUser)
    {
        UserCollectionAuxiliar userCollectionAuxiliar = await helperUserCardCollectionFirebaseKimboko.CreateAndReturnNewUserCollection(pUser);
        quantityOfCardsUserHaveFromBDOnline = userCollectionAuxiliar.quantityOfCardsUserHaveFromBDOnline;
        helperCardCollectionJsonKimboko.SetUserCollectionToJson(userCollectionAuxiliar.dfCollection);

        LoadGameCollectionFromFirebase(pUser);
    }

    public void LoadCollections(UserDB pUser)
    {
        CheckLastGameCollectionUpdate(pUser);
        CheckLastUserCollectionUpdate(pUser);
    }

    private async void CheckLastGameCollectionUpdate(UserDB pUser)
    {
        long bdLastGameCollectionUpdate = await helperGameCardCollectionFirebaseKimboko.GetLastGameCollectionUpdate();
        long bdLastGameCollectionDownloadByUser = await helperGameCardCollectionFirebaseKimboko.GetLastGameCollectionDownloadByUser(pUser);
        DateTime dtLGCU = Helper.UnixTimeStampToDateTimeMiliseconds(bdLastGameCollectionUpdate);
        DateTime dtLGCDBU = Helper.UnixTimeStampToDateTimeMiliseconds(bdLastGameCollectionDownloadByUser);
        int dtCompareGameCollection = DateTime.Compare(dtLGCU, dtLGCDBU);

        switch (dtCompareGameCollection)
        {
            case -1:
                //date1 is earlier than date2.// EL JUGADOR TIENE LA ULTIMA ACTUALIZACION DE LA CARD COLLECTION
                LoadGameCollectionFromJson();
                break;
            case 0:
                //date1 is the same as date2.// EL JUGADOR TIENE LA ULTIMA ACTUALIZACION DE LA CARD COLLECTION MUY RARO ESTO PERO PUEDE SER...
                LoadGameCollectionFromJson();
                break;
            case 1:
                // If date1 is later than date2. // ACA HAY UNA ACTUALIZACION Y ENTONCES TENEMOS QUE CARGARLO DESDE LA BD ONLINE
                LoadGameCollectionFromFirebase(pUser);
                break;
            default:
                break;
        }
    }

    private async void CheckLastUserCollectionUpdate(UserDB pUser)
    {
        long bdLastUserCollectionDownloadByUserJson = helperCardCollectionJsonKimboko.GetLastUserCollectionUpdateFromJsonLong();
        long bdLastUserCollectionDownloadByUser = await helperUserCardCollectionFirebaseKimboko.GetLastUserCollectionDownloadByUser(pUser);
        DateTime dtJson = Helper.UnixTimeStampToDateTimeMiliseconds(bdLastUserCollectionDownloadByUserJson);
        DateTime dtBD = Helper.UnixTimeStampToDateTimeMiliseconds(bdLastUserCollectionDownloadByUser);
        int dtCompareUserCollection = DateTime.Compare(dtBD, dtJson);
        Debug.Log("DTJSON " + bdLastUserCollectionDownloadByUserJson + "// " + dtJson);
        Debug.Log("DTBD " + bdLastUserCollectionDownloadByUser + "// " + dtBD);

        long bdLastUserCollectionModification = await helperUserCardCollectionFirebaseKimboko.GetLastUserCollectionModificationByUser(pUser);
        DateTime dtLastMod = Helper.UnixTimeStampToDateTimeMiliseconds(bdLastUserCollectionModification);
        int dtCompareLastModification = DateTime.Compare(dtLastMod, dtBD);
        //Debug.Log("dtLastMod " + bdLastUserCollectionModification + "// " + dtLastMod);
        switch (dtCompareLastModification)
        {
            case 1:
                // If date1 is later than date2.// SE MODIFICO DESPUES DE LA ULTIMA DESCARGADA
                //Debug.Log("ENTRA POR MODIFICATION");
                LoadUserCollectionFromFirebase(pUser);
                return;
            default:
                break;
        }

        switch (dtCompareUserCollection)
        {
            case -1:
                //date1 is earlier than date2.// EL JUGADOR TIENE LA ULTIMA ACTUALIZACION DE LA CARD COLLECTION
                LoadUserCollectionFromJson();
                break;
            case 0:
                //date1 is the same as date2.// EL JUGADOR TIENE LA ULTIMA ACTUALIZACION DE LA CARD COLLECTION MUY RARO ESTO PERO PUEDE SER...
                LoadUserCollectionFromFirebase(pUser);
                break;
            case 1:
                // If date1 is later than date2.// ACA HAY UNA ACTUALIZACION Y ENTONCES TENEMOS QUE CARGARLO DESDE LA BD ONLINE
                //Debug.Log("ENTRA POR LAST DOWNLOAD");
                LoadUserCollectionFromFirebase(pUser);
                break;
            default:
                break;
        }
    }

    private async void LoadGameCollectionFromFirebase(UserDB pUser)
    {
        GameCollectionAuxiliar gameCollectionAuxiliar = await helperGameCardCollectionFirebaseKimboko.GetGameCollectionFromFirebase(pUser);
        cardCollectionLibraryFromBDOnline = gameCollectionAuxiliar.cardCollectionLibraryFromBDOnline;
        allCardsDataArray = gameCollectionAuxiliar.allCardsDataArray;
        helperCardCollectionJsonKimboko.SetGameCollectionToJson(gameCollectionAuxiliar.cardData);
        isLoaded = true;        
    }

    private void LoadGameCollectionFromJson()
    {
        GameCollectionAuxiliar gameCollectionAuxiliar = helperCardCollectionJsonKimboko.GetGameCollectionFromJson();
        cardCollectionLibraryFromBDOnline = gameCollectionAuxiliar.cardCollectionLibraryFromBDOnline;
        allCardsDataArray = gameCollectionAuxiliar.allCardsDataArray;
        isLoaded = true;
    }

    public async void LoadUserCollectionFromFirebase(UserDB pUser)
    {
        UserCollectionAuxiliar userCollectionAuxiliar = await helperUserCardCollectionFirebaseKimboko.GetUserCollectionFromFirebase(pUser);
        quantityOfCardsUserHaveFromBDOnline = userCollectionAuxiliar.quantityOfCardsUserHaveFromBDOnline;
        helperCardCollectionJsonKimboko.SetUserCollectionToJson(userCollectionAuxiliar.dfCollection);
        gameMenuManager.StartCoroutine(WaitForLoading(userCollectionAuxiliar.dfCollection));
    }

    private IEnumerator WaitForLoading(List<DefaultCollectionDataDB> dfCollection)
    {
        while (isLoaded == false)
        {
            yield return null;
        }
        //TOERASEGAMECARDCOLLECTIONAUTOUPDATE();
        gameMenuManager.LoadVisualCollection(allCardsDataArray, quantityOfCardsUserHaveFromBDOnline);
        OnCardCollectionLoad?.Invoke();
        Debug.Log("ALL LOADED");
    }

    private void LoadUserCollectionFromJson()
    {
        UserCollectionAuxiliar userCollectionAuxiliar = helperCardCollectionJsonKimboko.GetUserCollectionFromJson();
        quantityOfCardsUserHaveFromBDOnline = userCollectionAuxiliar.quantityOfCardsUserHaveFromBDOnline;
        gameMenuManager.StartCoroutine(WaitForLoading(userCollectionAuxiliar.dfCollection));
    }

    public List<DefaultCollectionDataDB> GetUserCollectionFromJsonTOERASE()
    {
        UserCollectionAuxiliar userCollectionAuxiliar = helperCardCollectionJsonKimboko.GetUserCollectionFromJson();
        return userCollectionAuxiliar.dfCollection;
    }

    public async Task<bool> AddNewCardToUserCollection(CardData pCardData, UserDB pUserDB)
    {
        string cardID = "CardID" + pCardData.ID;
        DefaultCollectionDataDB dfColl = new DefaultCollectionDataDB(cardID, 1);
        bool isLoaded = await helperUserCardCollectionFirebaseKimboko.AddNewCardToUserCollection(dfColl, pUserDB);
        return isLoaded;
    }

    public void RestCardAmountFromCardCollection(DefaultCollectionDataDB pCardData, UserDB pUserDB)
    {
        helperUserCardCollectionFirebaseKimboko.RestCardAmountFromCardCollection(pCardData, pUserDB);
    }

    public CardData[] GetAllCardDataArray()
    {
        return allCardsDataArray;
    }

    public Dictionary<string, CardDataRT> GetGameCollectionDictionary()
    {
        return cardCollectionLibraryFromBDOnline;
    }

    public Dictionary<string, int> GetUserCollectionDictionary()
    {
        return quantityOfCardsUserHaveFromBDOnline;
    }

    public void AddCardToGameCollectionDictionary(CardData cardDataAux)
    {
        // add this card to your collection. 
        if (quantityOfCardsUserHaveFromBDOnline.ContainsKey("CardID" + cardDataAux.ID))
        {
            quantityOfCardsUserHaveFromBDOnline["CardID" + cardDataAux.ID]++;
        }
        else
        {
            quantityOfCardsUserHaveFromBDOnline.Add("CardID" + cardDataAux.ID, 1);
        }
    }

    public int GetUserCollectionAmountByCardID(string cardID)
    {
        int amount = 0;

        if (quantityOfCardsUserHaveFromBDOnline.ContainsKey(cardID))
        {
            amount = quantityOfCardsUserHaveFromBDOnline[cardID];
        }

        return amount;
    }

    public CardData GetCardDataByCardID(string cardID)
    {
        if (cardCollectionLibraryFromBDOnline.ContainsKey(cardID))
        {
            CardData cDat = new CardData(cardCollectionLibraryFromBDOnline[cardID]);
            return cDat;
        }
        return null;
    }

}