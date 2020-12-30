using UnityEngine;
using Firebase.Database;
using System.Threading.Tasks;
using System.Collections.Generic;

public class FbCardDataLimitUpdater
{
    private string lastDownload = "utcLastDownloadCardLimitDataUnix";

    public void UpdateLastUserCardLimitDataDownloadTimestamp(UserDB userDB)
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return;

        DatosFirebaseRTHelper.Instance.reference.Child(DatosFirebaseRTHelper.Instance.usersTable)
                                                .Child(userDB.Name.ToLower()).UpdateChildrenAsync(
            new Dictionary<string, object>
            {
                { lastDownload, ServerValue.Timestamp }
            });//utcLastDownloadCardLimitDataUnix
    }

    public async Task<long> GetLastCardLimitDataUpdateTimestamp()
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return 0;

        DataSnapshot dtSnapshot = null;
        DatosFirebaseRTHelper.Instance.reference.Child(DatosFirebaseRTHelper.Instance.CardsLimitDataLastUpdateTable).KeepSynced(true);

        await DatosFirebaseRTHelper.Instance.reference.Child(DatosFirebaseRTHelper.Instance.CardsLimitDataLastUpdateTable).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                //Debug.Log("NoChild");
                // Handle the error...
            }
            else if (task.IsCompleted)
            {
                dtSnapshot = task.Result;
            }
        });
        LastUpdateAuxiliar utcLastGCCDownload = new LastUpdateAuxiliar();
        if (dtSnapshot != null)
        {
            if (dtSnapshot.Exists)
            {
                utcLastGCCDownload = JsonUtility.FromJson<LastUpdateAuxiliar>(dtSnapshot.GetRawJsonValue());
            }
        }

        return utcLastGCCDownload.uctCreatedUnix;
    }

    public void UpdateLastCardLimitDataUpdateTOERASELATERJUSTTOTEST()
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return;

        DatosFirebaseRTHelper.Instance.reference.Child(DatosFirebaseRTHelper.Instance.CardsLimitDataLastUpdateTable).UpdateChildrenAsync(
            new Dictionary<string, object> { { "uctCreatedUnix", ServerValue.Timestamp } });
    }

    public async Task<long> GetLastUserCardLimitDataDownloadTimestampUser(string name)
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return 0;

        FbUserChecker userCheck = new FbUserChecker();
        DataSnapshot userNameExist = await userCheck.UserDataSnapshotExistByName(name.ToLower());

        long utcLastUCCDownload = 0;

        if (userNameExist != null)
        {
            if (userNameExist.Exists)
            {
                UserDB user = JsonUtility.FromJson<UserDB>(userNameExist.GetRawJsonValue());
                utcLastUCCDownload = user.utcLastDownloadCardLimitDataUnix;
            }
        }

        return utcLastUCCDownload;
    }

    public void SetCardLimit()
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return;

        CardDataLimit climit = new CardDataLimit();
        climit.MaxAmountPerDeck = 20;
        climit.MaxAmountPerRarity.cardDataLimitRarityAmount = new List<CardDataLimitRarityAmount>();
        CardDataLimitRarityAmount cdataone = new CardDataLimitRarityAmount();
        cdataone.Amount = 20;
        cdataone.ID = 1;

        CardDataLimitRarityAmount cdatatwo = new CardDataLimitRarityAmount();
        cdatatwo.Amount = 4;
        cdatatwo.ID = 2;

        CardDataLimitRarityAmount cdatathree = new CardDataLimitRarityAmount();
        cdatathree.Amount = 3;
        cdatathree.ID = 3;

        CardDataLimitRarityAmount cdatafour = new CardDataLimitRarityAmount();
        cdatafour.Amount = 2;
        cdatafour.ID = 4;

        CardDataLimitRarityAmount cdatafive = new CardDataLimitRarityAmount();
        cdatafive.Amount = 1;
        cdatafive.ID = 5;

        climit.MaxAmountPerRarity.cardDataLimitRarityAmount.Add(cdataone);
        climit.MaxAmountPerRarity.cardDataLimitRarityAmount.Add(cdatatwo);
        climit.MaxAmountPerRarity.cardDataLimitRarityAmount.Add(cdatathree);
        climit.MaxAmountPerRarity.cardDataLimitRarityAmount.Add(cdatafour);
        climit.MaxAmountPerRarity.cardDataLimitRarityAmount.Add(cdatafive);


        string json = JsonUtility.ToJson(climit);

        DatosFirebaseRTHelper.Instance.reference.Child(DatosFirebaseRTHelper.Instance.CardsLimitDataTable).SetRawJsonValueAsync(json);

        DatosFirebaseRTHelper.Instance.reference.Child(DatosFirebaseRTHelper.Instance.CardsLimitDataLastUpdateTable).UpdateChildrenAsync(
            new Dictionary<string, object> { { "uctCreatedUnix", ServerValue.Timestamp } });
    }
}

