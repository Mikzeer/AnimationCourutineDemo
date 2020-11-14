using System.Collections.Generic;
using System;

[Serializable]
public class DefaultCollectionDataDB
{
    public string ID;
    public int Amount;

    public DefaultCollectionDataDB(string ID, int Amount)
    {
        this.ID = ID;
        this.Amount = Amount;
    }

    public DefaultCollectionDataDB()
    {
        Amount = 0;
    }

    public Dictionary<string, System.Object> ToDictionary()
    {
        Dictionary<string, System.Object> result = new Dictionary<string, System.Object>();
        result["ID"] = ID;
        result["Amount"] = Amount;

        return result;
    }
}

