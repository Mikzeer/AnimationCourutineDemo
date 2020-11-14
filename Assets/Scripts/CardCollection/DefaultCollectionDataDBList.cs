using System.Collections.Generic;
using System;

[Serializable]
public class DefaultCollectionDataDBList
{
    public List<DefaultCollectionDataDB> dfCollection;

    public DefaultCollectionDataDBList()
    {
        dfCollection = new List<DefaultCollectionDataDB>();
    }

    public DefaultCollectionDataDBList(List<DefaultCollectionDataDB> dfCollection)
    {
        this.dfCollection = dfCollection;
    }
}
