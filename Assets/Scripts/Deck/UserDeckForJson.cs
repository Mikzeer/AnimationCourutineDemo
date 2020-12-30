using System;

[Serializable]
public class UserDeckForJson
{
    public int ID;
    public Deck userDeck;
    public UserDeckForJson(int ID, Deck userDeck)
    {
        this.ID = ID;
        this.userDeck = userDeck;
    }
}