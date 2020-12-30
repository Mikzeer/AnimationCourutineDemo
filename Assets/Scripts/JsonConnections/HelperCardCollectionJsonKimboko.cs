using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class HelperCardCollectionJsonKimboko
{
    private string jsonSavePath = Path.Combine("Assets/", "Resources/");   
    private string userCollectionFile = "UserCollection.json";
    private string gameCollectionFile = "GameCollection.json";
    private string lastCollectionUpdateFile = "lastcollectionupdate.json";
    private string cardLimitDataFile = "CardLimitData.json";
    private string deckIDFile = "DeckID.json";
    private string userDecksFile = "UserDecks.json";
    private string configurationDataFile = "ConfigurationData.json";
    private bool debugOn = false;

    public void SetConfigurationDataToJson(ConfigurationData configurationData)
    {
        CheckAndSave(CreateJsonStringFromObject(configurationData), configurationDataFile);
    }

    public ConfigurationData GetConfigurationDataFromJson()
    {
        ConfigurationData configurationData = new ConfigurationData();
        if (SaveLoadDirectoryExist() && FileExist(configurationDataFile))
        {
            string json = ReadStringFromJson(configurationDataFile);
            JsonUtility.FromJsonOverwrite(json, configurationData);
            if (debugOn) Debug.Log("CONFIGURATION DATA LOADED FROM JSON");
        }
        return configurationData;
    }

    public void SetUserCollectionToJson(List<DefaultCollectionDataDB> dfCollection)
    {
        DefaultCollectionDataDBList dfList = new DefaultCollectionDataDBList(dfCollection);
        CheckAndSave(CreateJsonStringFromObject(dfList), userCollectionFile);
    }

    public void SetGameCollectionToJson(List<CardDataRT> cardData)
    {
        CardDataRTList cDataList = new CardDataRTList(cardData);
        CheckAndSave(CreateJsonStringFromObject(cDataList), gameCollectionFile);
    }

    public void SetLastUserCollectionUpdateToJson(long uctCreatedUnix)
    {
        //if (debugOn) Debug.Log("LAST USER UPDATE TO JSON " + uctCreatedUnix);

        LastUpdateAuxiliar dateAux = new LastUpdateAuxiliar(uctCreatedUnix);
        CheckAndSave(CreateJsonStringFromObject(dateAux), lastCollectionUpdateFile);
    }

    public void SetCardLimitToJson(CardDataLimit cardDataLimit)
    {
        CheckAndSave(CreateJsonStringFromObject(cardDataLimit), cardLimitDataFile);
    }

    public void SetDeckIDToJson(DeckID cDataList)
    {
        CheckAndSave(CreateJsonStringFromObject(cDataList), deckIDFile);
    }

    public int GetDeckIDFromJson()
    {
        int ID = 1;
        if (SaveLoadDirectoryExist() && FileExist(deckIDFile))
        {
            string json = ReadStringFromJson(deckIDFile);
            DeckID deckID = new DeckID();
            JsonUtility.FromJsonOverwrite(json, deckID);
            ID = deckID.lastDeckID + 1;

            if (debugOn) Debug.Log("LAST DECK ID LOADED FROM JSON");
        }
        return ID;
    }

    public void SetUserDecks(UsersDecks usersDecks)
    {
        CheckAndSave(CreateJsonStringFromObject(usersDecks), userDecksFile);
    }

    public UsersDecks GetUserDecksFromJson()
    {
        UsersDecks userDecks = new UsersDecks();
        if (SaveLoadDirectoryExist() && FileExist(userDecksFile))
        {
            string json = ReadStringFromJson(userDecksFile);
            JsonUtility.FromJsonOverwrite(json, userDecks);
            if (debugOn) Debug.Log("USER DECKS LOADED FROM JSON");
        }
        return userDecks;
    }

    public GameCollectionAuxiliar GetGameCollectionFromJson()
    {
        Dictionary<string, CardDataRT> cardCollectionLibraryFromBDOnline = new Dictionary<string, CardDataRT>();
        if (SaveLoadDirectoryExist() && FileExist(gameCollectionFile))
        {
            string json = ReadStringFromJson(gameCollectionFile);
            CardDataRTList collDbList = new CardDataRTList();
            JsonUtility.FromJsonOverwrite(json, collDbList);

            List<CardDataRT> cardData = new List<CardDataRT>();
            List<CardData> cDat = new List<CardData>();
            foreach (CardDataRT data in collDbList.cardDataList)
            {
                if (!cardCollectionLibraryFromBDOnline.ContainsKey("CardID" + data.ID))
                    cardCollectionLibraryFromBDOnline.Add("CardID" + data.ID, data);

                cardData.Add(data);

                CardData cAux = new CardData(data);
                cDat.Add(cAux);
            }
            CardData[] allCardsDataArray = cDat.ToArray();

            GameCollectionAuxiliar gameCollectionAuxiliar = new GameCollectionAuxiliar(cardData, cDat, cardCollectionLibraryFromBDOnline, allCardsDataArray);


            if (debugOn) Debug.Log("GAME CARD COLLECTION LOADED FROM JSON");

            return gameCollectionAuxiliar;
        }
        return null;
    }

    public UserCollectionAuxiliar GetUserCollectionFromJson()
    {
        Dictionary<string, int> quantityOfCardsUserHaveFromBDOnline = new Dictionary<string, int>();
        if (SaveLoadDirectoryExist() && FileExist(userCollectionFile))
        {
            string json = ReadStringFromJson(userCollectionFile);
            DefaultCollectionDataDBList collDbList = new DefaultCollectionDataDBList();
            JsonUtility.FromJsonOverwrite(json, collDbList);

            List<DefaultCollectionDataDB> dfCollection = new List<DefaultCollectionDataDB>();
            foreach (DefaultCollectionDataDB data in collDbList.dfCollection)
            {
                if (!quantityOfCardsUserHaveFromBDOnline.ContainsKey(data.ID))
                    quantityOfCardsUserHaveFromBDOnline.Add(data.ID, data.Amount);

                dfCollection.Add(data);
            }

            UserCollectionAuxiliar userCollectionAuxiliar = new UserCollectionAuxiliar(dfCollection, quantityOfCardsUserHaveFromBDOnline);

            if (debugOn) Debug.Log("USER CARD COLLECTION LOADED FROM JSON");

            return userCollectionAuxiliar;
        }
        return null;
    }

    public long GetLastUserCollectionUpdateFromJsonLong()
    {
        long lastUpd = 0;
        if (SaveLoadDirectoryExist() && FileExist(lastCollectionUpdateFile))
        {
            string json = ReadStringFromJson(lastCollectionUpdateFile);
            LastUpdateAuxiliar dateAux2 = new LastUpdateAuxiliar();
            JsonUtility.FromJsonOverwrite(json, dateAux2);
            lastUpd = dateAux2.uctCreatedUnix;

            if (debugOn) Debug.Log("LAST COLLECTION UPDATE LOADED FROM JSON");
        }
        return lastUpd;
    }

    public CardDataLimit GetCardLimitDataFromJson()
    {
        CardDataLimit cardLimitData = new CardDataLimit();
        if (SaveLoadDirectoryExist() && FileExist(cardLimitDataFile))
        {
            string json = ReadStringFromJson(cardLimitDataFile);
            JsonUtility.FromJsonOverwrite(json, cardLimitData);
            if (debugOn) Debug.Log("CARD LIMIT DATA LOADED FROM JSON");
        }
        return cardLimitData;
    }

    private string CreateJsonStringFromObject(object obj)
    {
        string jsonSave = JsonUtility.ToJson(obj, true);//true for you can read the file
        return jsonSave;
    }

    private string ReadStringFromJson(string fileName)
    {
        string path = jsonSavePath;
        path += fileName;
        string json = File.ReadAllText(path);

        return json;
    }

    private void CheckAndSave(string jsonSave, string fileName)
    {
        if (SaveLoadDirectoryExist() == false)
        {
            CreateSaveLoadDirectory();
        }

        if (FileExist(fileName) == false)
        {
            CreateFile(fileName);
        }

        SaveJsonInFile(jsonSave, fileName);
    }

    private bool SaveLoadDirectoryExist()
    {
        return Directory.Exists(jsonSavePath);
    }

    private bool FileExist(string fileName)
    {
        string path = jsonSavePath;
        path += fileName;
        return File.Exists(path);
    }

    private void CreateSaveLoadDirectory()
    {
        Directory.CreateDirectory(jsonSavePath);
    }

    private void CreateFile(string fileName)
    {
        string path = jsonSavePath;
        path += fileName;
        FileStream fileStream = new FileStream(fileName,
                   FileMode.OpenOrCreate,
                   FileAccess.ReadWrite,
                   FileShare.None);

        using (fileStream = File.Create(path))
        {
            //string jsonSave = JsonUtility.ToJson(dfList, true);//true for you can read the file
            //File.WriteAllText(path, jsonSave);
        }
    }

    private void SaveJsonInFile(string jsonSave, string fileName)
    {
        string path = jsonSavePath;
        path += fileName;
        File.WriteAllText(path, jsonSave);
    }
}