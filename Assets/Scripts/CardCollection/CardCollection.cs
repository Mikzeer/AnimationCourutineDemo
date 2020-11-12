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

        UserDB user = new UserDB("mmm", "", "", "");
        LoadUserCollectionFromFirebase(user);
        //Debug.Log("PEPE");
    }

    public async void CreateNewUserCollections(UserDB pUser)
    {
        List<DefaultCollectionDataDB> dfCollection = await cardCollectionFirebase.CreateNewUserCollection(pUser);

        foreach (DefaultCollectionDataDB ca in dfCollection)
        {
            if (!quantityOfCardsUserHaveFromBDOnline.ContainsKey(ca.ID))
                quantityOfCardsUserHaveFromBDOnline.Add(ca.ID, ca.Amount);
        }

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
        Debug.Log("GAME CARD COLLECTION LOADED");
    }

    public async void LoadUserCollectionFromFirebase(UserDB pUser)
    {
        List<DefaultCollectionDataDB> dfCollection = await cardCollectionFirebase.GetUserCardCollection(pUser);

        foreach (DefaultCollectionDataDB ca in dfCollection)
        {
            if (!quantityOfCardsUserHaveFromBDOnline.ContainsKey(ca.ID))
                quantityOfCardsUserHaveFromBDOnline.Add(ca.ID, ca.Amount);
        }
        Pito();
    }

    private void Pito()
    {
        Debug.Log("PEPE " + quantityOfCardsUserHaveFromBDOnline.Count);
        Debug.Log("USER CARD COLLECTION LOADED");
    }

    private async void LoadCollections(UserDB pUser)
    {
        // Local Time
        DateTime now = DateTime.Now;
        Debug.Log($"Local time {now:HH:mm:ss}");
        
        //One global time helps to avoid confusion about time zones and daylight saving time. The UTC (Universal Coordinated time)
        DateTime utc = DateTime.UtcNow;
        Debug.Log($"UTC time {utc:HH:mm:ss}");


        // CHEQUEAR LA ULTIMA ACTUALIZACION DE LA BASE DE DATOS CONTRA LA ULTIMA ACTUALIZACION DEL JUGADOR
        long bdLastUpdate = await cardCollectionFirebase.GetLastGameCardCollectionDownloadTimestampUser(pUser.Name.ToLower());
        long jsonLastUpdate = GetLastGameCollectionUpdateFromJsonLong();

        Debug.Log("bdLastUpdate " + bdLastUpdate);
        Debug.Log("jsonLastUpdate " + jsonLastUpdate);

        DateTime dtBD = Helper.UnixTimeStampToDateTimeMiliseconds(bdLastUpdate);
        DateTime dtJson = Helper.UnixTimeStampToDateTimeMiliseconds(jsonLastUpdate);

        Debug.Log("dtBD " + dtBD);
        Debug.Log("dtJson " + dtJson);

        int dtComp = DateTime.Compare(dtBD, dtJson);

        // ACA ESTOY HACIENDO MAL, YA QUE LA FECHA DE ULTIMA ACTUALIZACION DE LA BASE DE DATOS ES UNA
        // Y LA DE CADA USUARIO ES PARTICULAR A CADA USUARIO... ENTONCES NO HACE FALTA GUARDAR UN JSON EN LA COMPU
        // YA QUE EL USUARIO CUANDO SE DESCARGA LA GAME CARD COLLECTION ACTUALIZA EN SU REGISTRO ONLINE LA ULTIMA FECHA DE UPDATE
        // Y LA BASE DE DATOS ONLINE TIENE UN SOLO REGISTRO PARA GUARDAR LA ULTIMA ACTUALIZACION DE LA GAME CARD COLLECTION
        // ENTONCES LO QUE HAGO ES REVISAR LA INFORMACION EN LA BD QUE ESTA GUARDADA EN EL USUARIO Y LA QUE ESTA GUARDAD EN EL GENERAL DE LA BD
        // SI LA BASE DE DATOS GENERAL ACTUALIZO Y EL USUARIO NO, ENTONCES EL DATO VA A ESTAR GUARDADO
        // UNA VEZ QUE ACTUALICE EL USUARIO ESTE SUBE A LA BASE DE DATOS LA FECHA QUE ACTUALIZO QUE VA A SER MAYOR A LA DE LA BD SIEMPRE

        // MIENTRAS QUE LA FECHA DE ACTUALIZACION DEL USUARIO SEA MAYOR A LA QUE ESTA EN EL GENERAL DE LA BD SIGNIFICA QUE EL USUARIO ESTA UPDATEADO
        // ENTONCES NO HACE FALTA DESCARGAR LA CARD COLLECTION DE LA BD ONLINE, SOLO LA LEEMOS DESDE EL JSON GUARDADO SIEMPRE Y CUANDO NO SEA NULO

        switch (dtComp)
        {
            case -1:
                //date1 is earlier than date2.
                // ACA ESTA TODO MAL
                break;
            case 0:
                //date1 is the same as date2.
                // ACA NO HACE FALTA ACTUALIZAR DE LA BD Y SOLO CARGAMOS DESDE EL JSON GUARDADO
                break;
            case 1:
                // If date1 is later than date2.
                // ACA HAY UNA ACTUALIZACION Y ENTONCES TENEMOS QUE CARGARLO DESDE LA BD ONLINE
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

[Serializable]
public class DefaultCollectionDataDBList
{
    public List<DefaultCollectionDataDB> dfCollection;

    public DefaultCollectionDataDBList()
    {
        dfCollection = new List<DefaultCollectionDataDB>();
    }

    public DefaultCollectionDataDBList(List<DefaultCollectionDataDB> dfCollection)
    {
        this.dfCollection = dfCollection;
    }
}

[Serializable]
public class CardDataRTList
{
    public List<CardDataRT> cardDataList;

    public CardDataRTList()
    {
        cardDataList = new List<CardDataRT>();
    }

    public CardDataRTList(List<CardDataRT> dfCollection)
    {
        this.cardDataList = dfCollection;
    }
}