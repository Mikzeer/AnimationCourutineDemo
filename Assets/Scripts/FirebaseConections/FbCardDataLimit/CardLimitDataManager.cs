using PositionerDemo;
using System;

public class CardLimitDataManager
{
    HelperCardDataLimitFirebaseKimboko helperCardDataLimitFirebaseKimboko;
    HelperCardCollectionJsonKimboko helperCardCollectionJsonKimboko;

    public CardLimitDataManager()
    {
        helperCardDataLimitFirebaseKimboko = new HelperCardDataLimitFirebaseKimboko();
        helperCardCollectionJsonKimboko = new HelperCardCollectionJsonKimboko();
    }

    public void LoadCardLimitData(UserDB pUser)
    {
        CheckCardLimitDataUpdate(pUser);
    }

    private async void CheckCardLimitDataUpdate(UserDB pUser)
    {
        long bdLastCardDataLimitDownload = await helperCardDataLimitFirebaseKimboko.GetLastCardLimitDataDownload(pUser);
        long bdLastCardLimitUpdate = await helperCardDataLimitFirebaseKimboko.GetLastCardLimitDataUpdate();
        DateTime dtLastCLimitDownload = Helper.UnixTimeStampToDateTimeMiliseconds(bdLastCardDataLimitDownload);
        DateTime dtLastClimitUpdate = Helper.UnixTimeStampToDateTimeMiliseconds(bdLastCardLimitUpdate);
        int dtCompareUserCollection = DateTime.Compare(dtLastClimitUpdate, dtLastCLimitDownload);

        switch (dtCompareUserCollection)
        {
            case -1:
                //date1 is earlier than date2.// EL JUGADOR TIENE LA ULTIMA ACTUALIZACION DE LA CARD COLLECTION
                LoadCardLimitDataFromJson();
                break;
            case 0:
                //date1 is the same as date2.// EL JUGADOR TIENE LA ULTIMA ACTUALIZACION DE LA CARD COLLECTION MUY RARO ESTO PERO PUEDE SER...
                LoadCardLimitDataFromJson();
                break;
            case 1:
                // If date1 is later than date2.// ACA HAY UNA ACTUALIZACION Y ENTONCES TENEMOS QUE CARGARLO DESDE LA BD ONLINE
                LoadCardLimitDataFromFirebase(pUser);
                break;
            default:
                break;
        }
    }

    private void LoadCardLimitDataFromJson()
    {
        CardDataLimit cardDataLimit = helperCardCollectionJsonKimboko.GetCardLimitDataFromJson();
        CardPropertiesDatabase.SetCardDataLimits(cardDataLimit);
    }

    private async void LoadCardLimitDataFromFirebase(UserDB pUser)
    {
        CardDataLimit cardLimitData = await helperCardDataLimitFirebaseKimboko.GetCardDataLimitFromFirebase(pUser);
        CardPropertiesDatabase.SetCardDataLimits(cardLimitData);
        helperCardCollectionJsonKimboko.SetCardLimitToJson(cardLimitData);
    }
}
