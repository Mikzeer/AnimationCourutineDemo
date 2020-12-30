using System.Threading.Tasks;

public class HelperCardDataLimitFirebaseKimboko
{
    bool debugOn = true;
    FbCardDataLimit fbCardDataLimit;
    FbCardDataLimitUpdater fbCardDataLimitUpdater;

    public HelperCardDataLimitFirebaseKimboko()
    {
        fbCardDataLimit = new FbCardDataLimit();
        fbCardDataLimitUpdater = new FbCardDataLimitUpdater();
    }

    public async Task<long> GetLastCardLimitDataDownload(UserDB pUserDB)
    {
        long bdLastCardDataLimitDownload = await fbCardDataLimitUpdater.GetLastUserCardLimitDataDownloadTimestampUser(pUserDB.Name.ToLower());
        return bdLastCardDataLimitDownload;
    }

    public async Task<long> GetLastCardLimitDataUpdate()
    {
        long bdLastCardLimitUpdate = await fbCardDataLimitUpdater.GetLastCardLimitDataUpdateTimestamp();
        return bdLastCardLimitUpdate;
    }

    public async Task<CardDataLimit> GetCardDataLimitFromFirebase(UserDB pUser)
    {
        CardDataLimit cardLimitData = await fbCardDataLimit.GetCardsLimitData(pUser);
        return cardLimitData;
    }

}
