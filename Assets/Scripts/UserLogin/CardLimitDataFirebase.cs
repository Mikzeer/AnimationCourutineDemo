using UnityEngine;
using Firebase.Database;
using System.Threading.Tasks;
using System.Collections.Generic;

public class CardLimitDataFirebase : MonoBehaviour
{
    private const string usersTable = "Users";
    private const string CardsLimitDataTable = "CardsLimitData";
    private const string CardsLimitDataLastUpdateTable = "CardsLimitDataLastUpdate";

    public async Task<CardDataLimit> GetCardsLimitData(UserDB pUser)
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return null;

        CardDataLimit cardsLimitData = new CardDataLimit();

        await FirebaseDatabase.DefaultInstance.GetReference(CardsLimitDataTable).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                //Debug.Log("NoChild");
                // Handle the error...
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                cardsLimitData = JsonUtility.FromJson<CardDataLimit>(snapshot.GetRawJsonValue());

                UpdateLastUserCardLimitDataDownloadTimestamp(pUser);
            }
        });



        return cardsLimitData;
    }

    public async Task<long> GetLastCardLimitDataUpdateTimestamp()
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return 0;

        DataSnapshot dtSnapshot = null;
        await FirebaseDatabase.DefaultInstance.GetReference(CardsLimitDataLastUpdateTable).GetValueAsync().ContinueWith(task =>
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

        DatosFirebaseRTHelper.Instance.reference.Child(CardsLimitDataLastUpdateTable).UpdateChildrenAsync(
            new Dictionary<string, object> { { "uctCreatedUnix", ServerValue.Timestamp } });
    }

    public async Task<long> GetLastUserCardLimitDataDownloadTimestampUser(string name)
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return 0;

        DataSnapshot userNameExist = await UserDataSnapshotExistByName(name.ToLower());

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

    public void UpdateLastUserCardLimitDataDownloadTimestamp(UserDB userDB)
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return;

        DatosFirebaseRTHelper.Instance.reference.Child(usersTable).Child(userDB.Name.ToLower()).UpdateChildrenAsync(
            new Dictionary<string, object> { { "utcLastDownloadCardLimitDataUnix", ServerValue.Timestamp } });//utcLastDownloadCardLimitDataUnix
    }

    private async Task<DataSnapshot> UserDataSnapshotExistByName(string name)
    {
        DataSnapshot dtSnapshot = null;
        await FirebaseDatabase.DefaultInstance.GetReference(usersTable).Child(name).GetValueAsync().ContinueWith(task =>
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

        return dtSnapshot;
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

        DatosFirebaseRTHelper.Instance.reference.Child(CardsLimitDataTable).SetRawJsonValueAsync(json);

        DatosFirebaseRTHelper.Instance.reference.Child(CardsLimitDataLastUpdateTable).UpdateChildrenAsync(
            new Dictionary<string, object> { { "uctCreatedUnix", ServerValue.Timestamp } });
    }
}

public class UserManager
{
    //  HAY UN SOLO USER POR APLICACION
    private UserDB pUserDB;
    private UserResources userResources;

    public void SetUser(UserDB pUser)
    {
        pUserDB = pUser;
    }

    public UserDB GetUser()
    {
        return pUserDB;
    }

    public void SetUserResources(UserResources userResources)
    {
        this.userResources = userResources;
    }

    public UserResources GetUserResources()
    {
        return userResources;
    }
}

