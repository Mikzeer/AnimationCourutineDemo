using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UserDeckSelectionManager : MonoBehaviour
{
    [SerializeField] private Button btnStartGame = default;
    [SerializeField] private Button btnBack = default;
    [SerializeField] private Text txtDeckName = default;
    [SerializeField] private DeckSelectionUserDeckUI deckSelectionUserDeckUI = default;
    [SerializeField] private GameMenuManager gameMenuManager = default;
    Deck selectedDeck;

    private void OnEnable()
    {
        LoadDecks();
        btnStartGame.onClick.AddListener(StartGame);
        btnBack.onClick.AddListener(Back);
    }

    private void OnDisable()
    {
        btnStartGame.onClick.RemoveAllListeners();
        btnBack.onClick.RemoveAllListeners();
    }

    private void Awake()
    {
        txtDeckName.text = string.Empty;
    }

    public void OnDeckSelectionChange(Deck deck)
    {
        if (selectedDeck == deck) return;

        selectedDeck = deck;
        txtDeckName.text = deck.name;
    }

    public void LoadDecks()
    {
        deckSelectionUserDeckUI.ClearUserDeckDisplay();
        HelperCardCollectionJsonKimboko help = new HelperCardCollectionJsonKimboko();
        UsersDecks userDecks = help.GetUserDecksFromJson();
        userDecks.GenerateUserDeckFromList();
        for (int i = 0; i < userDecks.userDecksStr.Count; i++)
        {
            gameMenuManager.GenerateUserDeckFromJson(userDecks.userDecksStr[i].userDeck);
            CreateNewUserDeckDisplay(userDecks.userDecksStr[i].userDeck);
        }
    }

    private void CreateNewUserDeckDisplay(Deck deckData)
    {
        UserDeckSelectionDisplay userDeckDisplay = deckSelectionUserDeckUI.CreateNewUserDeckDisplay(deckData.ID);
        userDeckDisplay.SetData(deckData, OnDeckSelectionChange);
    }

    private void StartGame()
    {
        GameSceneManager.Instance.SetActiveWaitForLoad(true);
        if (selectedDeck == null)
        {
            ShowNotSelectedDeckBanner();
            return;
        }
        Debug.Log("START GAME WITH DECK " + selectedDeck.name);

        HelperCardCollectionJsonKimboko help = new HelperCardCollectionJsonKimboko();
        ConfigurationData cnfDat = help.GetConfigurationDataFromJson();

        if (cnfDat == null)
        {
            GameSceneManager.Instance.SetActiveWaitForLoad(false);
            return;
        }
        cnfDat.selectedDeck = selectedDeck;
        help.SetConfigurationDataToJson(cnfDat);
        GameSceneManager.Instance.SetActiveWaitForLoad(false);
        GameSceneManager.Instance.LoadSceneAsync(GameSceneManager.GAMESCENE.GAME);
    }

    private void Back()
    {
        gameObject.SetActive(false);
        deckSelectionUserDeckUI.ClearUserDeckDisplay();
        selectedDeck = null;
        txtDeckName.text = string.Empty;
    }

    private void ShowNotSelectedDeckBanner()
    {
        string title = "NO DECK";
        string description = "PLEASE, SELECT A DECK";
        gameMenuManager.GeneratePopUpBanner(title, description, null, null);
    }

}
