
public class DeckBuilderUserDeckManager
{
    GameMenuManager gameMenuManager;
    DeckBuilderUserDeckUI deckBuilderUserDeckUI;
    DeckBuilderManager deckBuilderManager;
    UsersDecks userDecks;

    public DeckBuilderUserDeckManager(DeckBuilderUserDeckUI deckBuilderUserDeckUI, DeckBuilderManager deckBuilderManager, GameMenuManager gameMenuManager)
    {
        this.deckBuilderUserDeckUI = deckBuilderUserDeckUI;
        this.deckBuilderManager = deckBuilderManager;
        this.gameMenuManager = gameMenuManager;
    }

    public void LoadUserDecks()
    {
        deckBuilderUserDeckUI.ClearUserDeckDisplay();
        HelperCardCollectionJsonKimboko help = new HelperCardCollectionJsonKimboko();
        userDecks = help.GetUserDecksFromJson();
        userDecks.GenerateUserDeckFromList();
        for (int i = 0; i < userDecks.userDecksStr.Count; i++)
        {
            gameMenuManager.GenerateUserDeckFromJson(userDecks.userDecksStr[i].userDeck);
            CreateNewUserDeckDisplay(userDecks.userDecksStr[i].userDeck);
        }
        GameSceneManager.Instance.SetActiveWaitForLoad(false);
    }

    private void CreateNewUserDeckDisplay(Deck deckData)
    {
        UserDeckDisplay userDeckDisplay = deckBuilderUserDeckUI.CreateNewUserDeckDisplay(deckData.ID);
        userDeckDisplay.SetData(deckData, deckBuilderManager.OnTryModifyUserDeck);
    }

    public void SaveNewDeck(Deck auxDeck)
    {
        auxDeck.GenerateUserDeckToJson();
        userDecks.AddNewDeck(auxDeck);
        userDecks.GenerateUserDeckFromDictionary();
        HelperCardCollectionJsonKimboko help = new HelperCardCollectionJsonKimboko();
        help.SetUserDecks(userDecks);
    }

    public void ModiyExistingDeck(Deck auxDeck)
    {
        auxDeck.GenerateUserDeckToJson();
        userDecks.ModifyDeck(auxDeck);
        userDecks.GenerateUserDeckFromDictionary();
        HelperCardCollectionJsonKimboko help = new HelperCardCollectionJsonKimboko();
        help.SetUserDecks(userDecks);
    }

    public void RemoveExistingDeck(Deck auxDeck)
    {
        userDecks.EraseDeck(auxDeck);
        userDecks.GenerateUserDeckFromDictionary();
        HelperCardCollectionJsonKimboko help = new HelperCardCollectionJsonKimboko();
        help.SetUserDecks(userDecks);
    }
}
