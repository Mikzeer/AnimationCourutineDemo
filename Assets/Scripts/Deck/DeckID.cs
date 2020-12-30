using System;

[Serializable]
public class DeckID
{
    public int lastDeckID;

    public DeckID()
    {

    }

    public DeckID(int lastDeckID)
    {
        this.lastDeckID = lastDeckID;
    }
}
