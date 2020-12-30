using MikzeerGame;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DeckBuilderManager : MonoBehaviour
{
    public bool debugOn = false;
    [Header("BUTTONS")]
    [SerializeField] private Button btnCreateNewDeck;
    [SerializeField] private Button btnBack;
    [SerializeField] private Button btnDeleteDeck;
    [SerializeField] private Button btnCancelDeleteDeck;
    [SerializeField] private Button btnSave;
    [Header("UI")]
    [SerializeField] private InputField nameInputFiled;
    [Header("PARENTS")]
    [SerializeField] private RectTransform canvas;
    [Header("MANAGERS")]
    [SerializeField] private RibbonManagerUI ribbonManagerUI;
    [SerializeField] private DeckBuilderCreationUI deckBuilderCreationUI;
    [SerializeField] private DeckBuilderUserDeckUI deckBuilderUserDeckUI;
    [SerializeField] private CardCollectionVisualManager cardCollectionVisualManager;
    [SerializeField] private CardCollectionSearchManager cardCollectionSearchManager;
    [SerializeField] private GameMenuManager gameMenuManager;

    public DeckBuilderCreationManager deckBuilderCreationManager { get; private set; }
    public static event Action<Deck> OnDeckChange;
    DeckBuilderUserDeckManager deckBuilderUserDeckManager;
    RibbonManager ribbonManager;

    private void OnEnable()
    {
        btnCreateNewDeck.onClick.AddListener(CreateNewDeck);
        btnBack.onClick.AddListener(Back);
        btnDeleteDeck.onClick.AddListener(Delete);
        btnCancelDeleteDeck.onClick.AddListener(CancelDelete);
        nameInputFiled.onEndEdit.AddListener(OnDeckNameEndEdit);
    }

    private void OnDisable()
    {
        btnCreateNewDeck.onClick.RemoveAllListeners();
        btnBack.onClick.RemoveAllListeners();
        btnSave.onClick.RemoveAllListeners();
        btnDeleteDeck.onClick.RemoveAllListeners();
        btnCancelDeleteDeck.onClick.RemoveAllListeners();
        nameInputFiled.onEndEdit.RemoveAllListeners();
    }

    private void Start()
    {
        CardDisplay cardDisplay = cardCollectionVisualManager.CreateCardDisplayForRibbon(canvas);
        ribbonManagerUI.SetCardDisplay(cardDisplay);
        ribbonManager = new RibbonManager(ribbonManagerUI);
        deckBuilderCreationManager = new DeckBuilderCreationManager(OnDeckChange, ribbonManager);
        deckBuilderUserDeckManager = new DeckBuilderUserDeckManager(deckBuilderUserDeckUI, this, gameMenuManager);
        CardCollectionManager.OnCardCollectionLoad += deckBuilderUserDeckManager.LoadUserDecks;
    }

    public void CreateNewDeck()
    {
        deckBuilderCreationUI.SetActiveStatusPanel();
        deckBuilderCreationManager.CreateNewDeck();
        deckBuilderUserDeckUI.ClearUserDeckDisplay();
        btnSave.onClick.RemoveAllListeners();
        btnSave.onClick.AddListener(Save);
        gameMenuManager.LoadVisualDeckBuilder();
        cardCollectionSearchManager.ClearAllFiltters();
    }

    private void Save()
    {
        if (deckBuilderCreationManager.isDirty == false)
        {
            ClearDeckBuilder();
            return;
        }
        string title = "SAVE NEW DECK";
        string description = "Do you wanna Save this new deck ? " + deckBuilderCreationManager.auxDeck.name;
        gameMenuManager.GeneratePopUpBanner(title, description, () => { return; }, SaveBtnAccept);
    }

    private void SaveBtnAccept()
    {
        if (debugOn) Debug.Log("Accept Save");

        if (deckBuilderCreationManager.IsDeckQuantityMoreThanTheMinimum() == false)
        {
            string title = "INVALID DECK";
            string description = "YOU DONT HAVE ENOUGH CARDS, YOU NEED AT LEAST 10 CARDS IN YOUR DECK";
            gameMenuManager.GeneratePopUpBanner(title, description, null, null);
            return;
        }
        deckBuilderCreationManager.SaveNewDeck();
        deckBuilderUserDeckManager.SaveNewDeck(deckBuilderCreationManager.auxDeck);
        ClearDeckBuilder();
    }

    private void ClearDeckBuilder()
    {
        nameInputFiled.text = string.Empty;
        ribbonManager.Clear();
        deckBuilderCreationManager.Clear();
        gameMenuManager.LoadVisualCollection();
        deckBuilderUserDeckManager.LoadUserDecks();
        deckBuilderCreationUI.Clear();
        GameSceneManager.Instance.SetActiveWaitForLoad(false);
        cardCollectionSearchManager.ClearAllFiltters();
    }

    public void OnTryModifyUserDeck(Deck userDeck)
    {
        if (deckBuilderCreationManager.isEditing == true) return;
        ModifyDeck(userDeck);
    }

    public void ModifyDeck(Deck userDeck)
    {
        deckBuilderCreationUI.SetActiveStatusPanel();
        deckBuilderCreationManager.ModifyDeck(userDeck);
        deckBuilderUserDeckUI.ClearUserDeckDisplay();
        btnSave.onClick.RemoveAllListeners();
        btnSave.onClick.AddListener(Modify);
        nameInputFiled.text = userDeck.name;
        gameMenuManager.LoadVisualDeckBuilder();
        Dictionary<CardData, CardDisplaySlot> cardSlotUIDisplay;
        cardSlotUIDisplay = cardCollectionVisualManager.GetCardDisplaySlotDictionary();
        for (int i = 0; i < userDeck.userDeck.Count; i++)
        {
            var item = userDeck.userDeck.ElementAt(i);
            for (int j = 0; j < cardSlotUIDisplay.Count; j++)
            {
                var itnDis = cardSlotUIDisplay.ElementAt(j);
                if (item.Key.ID == itnDis.Key.ID)
                {
                    CardSlotUI cSlot = cardSlotUIDisplay[itnDis.Key].cardSlotUI;
                    CardSlot slot = cSlot.cardSlot;
                    slot.AddDeckAmount(item.Value.Amount);
                    //cSlot.ChangeSlotData();
                    RibbonData ribbonData = new RibbonData(item.Key.CardName, item.Value.Amount, item.Key, cSlot);
                    ribbonManager.AddRibbon(ribbonData, deckBuilderCreationManager);
                    break;
                }
            }
        }
        cardCollectionSearchManager.ClearAllFiltters();
    }

    private void Modify()
    {
        if (deckBuilderCreationManager.isDirty == false)
        {
            ClearDeckBuilder();
            return;
        }
        string title = "MODIFY DECK " + deckBuilderCreationManager.auxDeck.name;
        string description = "Do you wanna Save the modifications ? ";
        gameMenuManager.GeneratePopUpBanner(title, description, () => { return; }, ModifyBtnAccept);
    }

    private void ModifyBtnAccept()
    {
        if (debugOn) Debug.Log("Accept Modify");

        if (deckBuilderCreationManager.IsDeckQuantityMoreThanTheMinimum() == false)
        {
            string title = "INVALID DECK";
            string description = "YOU DONT HAVE ENOUGH CARDS, YOU NEED AT LEAST 10 CARDS IN YOUR DECK";
            gameMenuManager.GeneratePopUpBanner(title, description, null, null);
            return;
        }
        deckBuilderUserDeckManager.ModiyExistingDeck(deckBuilderCreationManager.auxDeck);
        ClearDeckBuilder();
    }

    private void Back()
    {
        if (deckBuilderCreationManager.isDirty == false)
        {
            ClearDeckBuilder();
            return;
        }
        string title = "STOP DECK BUILDER";
        string description = "Do you wanna leave without save?";
        gameMenuManager.GeneratePopUpBanner(title, description, () => { return; }, ClearDeckBuilder);
    }

    private void CancelDelete()
    {
        SetActiveCreationAndEditigButtons(true);
        deckBuilderUserDeckUI.ChangeUserDeckDisplayEvent(OnTryModifyUserDeck);
    }

    private void Delete()
    {
        string title = "DELETE SOME DECK ";
        string description = "Do you wanna DELETE A DECK ? ";
        gameMenuManager.GeneratePopUpBanner(title, description, () => { return; }, OnTryEnterToDeleteDeckBtnAcept);
    }

    private void OnTryEnterToDeleteDeckBtnAcept()
    {
        deckBuilderUserDeckUI.ChangeUserDeckDisplayEvent(OnTryDeleteUserDeck);
        SetActiveCreationAndEditigButtons(false);
    }

    public void OnTryDeleteUserDeck(Deck userDeck)
    {
        deckBuilderCreationManager.OnTryDeleteUserDeck(userDeck);
        DeleteDeck(userDeck);
    }

    public void DeleteDeck(Deck userDeck)
    {
        string title = "DELETE SOME DECK ";
        string description = "ARE YOU SURE YOU WANT TO DELETE " + userDeck.name + "?";
        gameMenuManager.GeneratePopUpBanner(title, description, () => { return; }, RemoveExistingDeckBtnAcept);
    }

    public void RemoveExistingDeckBtnAcept()
    {
        deckBuilderUserDeckManager.RemoveExistingDeck(deckBuilderCreationManager.auxDeck);
        deckBuilderUserDeckUI.ClearUserDeckDisplay();
        deckBuilderUserDeckManager.LoadUserDecks();
        SetActiveCreationAndEditigButtons(true);
    }

    public void OnDeckNameEndEdit(string deckName)
    {
        deckBuilderCreationManager.OnDeckNameEndEdit(deckName);
    }

    private void SetActiveCreationAndEditigButtons(bool isActive)
    {
        if (isActive)
        {
            btnCreateNewDeck.gameObject.SetActive(true);
            btnDeleteDeck.gameObject.SetActive(true);
            btnCancelDeleteDeck.gameObject.SetActive(false);
        }
        else
        {
            btnCreateNewDeck.gameObject.SetActive(false);
            btnDeleteDeck.gameObject.SetActive(false);
            btnCancelDeleteDeck.gameObject.SetActive(true);
        }
    }

    public bool IsEditing()
    {
        return deckBuilderCreationManager.isEditing;
    }
}
