using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PositionerDemo;
using System.Linq;
using System.IO;
using System;
using System.Threading.Tasks;

public class CardCollection : MonoBehaviour
{
    #region SINGLETON

    [SerializeField] protected bool dontDestroy;
    private static CardCollection instance;
    public static CardCollection Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CardCollection>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    instance = obj.AddComponent<CardCollection>();
                }
            }
            return instance;
        }
    }

    #endregion

    private CardAsset[] allCardsArray; // ALL THE CARDS THAT EXIST IN THE GAME
    private Dictionary<string, CardAsset> AllCardsDictionary = new Dictionary<string, CardAsset>(); // ALL THE CARDS THAT EXIST IN THE GAME BUT IN A DICTIONARY TO FIND BY NAME
    public Dictionary<CardAsset, int> QuantityOfEachCard = new Dictionary<CardAsset, int>(); // HOW MUCH OF EACH CARD DOES THE PLAYER HAS IN HIS LIBRARY


    [SerializeField] private CardCollectionFirebase cardCollectionFirebase;
    private Dictionary<string, CardDataRT> cardCollectionLibraryFromBDOnline = new Dictionary<string, CardDataRT>(); // id // carddata
    public Dictionary<string, int> quantityOfCardsUserHaveFromBDOnline = new Dictionary<string, int>(); // id / amount

    void Awake()
    {
        if (instance == null)
        {
            instance = this as CardCollection;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
        LoadCardsArrays();
    }

    private void Start()
    {
        //List<DefaultCollectionDataDB> cd = await cardCollectionFirebase.GetAndSetDefaultCardCollection();
        //SetUserCollectionToJson(cd);

        //List<CardDataRT> cdata = await cardCollectionFirebase.GetGameCardCollection();
        //SetGameCollectionToJson(cdata);

        //List<DefaultCollectionDataDB> cd = GetUserCollectionFromJson();
        //Debug.Log("PEPE");

        //List<CardDataRT> cdata = GetGameCollectionFromJson();
        //Debug.Log("PEPE");

        //UserDB user = new UserDB("mmm", "", "", "");
        //LoadUserCollectionFromFirebase(user);
        //Debug.Log("PEPE");

        //cardCollectionFirebase.UpdateLastGameCardCollectionUpdateTOERASELATERJUSTTOTEST();

        //CardDatabase.GetCardFiltterSubClassByReflection();
    }

    public async void CreateNewUserCollections(UserDB pUser)
    {
        List<DefaultCollectionDataDB> dfCollection = await cardCollectionFirebase.CreateNewUserCollection(pUser);

        foreach (DefaultCollectionDataDB ca in dfCollection)
        {
            if (!quantityOfCardsUserHaveFromBDOnline.ContainsKey(ca.ID))
                quantityOfCardsUserHaveFromBDOnline.Add(ca.ID, ca.Amount);
        }

        SetUserCollectionToJson(dfCollection);

        LoadGameCollectionFromFirebase(pUser);
    }

    public async void LoadGameCollectionFromFirebase(UserDB pUser)
    {
        List<CardDataRT> cardData = await cardCollectionFirebase.GetGameCardCollection(pUser);
        foreach (CardDataRT ca in cardData)
        {
            if (!cardCollectionLibraryFromBDOnline.ContainsKey(ca.CardName))
                cardCollectionLibraryFromBDOnline.Add(ca.CardName, ca);
        }


        SetGameCollectionToJson(cardData);

        Debug.Log("GAME CARD COLLECTION LOADED FROM DB ONLINE");
    }

    public void LoadGameCollectionFromJson()
    {
        List<CardDataRT> cardData = GetGameCollectionFromJson();
        foreach (CardDataRT ca in cardData)
        {
            if (!cardCollectionLibraryFromBDOnline.ContainsKey(ca.CardName))
                cardCollectionLibraryFromBDOnline.Add(ca.CardName, ca);
        }
        Debug.Log("GAME CARD COLLECTION LOADED FROM JSON");
    }

    public async void LoadUserCollectionFromFirebase(UserDB pUser)
    {
        List<DefaultCollectionDataDB> dfCollection = await cardCollectionFirebase.GetUserCardCollection(pUser);

        foreach (DefaultCollectionDataDB ca in dfCollection)
        {
            if (!quantityOfCardsUserHaveFromBDOnline.ContainsKey(ca.ID))
                quantityOfCardsUserHaveFromBDOnline.Add(ca.ID, ca.Amount);
        }
        Debug.Log("USER CARD COLLECTION LOADED FROM DB ONLINE");
    }

    public void LoadUserCollectionFromJson()
    {
        List<DefaultCollectionDataDB> dfCollection = GetUserCollectionFromJson();

        foreach (DefaultCollectionDataDB ca in dfCollection)
        {
            if (!quantityOfCardsUserHaveFromBDOnline.ContainsKey(ca.ID))
                quantityOfCardsUserHaveFromBDOnline.Add(ca.ID, ca.Amount);
        }
        Debug.Log("USER CARD COLLECTION LOADED FROM JSON");
    }

    private void LoadCollections(UserDB pUser)
    {
        CheckLastGameCollectionUpdate(pUser);
        CheckLastUserCollectionUpdate(pUser);

        //// Local Time
        //DateTime now = DateTime.Now;
        //Debug.Log($"Local time {now:HH:mm:ss}");        
        ////One global time helps to avoid confusion about time zones and daylight saving time. The UTC (Universal Coordinated time)
        //DateTime utc = DateTime.UtcNow;
        //Debug.Log($"UTC time {utc:HH:mm:ss}");

        //// CHEQUEAR LA ULTIMA ACTUALIZACION DE LA BASE DE DATOS CONTRA LA ULTIMA ACTUALIZACION DEL JUGADOR
        //long bdLastGameCollectionUpdate = await cardCollectionFirebase.GetLastGameCardCollectionUpdateTimestamp();
        //long bdLastGameCollectionDownloadByUser = await cardCollectionFirebase.GetLastGameCardCollectionDownloadTimestampUser(pUser.Name.ToLower());

        //Debug.Log("bdLastGameCollectionUpdate " + bdLastGameCollectionUpdate);
        //Debug.Log("bdLastGameCollectionDownloadByUser " + bdLastGameCollectionDownloadByUser);

        //DateTime dtLGCU = Helper.UnixTimeStampToDateTimeMiliseconds(bdLastGameCollectionUpdate);
        //DateTime dtLGCDBU = Helper.UnixTimeStampToDateTimeMiliseconds(bdLastGameCollectionDownloadByUser);

        //Debug.Log("dtLGCU " + dtLGCU);
        //Debug.Log("dtLGCDBU " + dtLGCDBU);

        //int dtCompareGameCollection = DateTime.Compare(dtLGCU, dtLGCDBU);

        //switch (dtCompareGameCollection)
        //{
        //    case -1:
        //        //date1 is earlier than date2.
        //        // EL JUGADOR TIENE LA ULTIMA ACTUALIZACION DE LA CARD COLLECTION
        //        LoadGameCollectionFromJson();
        //        break;
        //    case 0:
        //        //date1 is the same as date2.
        //        // EL JUGADOR TIENE LA ULTIMA ACTUALIZACION DE LA CARD COLLECTION MUY RARO ESTO PERO PUEDE SER...
        //        LoadGameCollectionFromJson();
        //        break;
        //    case 1:
        //        // If date1 is later than date2.
        //        // ACA HAY UNA ACTUALIZACION Y ENTONCES TENEMOS QUE CARGARLO DESDE LA BD ONLINE
        //        LoadGameCollectionFromFirebase(pUser);
        //        break;
        //    default:
        //        break;
        //}
    }

    private async void CheckLastGameCollectionUpdate(UserDB pUser)
    {
        long bdLastGameCollectionUpdate = await cardCollectionFirebase.GetLastGameCardCollectionUpdateTimestamp();
        long bdLastGameCollectionDownloadByUser = await cardCollectionFirebase.GetLastGameCardCollectionDownloadTimestampUser(pUser.Name.ToLower());

        Debug.Log("bdLastGameCollectionUpdate " + bdLastGameCollectionUpdate);
        Debug.Log("bdLastGameCollectionDownloadByUser " + bdLastGameCollectionDownloadByUser);

        DateTime dtLGCU = Helper.UnixTimeStampToDateTimeMiliseconds(bdLastGameCollectionUpdate);
        DateTime dtLGCDBU = Helper.UnixTimeStampToDateTimeMiliseconds(bdLastGameCollectionDownloadByUser);

        Debug.Log("dtLGCU " + dtLGCU);
        Debug.Log("dtLGCDBU " + dtLGCDBU);

        int dtCompareGameCollection = DateTime.Compare(dtLGCU, dtLGCDBU);

        switch (dtCompareGameCollection)
        {
            case -1:
                //date1 is earlier than date2.
                // EL JUGADOR TIENE LA ULTIMA ACTUALIZACION DE LA CARD COLLECTION
                LoadGameCollectionFromJson();
                break;
            case 0:
                //date1 is the same as date2.
                // EL JUGADOR TIENE LA ULTIMA ACTUALIZACION DE LA CARD COLLECTION MUY RARO ESTO PERO PUEDE SER...
                LoadGameCollectionFromJson();
                break;
            case 1:
                // If date1 is later than date2.
                // ACA HAY UNA ACTUALIZACION Y ENTONCES TENEMOS QUE CARGARLO DESDE LA BD ONLINE
                LoadGameCollectionFromFirebase(pUser);
                break;
            default:
                break;
        }
    }

    private async void CheckLastUserCollectionUpdate(UserDB pUser)
    {
        long bdLastUserCollectionDownloadByUserJson = GetLastUserCollectionUpdateFromJsonLong();
        long bdLastUserCollectionDownloadByUser = await cardCollectionFirebase.GetLastUserCardCollectionDownloadTimestampUser(pUser.Name.ToLower());

        Debug.Log("bdLastUserCollectionDownloadByUserJson " + bdLastUserCollectionDownloadByUserJson);
        Debug.Log("bdLastUserCollectionDownloadByUser " + bdLastUserCollectionDownloadByUser);

        DateTime dtJson = Helper.UnixTimeStampToDateTimeMiliseconds(bdLastUserCollectionDownloadByUserJson);
        DateTime dtBD = Helper.UnixTimeStampToDateTimeMiliseconds(bdLastUserCollectionDownloadByUser);

        Debug.Log("dtJson " + dtJson);
        Debug.Log("dtBD " + dtBD);

        int dtCompareUserCollection = DateTime.Compare(dtBD, dtJson);

        // no solo tengo que verificar cuando fue la ultima vez que se la descargo, sino tambien , cuando fue la ultima vez que la lista se updateo

        long bdLastUserCollectionModification = await cardCollectionFirebase.GetLastUserCardCollectioModificationTimestampUser(pUser.Name.ToLower());
        DateTime dtLastMod = Helper.UnixTimeStampToDateTimeMiliseconds(bdLastUserCollectionModification);
        // ultima modificacion bd hoy
        // ultima descarga  bd ayer
        // ultima descarga json ayer

        int dtCompareLastModification = DateTime.Compare(dtLastMod, dtBD);


        switch (dtCompareUserCollection)
        {
            case -1:
                //date1 is earlier than date2.
                // EL JUGADOR TIENE LA ULTIMA ACTUALIZACION DE LA CARD COLLECTION
                LoadUserCollectionFromJson();
                break;
            case 0:
                //date1 is the same as date2.
                // EL JUGADOR TIENE LA ULTIMA ACTUALIZACION DE LA CARD COLLECTION MUY RARO ESTO PERO PUEDE SER...
                LoadUserCollectionFromJson();
                break;
            case 1:
                // If date1 is later than date2.
                // ACA HAY UNA ACTUALIZACION Y ENTONCES TENEMOS QUE CARGARLO DESDE LA BD ONLINE
                LoadUserCollectionFromFirebase(pUser);
                break;
            default:
                break;
        }
    }

    private void SetGameCollectionToJson(List<CardDataRT> cardData)
    {
        CardDataRTList cDataList = new CardDataRTList(cardData);

        string path = Path.Combine("Assets/", "Resources/");
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);

            path += "GameCollection.json";

            if (File.Exists(path))
            {
                string jsonSave = JsonUtility.ToJson(cDataList, true);//true for you can read the file
                File.WriteAllText(path, jsonSave);
            }
            else
            {
                FileStream fileStream = new FileStream(@"GameCollection.json",
                                       FileMode.OpenOrCreate,
                                       FileAccess.ReadWrite,
                                       FileShare.None);

                using (fileStream = File.Create(path))
                {
                    string jsonSave = JsonUtility.ToJson(cDataList, true);//true for you can read the file
                    File.WriteAllText(path, jsonSave);
                }
            }
        }
        else
        {
            path += "GameCollection.json";

            if (File.Exists(path))
            {
                string jsonSave = JsonUtility.ToJson(cDataList, true);//true for you can read the file
                File.WriteAllText(path, jsonSave);
            }
            else
            {
                FileStream fileStream = new FileStream(@"GameCollection.json",
                                       FileMode.OpenOrCreate,
                                       FileAccess.ReadWrite,
                                       FileShare.None);

                using (fileStream = File.Create(path))
                {
                    string jsonSave = JsonUtility.ToJson(cDataList, true);//true for you can read the file
                    File.WriteAllText(path, jsonSave);
                }
            }
        }
    }

    private void SetUserCollectionToJson(List<DefaultCollectionDataDB> dfCollection)
    {
        DefaultCollectionDataDBList dfList = new DefaultCollectionDataDBList(dfCollection);

        string path = Path.Combine("Assets/", "Resources/");
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);

            path += "UserCollection.json";

            if (File.Exists(path))
            {
                string jsonSave = JsonUtility.ToJson(dfList, true);//true for you can read the file
                File.WriteAllText(path, jsonSave);
            }
            else
            {
                FileStream fileStream = new FileStream(@"UserCollection.json",
                                       FileMode.OpenOrCreate,
                                       FileAccess.ReadWrite,
                                       FileShare.None);

                using (fileStream = File.Create(path))
                {
                    string jsonSave = JsonUtility.ToJson(dfList, true);//true for you can read the file
                    File.WriteAllText(path, jsonSave);
                }
            }
        }
        else
        {
            path += "UserCollection.json";

            if (File.Exists(path))
            {
                string jsonSave = JsonUtility.ToJson(dfList, true);//true for you can read the file
                File.WriteAllText(path, jsonSave);
            }
            else
            {
                FileStream fileStream = new FileStream(@"UserCollection.json",
                                       FileMode.OpenOrCreate,
                                       FileAccess.ReadWrite,
                                       FileShare.None);

                using (fileStream = File.Create(path))
                {
                    string jsonSave = JsonUtility.ToJson(dfList, true);//true for you can read the file
                    File.WriteAllText(path, jsonSave);
                }
            }
        }
    }

    private List<DefaultCollectionDataDB> GetUserCollectionFromJson()
    {
        List<DefaultCollectionDataDB> userCollection = new List<DefaultCollectionDataDB>();


        string path = Path.Combine("Assets/", "Resources/");
        if (!Directory.Exists(path))
        {
            return userCollection;
        }
        else
        {
            path += "UserCollection.json";

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);

                DefaultCollectionDataDBList collDbList = new DefaultCollectionDataDBList();

                JsonUtility.FromJsonOverwrite(json, collDbList);

                foreach (DefaultCollectionDataDB data in collDbList.dfCollection)
                {
                    userCollection.Add(data);
                }

                return userCollection;
            }
            else
            {
                return userCollection;
            }
        }
    }

    private List<CardDataRT> GetGameCollectionFromJson()
    {
        List<CardDataRT> gameCollection = new List<CardDataRT>();


        string path = Path.Combine("Assets/", "Resources/");
        if (!Directory.Exists(path))
        {
            return gameCollection;
        }
        else
        {
            path += "GameCollection.json";

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);

                CardDataRTList collDbList = new CardDataRTList();

                JsonUtility.FromJsonOverwrite(json, collDbList);

                foreach (CardDataRT data in collDbList.cardDataList)
                {
                    gameCollection.Add(data);
                }

                return gameCollection;
            }
            else
            {
                return gameCollection;
            }
        }
    }

    public CardDataRT GetCardDataRTByID(string cardID)
    {
        if (cardCollectionLibraryFromBDOnline.ContainsKey(cardID))
        {
            return cardCollectionLibraryFromBDOnline[cardID];
        }

        return null;
    }

    public void SetLastUserCollectionUpdateToJson(long uctCreatedUnix)
    {
        LastUpdateAuxiliar dateAux = new LastUpdateAuxiliar(uctCreatedUnix);

        string path = Path.Combine("Assets/", "Resources/");
        if (!Directory.Exists(path))
        {
            return;
        }
        else
        {
            path += "lastusercollectionupdate.json";

            if (File.Exists(path))
            {
                string jsonSave = JsonUtility.ToJson(dateAux, true);//true for you can read the file
                File.WriteAllText(path, jsonSave);
            }
            else
            {
                return;
            }
        }
    }

    public long GetLastUserCollectionUpdateFromJsonLong()
    {
        string path = Path.Combine("Assets/", "Resources/");
        if (!Directory.Exists(path))
        {
            return 0;
        }
        else
        {
            path += "lastusercollectionupdate.json";

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);

                LastUpdateAuxiliar dateAux2 = new LastUpdateAuxiliar();

                JsonUtility.FromJsonOverwrite(json, dateAux2);

                return dateAux2.uctCreatedUnix;
            }
            else
            {
                return 0;
            }
        }
    }

    public void AddNewCardToUserCollection(CardData pCardData, UserDB pUserDB)
    {
        string cardID = "CardID" + pCardData.ID;
        DefaultCollectionDataDB dfColl = new DefaultCollectionDataDB(cardID, 1);
        cardCollectionFirebase.SetNewCardToUserCardCollection(dfColl, pUserDB);
    }




    public DateTime GetLastGameCollectionUpdateFromJsonDateTime()
    {
        long lastUpdUnix = GetLastGameCollectionUpdateFromJsonLong();
        DateTime dtLastUpdate = Helper.UnixTimeStampToDateTimeMiliseconds(lastUpdUnix);
        return dtLastUpdate;
    }

    public long GetLastGameCollectionUpdateFromJsonLong()
    {
        string path = Path.Combine("Assets/", "Resources/");
        if (!Directory.Exists(path))
        {
            return 0;
        }
        else
        {
            path += "lastcollectionupdate.json";

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);

                LastUpdateAuxiliar dateAux2 = new LastUpdateAuxiliar();

                JsonUtility.FromJsonOverwrite(json, dateAux2);

                return dateAux2.uctCreatedUnix;
            }
            else
            {
                return 0;
            }
        }
    }

    public void SetLastGameCollectionUpdateToJson(DateTime pLastUpdate)
    {
        long uctCreatedUnix = Helper.DateTimeToUnixTimeStampSeconds(pLastUpdate);
        SetLastGameCollectionUpdateToJson(uctCreatedUnix);
    }

    public void SetLastGameCollectionUpdateToJson(long uctCreatedUnix)
    {
        LastUpdateAuxiliar dateAux = new LastUpdateAuxiliar(uctCreatedUnix);

        string path = Path.Combine("Assets/", "Resources/");
        if (!Directory.Exists(path))
        {
            return;
        }
        else
        {
            path += "lastcollectionupdate.json";

            if (File.Exists(path))
            {
                string jsonSave = JsonUtility.ToJson(dateAux, true);//true for you can read the file
                File.WriteAllText(path, jsonSave);
            }
            else
            {
                return;
            }
        }
    }



    private void ConvertBiteArrayToSprite()
    {
        //Debug.Log(cardCollectionLibraryFromBDOnline.ToString());
        foreach (KeyValuePair<string,CardDataRT> item in cardCollectionLibraryFromBDOnline)
        {
            //Debug.Log(item.Value.frontImageBytes.ToString());
            Sprite sp = Helper.GetSpriteFromByteArray(item.Value.frontImageBytes.ToArray());
            //DatosFirebaseRTHelper.Instance.pruebaSprite.sprite = sp;
            break;
        }              
    }

    private void LoadCardsArrays()
    {
        allCardsArray = Resources.LoadAll<CardAsset>("");
        foreach (CardAsset ca in allCardsArray)
        {
            if (!AllCardsDictionary.ContainsKey(ca.name))
                AllCardsDictionary.Add(ca.name, ca);
        }

        LoadQuantityOfCardsFromPlayerPrefs();
    }

    private void LoadQuantityOfCardsFromPlayerPrefs()
    {
        foreach (CardAsset cardAsset in allCardsArray)
        {
            if (PlayerPrefs.HasKey("NumberOf" + cardAsset.CardName + cardAsset.ID))
                QuantityOfEachCard.Add(cardAsset, PlayerPrefs.GetInt("NumberOf" + cardAsset.CardName + cardAsset.ID));
            else
                QuantityOfEachCard.Add(cardAsset, 0);
        }
    }

    private void SaveQuantityOfCardsIntoPlayerPrefs()
    {
        foreach (CardAsset cardAsset in allCardsArray)
        {
            PlayerPrefs.SetInt("NumberOf" + cardAsset.CardName + cardAsset.ID, QuantityOfEachCard[cardAsset]);
        }
    }

    void OnApplicationQuit()
    {
        SaveQuantityOfCardsIntoPlayerPrefs();
    }

    public CardAsset GetCardAssetByName(string name)
    {
        if (AllCardsDictionary.ContainsKey(name))  // if there is a card with this name, return its CardAsset
            return AllCardsDictionary[name];
        else        // if there is no card with name
            return null;
    }

    public CardAsset GetCardByID(int cardID)
    {
        // initially select all cards
        var cards = from card in allCardsArray select card;

        CardAsset cardAsset = cards.Where(card => card.ID == cardID).FirstOrDefault();

        return cardAsset;
    }

    public List<CardAsset> GetCardsWithCardRarity(CardRarity rarity)
    {
        return GetCards(true, false, true, true, false, false, -1, rarity);
    }

    public List<CardAsset> GetCards(bool showingCardsPlayerDoesNotOwn = false, bool includeAllRarities = true, bool includeAllCardTypes = true, bool includeAllActivationType = true,
                                    bool isChainable = false, bool isDarkCard = false, int darkPoints = -1,
                                    CardRarity rarity = CardRarity.BASIC, CARDTYPE cardType = CARDTYPE.NEUTRAL , ACTIVATIONTYPE activationType = ACTIVATIONTYPE.HAND,string keyword = "")
    {
        // initially select all cards
        var cards = from card in allCardsArray select card;

        if (!showingCardsPlayerDoesNotOwn)
            cards = cards.Where(card => QuantityOfEachCard[card] > 0);

        if (!includeAllRarities)
            cards = cards.Where(card => card.CardRarity == rarity);

        if (!includeAllCardTypes)
            cards = cards.Where(card => card.CardType == cardType);

        if (!includeAllActivationType)
            cards = cards.Where(card => card.ActivationType == activationType);

        if (isChainable)
            cards = cards.Where(card => card.IsChainable == isChainable);

        if (isDarkCard)
            cards = cards.Where(card => card.IsDarkCard == isDarkCard);

        if (keyword != null && keyword != "" && keyword != string.Empty)
            cards = cards.Where(card => (card.CardName.ToLower().Contains(keyword.ToLower()) ||
                (card.Tags.ToLower().Contains(keyword.ToLower()) && !keyword.ToLower().Contains(" "))));

        if (darkPoints != -1)
            cards = cards.Where(card => card.DarkPoints == darkPoints);


        var returnList = cards.ToList<CardAsset>();
        returnList.Sort();

        return returnList;
    }
}
