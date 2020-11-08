using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PositionerDemo;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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


    private Dictionary<string, CardDataRT> cardCollectionLibraryFromBDOnline = new Dictionary<string, CardDataRT>(); // 
    public Dictionary<int, int> quantityOfCardsUserHaveFromBDOnline = new Dictionary<int, int>(); // id / amount

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
        //LoadCardLibraryFromBDOnline();
    }

    private void SaveWithJSon()
    {
        string json = JsonUtility.ToJson(this, true);//true for you can read the file
        string path = Path.Combine(Application.persistentDataPath, "saved files", "data.json");
        File.WriteAllText(path, json);
    }

    [Serializable]
    public class aux
    {
        public long uctCreatedUnix;

        public aux()
        {

        }

        public aux(long uctCreatedUnix)
        {
            this.uctCreatedUnix = uctCreatedUnix;
        }
    }

    public DateTime LastCollectionUpdate()
    {
        DateTime dtLastUpdate = DateTime.Today;

        long uctCreatedUnix = 1604766896680;

        aux dateAux = new aux(uctCreatedUnix);

        //string path = Path.Combine(Application.persistentDataPath, "Resources", "lastcollectionupdate.json");
        string path = Path.Combine("Assets/", "Resources/", "lastcollectionupdate.json");


        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }


        string jsonSave = JsonUtility.ToJson(dateAux, true);//true for you can read the file
        File.WriteAllText(path, jsonSave);

        string json = File.ReadAllText(path);

        aux dateAux2 = new aux();

        JsonUtility.FromJsonOverwrite(json, dateAux2);

        dtLastUpdate = UnixTimeStampToDateTimeMiliseconds(dateAux2.uctCreatedUnix);

        Debug.Log("CREATED DATE " + dtLastUpdate);

        return dtLastUpdate;
    }

    private static DateTime UnixTimeStampToDateTimeMiliseconds(long unixTimeStamp)
    {
        DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp);
        return dtDateTime;
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

    private async void LoadCardLibraryFromBDOnline()
    {
        List<CardDataRT> cardData = await DatosFirebaseRTHelper.Instance.GetAllCardCollectionLibrary();
        foreach (CardDataRT ca in cardData)
        {
            if (!cardCollectionLibraryFromBDOnline.ContainsKey(ca.CardName))
                cardCollectionLibraryFromBDOnline.Add(ca.CardName, ca);
        }

        ShowCarD();
    }

    private void ShowCarD()
    {
        Debug.Log(cardCollectionLibraryFromBDOnline.ToString());

        foreach (KeyValuePair<string,CardDataRT> item in cardCollectionLibraryFromBDOnline)
        {
            Debug.Log(item.Value.frontImageBytes.ToString());
            Sprite sp = DatosFirebaseRTHelper.Instance.GetSpriteFromByteArray(item.Value.frontImageBytes.ToArray());
            DatosFirebaseRTHelper.Instance.pruebaSprite.sprite = sp;
            break;
        }              
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

    private void SaveWithBinaryFormatter()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");
        bf.Serialize(file, QuantityOfEachCard);
        file.Close();
    }

    private void LoadWithBianaryFormatter()
    {
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);

            QuantityOfEachCard = (Dictionary<CardAsset, int>)bf.Deserialize(file);
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