using PositionerDemo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class GameMenuManager : MonoBehaviour
{
    [Header("PREFABS")]
    [SerializeField] private GameObject popUpBannerPrefab = default;
    [Header("MANAGERS")]
    [SerializeField] private CardCollectionVisualManager cardCollectionVisualManager = default;
    [SerializeField] private ShopManager shopManager = default;
    UserManager userManager;
    CardCollectionManager cardCollectionManager;
    UserResourcesManager userResourcesManager;
    CardLimitDataManager cardLimitDataManager;
    UIPopUpBanner uIPopUpBanner;

    void Start()
    {
        userManager = new UserManager();
        cardCollectionManager = new CardCollectionManager(this);
        userResourcesManager = new UserResourcesManager();
        cardLimitDataManager = new CardLimitDataManager();

        StartCoroutine(WaitForDatabaseToLoad());
        GameSceneManager.Instance.SetActiveWaitForLoad(true);
    }

    private IEnumerator WaitForDatabaseToLoad()
    {
        while (DatosFirebaseRTHelper.Instance.isInit == false)
        {
            Debug.Log("WAITING");
            yield return null;
        }

        Debug.Log("DB LOADED");
        //Debug.Log("CONFIGURATION DATA " + configurationData.user.Name + " IS FIRS TIME " + configurationData.user.IsFirstTime);

        HelperCardCollectionJsonKimboko helperCardCollectionJsonKimboko;
        helperCardCollectionJsonKimboko = new HelperCardCollectionJsonKimboko();
        ConfigurationData cnfDat = helperCardCollectionJsonKimboko.GetConfigurationDataFromJson();

        if (cnfDat == null)
        {
            Application.Quit();
        }

        SetUser(cnfDat.user);
    }

    private async void SetUser(UserDB pUser)
    {
        //UserDB fuser = new UserDB("mmm", "", "", "", "", false);
        userManager.SetUser(pUser);
        if (pUser.IsFirstTime == true)
        {
            cardCollectionManager.CreateNewUserCollections(pUser);
            userResourcesManager.CreateNewUserResources(pUser);
        }
        cardCollectionManager.LoadCollections(pUser);
        cardLimitDataManager.LoadCardLimitData(pUser);

        GamePricesData priceData = await LoadPriceDataFromFirebase();
        UserResources userResources = await LoadUserResourcesFromFirebase();
        shopManager.LoadPriceDataFromFirebase(priceData);
        shopManager.LoadUserResourcesFromFirebase(userResources);
    }

    public UserDB GetUser()
    {
        return userManager.GetUser();
    }

    public void LoadUserCollectionFromFirebase()
    {
        cardCollectionManager.LoadUserCollectionFromFirebase(GetUser());
    }

    public async Task<bool> AddNewCardToUserCollection(CardData pCardData)
    {
        bool isLoaded = await cardCollectionManager.AddNewCardToUserCollection(pCardData, GetUser());
        return isLoaded;
    }

    private async Task<GamePricesData> LoadPriceDataFromFirebase()
    {
        GamePricesData priceData = await userResourcesManager.LoadPriceDataFromFirebase();
        return priceData;
    }

    private async Task<UserResources> LoadUserResourcesFromFirebase()
    {
        UserResources userResources = await userResourcesManager.LoadUserResourcesFromFirebase(GetUser());
        return userResources;
    }

    public void RestOneOpenPackFromFirebase()
    {
        userResourcesManager.RestOneOpenPackFromFirebase(GetUser());
    }

    public CardData[] GetAllCardDataArray()
    {
        return cardCollectionManager.GetAllCardDataArray();
    }

    private Dictionary<string, int> GetUserCollectionDictionary()
    {
        return cardCollectionManager.GetUserCollectionDictionary();
    }

    public void AddCardToGameCollectionDictionary(CardData cardDataAux)
    {
        cardCollectionManager.AddCardToGameCollectionDictionary(cardDataAux);
        // TENGO QUE AGREGAR O REFRESCAR LA CARD VISUAL MANAGER
        cardCollectionVisualManager.AddCardSlotUI(cardDataAux);

    }

    public int GetUserCollectionAmountByCardID(string cardID)
    {
        return cardCollectionManager.GetUserCollectionAmountByCardID(cardID);
    }

    private CardData GetCardDataByCardID(string cardID)
    {
        return cardCollectionManager.GetCardDataByCardID(cardID);
    }

    public void LoadVisualCollection(CardData[] allExistingCards, Dictionary<string, int> userCollection)
    {
        cardCollectionVisualManager.CreateCollectionVisualUI(allExistingCards, userCollection);
    }

    public void LoadVisualCollection()
    {
        cardCollectionVisualManager.CreateCollectionVisualUI(cardCollectionManager.GetAllCardDataArray(), cardCollectionManager.GetUserCollectionDictionary());
    }

    public void LoadVisualDeckBuilder()
    {
        CardData[] allExistingCards = GetAllCardDataArray();
        Dictionary<string, int> userCollection = GetUserCollectionDictionary();
        cardCollectionVisualManager.CreateDeckBuilderVisualUI(allExistingCards, userCollection);
    }

    public void GenerateUserDeckFromJson(Deck deck)
    {
        deck.userDeck = new Dictionary<CardData, DefaultCollectionDataDB>();
        deck.amountPerRarity = new Dictionary<CardRarity, int>();
        for (int i = 0; i < deck.userDeckJson.Count; i++)
        {
            CardData cData = GetCardDataByCardID(deck.userDeckJson[i].ID);
            deck.userDeck.Add(cData, deck.userDeckJson[i]);
            if (deck.amountPerRarity.ContainsKey(cData.CardRarity))
            {
                deck.amountPerRarity[cData.CardRarity] += deck.userDeckJson[i].Amount;
            }
            else
            {
                deck.amountPerRarity.Add(cData.CardRarity, deck.userDeckJson[i].Amount);
            }
        }
    }

    public void GeneratePopUpBanner(string title, string description, UnityAction OnCancel, UnityAction OnConfirm)
    {
        PopUpBannerData popBD = new PopUpBannerData(title, description);
        PopUpBannerAction popBA = new PopUpBannerAction(OnCancel, OnConfirm);
        uIPopUpBanner = CreatePopUpBanner();
        uIPopUpBanner.SetPopUpData(popBD, popBA);
        uIPopUpBanner.Show();
    }

    private UIPopUpBanner CreatePopUpBanner()
    {
        GameObject popBanAux = Instantiate(popUpBannerPrefab);
        UIPopUpBanner popUpBanner = popBanAux.GetComponent<UIPopUpBanner>();
        return popUpBanner;
    }
}
