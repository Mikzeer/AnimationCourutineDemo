using System;
using System.Collections.Generic;

[Serializable]
public class UsersDecks
{
    Dictionary<int, Deck> userDecks;

    public List<UserDeckForJson> userDecksStr;

    public UsersDecks()
    {
        userDecks = new Dictionary<int, Deck>();
    }

    public void AddNewDeck(Deck deck)
    {
        if (userDecks.ContainsKey(deck.ID) == false)
        {
            userDecks.Add(deck.ID, deck);
        }
    }

    public void ModifyDeck(Deck deck)
    {
        if (userDecks.ContainsKey(deck.ID))
        {
            userDecks[deck.ID] = deck;
        }
    }

    public void EraseDeck(Deck deck)
    {
        if (userDecks.ContainsKey(deck.ID))
        {
            userDecks.Remove(deck.ID);
        }
    }

    public Deck GetDeck(int ID)
    {
        if (userDecks.ContainsKey(ID)) return userDecks[ID];
        return null;
    }

    public void SetUsersDecks(List<UserDeckForJson> usDecks)
    {
        for (int i = 0; i < usDecks.Count; i++)
        {
            if (userDecks.ContainsKey(usDecks[i].ID) == false)
            {
                userDecks.Add(usDecks[i].ID, usDecks[i].userDeck);
            }
        }
    }

    public List<UserDeckForJson> GetUsersDecks()
    {
        List<UserDeckForJson> userDecksStr = new List<UserDeckForJson>();
        foreach (KeyValuePair<int,Deck> item in userDecks)
        {
            UserDeckForJson dkStr = new UserDeckForJson(item.Key, item.Value);
            userDecksStr.Add(dkStr);
        }
        return userDecksStr;
    }

    public void GenerateUserDeckFromDictionary()
    {
        userDecksStr = new List<UserDeckForJson>();
        foreach (KeyValuePair<int, Deck> item in userDecks)
        {
            UserDeckForJson dkStr = new UserDeckForJson(item.Key, item.Value);
            userDecksStr.Add(dkStr);
        }
    }

    public void GenerateUserDeckFromList()
    {
        for (int i = 0; i < userDecksStr.Count; i++)
        {
            userDecks.Add(userDecksStr[i].ID, userDecksStr[i].userDeck);
        }
    }
}