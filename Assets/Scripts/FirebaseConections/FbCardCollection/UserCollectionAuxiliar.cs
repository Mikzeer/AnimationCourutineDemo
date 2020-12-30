using System.Collections.Generic;

public class UserCollectionAuxiliar
{
    public List<DefaultCollectionDataDB> dfCollection;
    public Dictionary<string, int> quantityOfCardsUserHaveFromBDOnline;

    public UserCollectionAuxiliar(List<DefaultCollectionDataDB> dfCollection, Dictionary<string, int> quantityOfCardsUserHaveFromBDOnline)
    {
        this.dfCollection = dfCollection;
        this.quantityOfCardsUserHaveFromBDOnline = quantityOfCardsUserHaveFromBDOnline;
    }
}