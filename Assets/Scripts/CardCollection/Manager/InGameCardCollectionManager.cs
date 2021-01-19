using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using System.Threading.Tasks;
using PositionerDemo;

public class InGameCardCollectionManager
{
    private CardData[] allCardsDataArray; // ALL THE CARDS THAT EXIST IN THE GAME
    private Dictionary<string, CardDataRT> cardCollectionLibraryFromBDOnline = new Dictionary<string, CardDataRT>(); // id // carddata
    private Dictionary<string, Dictionary<string, int>> allUserCards = new Dictionary<string, Dictionary<string, int>>(); // UserID // <CardID, CardAmount>
    private HelperUserCardCollectionFirebaseKimboko helperUserCardCollectionFirebaseKimboko;
    private HelperGameCardCollectionFirebaseKimboko helperGameCardCollectionFirebaseKimboko;
    private HelperCardCollectionJsonKimboko helperCardCollectionJsonKimboko;
    private MonoBehaviour gameMenuManager;
    private Action OnCardCollectionLoad;
    private bool gameCollectionLoaded = false;
    private bool userCollectionLoaded = false;

    public InGameCardCollectionManager(MonoBehaviour owner, Action OnCardCollectionLoad)
    {
        this.gameMenuManager = owner;
        this.OnCardCollectionLoad = OnCardCollectionLoad;
        helperCardCollectionJsonKimboko = new HelperCardCollectionJsonKimboko();
        helperUserCardCollectionFirebaseKimboko = new HelperUserCardCollectionFirebaseKimboko();
        helperGameCardCollectionFirebaseKimboko = new HelperGameCardCollectionFirebaseKimboko();
    }

    public async void LoadAllCollection(UserDB[] users)
    {
        gameMenuManager.StartCoroutine(WaitForLoading());
        LoadGameCollectionFromFirebase();
        for (int i = 0; i < users.Length; i++)
        {
            bool isLoaded = await LoadUserCollectionFromFirebase(users[i]);
        }
        userCollectionLoaded = true;
    }

    public void LoadAllCollectionJson(UserDB[] users)
    {
        gameMenuManager.StartCoroutine(WaitForLoading());
        LoadGameCollectionFromJson();
        for (int i = 0; i < users.Length; i++)
        {
            LoadUserCollectionFromJson(users[i]);
        }
        userCollectionLoaded = true;
    }

    private async void LoadGameCollectionFromFirebase()
    {
        GameCollectionAuxiliar gameCollectionAuxiliar = await helperGameCardCollectionFirebaseKimboko.GetGameCollectionFromFirebase();
        cardCollectionLibraryFromBDOnline = gameCollectionAuxiliar.cardCollectionLibraryFromBDOnline;
        allCardsDataArray = gameCollectionAuxiliar.allCardsDataArray;
        gameCollectionLoaded = true;
    }

    private async Task<bool> LoadUserCollectionFromFirebase(UserDB pUser)
    {
        UserCollectionAuxiliar userCollectionAuxiliar = await helperUserCardCollectionFirebaseKimboko.GetUserCollectionFromFirebase(pUser);
        Dictionary<string, int> quantityOfCardsUserHaveFromBDOnline = userCollectionAuxiliar.quantityOfCardsUserHaveFromBDOnline;
        allUserCards.Add(pUser.ID, quantityOfCardsUserHaveFromBDOnline);

        return true;
    }

    private IEnumerator WaitForLoading()
    {
        while (gameCollectionLoaded == false && userCollectionLoaded == false)
        {
            yield return null;
        }
        Debug.Log("ALL LOADED");
        OnCardCollectionLoad?.Invoke();
    }

    public CardData[] GetAllCardDataArray()
    {
        return allCardsDataArray;
    }

    public Dictionary<string, CardDataRT> GetGameCollectionDictionary()
    {
        return cardCollectionLibraryFromBDOnline;
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

    private void LoadGameCollectionFromJson()
    {
        GameCollectionAuxiliar gameCollectionAuxiliar = helperCardCollectionJsonKimboko.GetGameCollectionFromJson();
        cardCollectionLibraryFromBDOnline = gameCollectionAuxiliar.cardCollectionLibraryFromBDOnline;
        allCardsDataArray = gameCollectionAuxiliar.allCardsDataArray;
        gameCollectionLoaded = true;
    }

    private void LoadUserCollectionFromJson(UserDB pUser)
    {
        UserCollectionAuxiliar userCollectionAuxiliar = helperCardCollectionJsonKimboko.GetUserCollectionFromJson();
        Dictionary<string, int> quantityOfCardsUserHaveFromBDOnline = userCollectionAuxiliar.quantityOfCardsUserHaveFromBDOnline;
        allUserCards.Add(pUser.ID, quantityOfCardsUserHaveFromBDOnline);
    }

}