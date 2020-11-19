using UnityEngine;
using Firebase.Database;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

public class CardCollectionFirebase : MonoBehaviour
{
    private const string usersTable = "Users";
    private const string cardsTable = "Cards";
    private const string UsersCardCollectionTable = "UsersCardCollection";
    private const string DefaultCollectionTable = "DefaultCollection";
    private const string GameCardCollectionLastUpdateTable = "GameCollectionLastUpdate";

    public async Task<List<DefaultCollectionDataDB>> CreateNewUserCollection(UserDB user)
    {       
        List<DefaultCollectionDataDB> allCardList = await GetDefaultCardCollection();

        if (allCardList != null && allCardList.Count > 0)
        {
            // SET ISFIRSTTIME TO FALSE
            await DatosFirebaseRTHelper.Instance.reference.Child(usersTable).Child(user.Name.ToLower()).Child("IsFirstTime").SetValueAsync(false);

            // SET DEFAULT COLLECTION IN THE USERS COLLECTION IN DB
            foreach (DefaultCollectionDataDB dcData in allCardList)
            {
                string json = JsonUtility.ToJson(dcData);
                await DatosFirebaseRTHelper.Instance.reference.Child(UsersCardCollectionTable).Child(user.Name.ToLower()).Child(dcData.ID).SetRawJsonValueAsync(json);
            }
            UpdateLastUserCardCollectionDownloadTimestamp(user);
        }

        return allCardList;
    }

    public async Task<List<CardDataRT>> GetGameCardCollection(UserDB pUser)
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return null;

        List<CardDataRT> allCardList = new List<CardDataRT>();

        await FirebaseDatabase.DefaultInstance.GetReference(cardsTable).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                //Debug.Log("NoChild");
                // Handle the error...
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (var child in snapshot.Children)
                {
                    CardDataRT card = JsonUtility.FromJson<CardDataRT>(child.GetRawJsonValue());
                    allCardList.Add(card);
                }
            }
        });

        if (allCardList.Count > 0)
        {
            UpdateLastGameCardCollectionDownloadTimestamp(pUser);
        }

        return allCardList;
    }

    public async Task<List<DefaultCollectionDataDB>> GetUserCardCollection(UserDB pUser)
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return null;

        List<DefaultCollectionDataDB> allCardList = new List<DefaultCollectionDataDB>();

        await FirebaseDatabase.DefaultInstance.GetReference(UsersCardCollectionTable).Child(pUser.Name.ToLower()).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                //Debug.Log("NoChild");
                // Handle the error...
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (var child in snapshot.Children)
                {
                    DefaultCollectionDataDB card = JsonUtility.FromJson<DefaultCollectionDataDB>(child.GetRawJsonValue());
                    allCardList.Add(card);
                }
            }
        });

        if (allCardList.Count > 0)
        {
            UpdateLastUserCardCollectionDownloadTimestamp(pUser);
            //long lastUpdate = await GetLastUserCardCollectionDownloadTimestampUser(pUser.Name.ToLower());
            //Debug.Log("LONG " + lastUpdate);            
        }

        return allCardList;
    }

    public void UpdateLastGameCardCollectionDownloadTimestamp(UserDB userDB)
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return;

        DatosFirebaseRTHelper.Instance.reference.Child("Users").Child(userDB.Name.ToLower()).UpdateChildrenAsync(
            new Dictionary<string, object> { { "utcLastDownloadGameCollectionUnix", ServerValue.Timestamp } });
    }

    public async void UpdateLastUserCardCollectionDownloadTimestamp(UserDB userDB)
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return;

        await DatosFirebaseRTHelper.Instance.reference.Child("Users").Child(userDB.Name.ToLower()).UpdateChildrenAsync(
            new Dictionary<string, object> { { "utcLastDownloadUserCollectionUnix", ServerValue.Timestamp } });

        long lastUpdate = await GetLastUserCardCollectionDownloadTimestampUser(userDB.Name.ToLower());
        //Debug.Log("LONG inside " + lastUpdate);
        CardCollection.Instance.SetLastUserCollectionUpdateToJson(lastUpdate);
    }

    public async void UpdateLastUserCardCollectionModifyUpdateTimestamp(UserDB userDB)
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return;

        await DatosFirebaseRTHelper.Instance.reference.Child("Users").Child(userDB.Name.ToLower()).UpdateChildrenAsync(
            new Dictionary<string, object> { { "utcLastModificationUserCollectionUnix", ServerValue.Timestamp } });
    }

    public void UpdateLastGameCardCollectionUpdateTOERASELATERJUSTTOTEST()
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return;

        DatosFirebaseRTHelper.Instance.reference.Child(GameCardCollectionLastUpdateTable).UpdateChildrenAsync(
            new Dictionary<string, object> { { "uctCreatedUnix", ServerValue.Timestamp } });
    }

    public async Task<long> GetLastGameCardCollectionDownloadTimestampUser(string name)
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return 0;

        DataSnapshot userNameExist = await UserDataSnapshotExistByName(name.ToLower());

        long utcLastGCCDownload = 0;

        if (userNameExist != null)
        {
            if (userNameExist.Exists)
            {
                UserDB user = JsonUtility.FromJson<UserDB>(userNameExist.GetRawJsonValue());                
                utcLastGCCDownload = user.utcLastDownloadGameCollectionUnix;
            }
        }

        return utcLastGCCDownload;
    }

    public async Task<long> GetLastUserCardCollectionDownloadTimestampUser(string name)
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return 0;

        DataSnapshot userNameExist = await UserDataSnapshotExistByName(name.ToLower());

        long utcLastUCCDownload = 0;

        if (userNameExist != null)
        {
            if (userNameExist.Exists)
            {
                UserDB user = JsonUtility.FromJson<UserDB>(userNameExist.GetRawJsonValue());
                utcLastUCCDownload = user.utcLastDownloadUserCollectionUnix;
            }
        }

        return utcLastUCCDownload;
    }

    public async Task<long> GetLastUserCardCollectioModificationTimestampUser(string name)
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false)
        {
            //Debug.Log("FIREBASE NOT INITIALIZE");
            return 0;
        }

        FirebaseDatabase.DefaultInstance.GetReference("Users").Child(name).KeepSynced(true);

        DataSnapshot userNameExist = await UserDataSnapshotExistByName(name.ToLower());

        long utcLastUCCDownload = 0;

        if (userNameExist != null)
        {
            if (userNameExist.Exists)
            {
                UserDB user = JsonUtility.FromJson<UserDB>(userNameExist.GetRawJsonValue());
                utcLastUCCDownload = user.utcLastModificationUserCollectionUnix;
                //Debug.Log("user.MAcc " + user.Salt);
            }
        }

        return utcLastUCCDownload;
    }


    // ESTE ES EL METODO QUE SI FUNCIONO SIN TENER QUE ESCRIBIR UN CHOTO..
    // CREO QUE FirebaseDatabase.DefaultInstance A VECES TE PUEDE TRAER UNA COPIA VIEJA DE LOS DATOS
    // QUE NO TENGO NI LAS MAS PUTA IDEA DE DONDE PUEDEN ESTAR GUARDADOS
    public async Task<long> GetLastGameCardCollectionUpdateTimestamp()
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return 0;

        //DatosFirebaseRTHelper.Instance.reference.Child(GameCardCollectionLastUpdateTable)
        //FirebaseDatabase.DefaultInstance.GetReference(GameCardCollectionLastUpdateTable)
        //FirebaseDatabase.DefaultInstance.GetReference(GameCardCollectionLastUpdateTable).KeepSynced(true);

        DataSnapshot dtSnapshot = null;
        await DatosFirebaseRTHelper.Instance.reference.Child(GameCardCollectionLastUpdateTable).GetValueAsync().ContinueWith(task =>
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
                Debug.Log("LAST GAME UPDATE " + utcLastGCCDownload.uctCreatedUnix);
            }
        }      

        return utcLastGCCDownload.uctCreatedUnix;
    }

    private async Task<DataSnapshot> UserDataSnapshotExistByName(string name)
    {
        DataSnapshot dtSnapshot = null;

        

        await FirebaseDatabase.DefaultInstance.GetReference("Users").Child(name).GetValueAsync().ContinueWith(task =>
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

    public async Task<List<DefaultCollectionDataDB>> GetDefaultCardCollection()
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return null;

        List<DefaultCollectionDataDB> allCardList = new List<DefaultCollectionDataDB>();

        await FirebaseDatabase.DefaultInstance.GetReference(DefaultCollectionTable).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                //Debug.Log("NoChild");
                // Handle the error...
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (var child in snapshot.Children)
                {
                    DefaultCollectionDataDB card = JsonUtility.FromJson<DefaultCollectionDataDB>(child.GetRawJsonValue());
                    allCardList.Add(card);
                }
            }
        });

        return allCardList;
    }

    public async Task<List<CardDataRT>> GetGameCardCollection()
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return null;

        List<CardDataRT> allCardList = new List<CardDataRT>();

        await FirebaseDatabase.DefaultInstance.GetReference(cardsTable).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                //Debug.Log("NoChild");
                // Handle the error...
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (var child in snapshot.Children)
                {
                    CardDataRT card = JsonUtility.FromJson<CardDataRT>(child.GetRawJsonValue());
                    allCardList.Add(card);
                }
            }
        });

        return allCardList;
    }

    public async Task<bool> SetNewCardToUserCardCollection(DefaultCollectionDataDB pCardData, UserDB pUserDB)
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return false;

        int cardAmount = await GetUserCardCollectionCardAmount(pCardData, pUserDB);
        //Debug.Log("CARD AMOUNT INITIAL CARD ID " + pCardData.ID + " Amount: " + cardAmount);
        cardAmount++;
        //Debug.Log("CARD AMOUNT TO SET CARD ID " + pCardData.ID + " Amount: " + cardAmount);

        await DatosFirebaseRTHelper.Instance.reference.Child(UsersCardCollectionTable)
                                                      .Child(pUserDB.Name.ToLower())
                                                      .Child(pCardData.ID)                                                      
                                                      .UpdateChildrenAsync(new Dictionary<string, object> { { "Amount", cardAmount } });

        int newCardAmount = await GetUserCardCollectionCardAmount(pCardData, pUserDB);

        //Debug.Log("NEW CARD AMOUNT SETED ON DB CARD ID " + pCardData.ID + " Amount: " + cardAmount);

        if (cardAmount == newCardAmount)
        {
            //Debug.Log("IS THE SAME AMOUNT TRUE");
            UpdateLastUserCardCollectionModifyUpdateTimestamp(pUserDB);
            return true;
        }
        else
        {
            return false;
        }

        
    }

    public async void RestCardAmountFromCardCollection(DefaultCollectionDataDB pCardData, UserDB pUserDB)
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return;

        int cardAmount = await GetUserCardCollectionCardAmount(pCardData, pUserDB);
        if (cardAmount <= 1)
        {
            await DatosFirebaseRTHelper.Instance.reference.Child(UsersCardCollectionTable)
                                                          .Child(pUserDB.Name.ToLower())
                                                          .Child(pCardData.ID).SetValueAsync(null);
        }
        else
        {
            cardAmount--;
            await DatosFirebaseRTHelper.Instance.reference.Child(UsersCardCollectionTable)
                                                          .Child(pUserDB.Name.ToLower())
                                                          .Child(pCardData.ID)
                                                          .UpdateChildrenAsync(new Dictionary<string, object> { { "Amount", cardAmount } });
        }

        UpdateLastUserCardCollectionModifyUpdateTimestamp(pUserDB);
    }

    public async Task<int> GetUserCardCollectionCardAmount(DefaultCollectionDataDB pCardData, UserDB pUserDB)
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return 0;

        DataSnapshot dtSnapshot = null;
        await FirebaseDatabase.DefaultInstance.GetReference(UsersCardCollectionTable)
                                              .Child(pUserDB.Name.ToLower())
                                              .Child(pCardData.ID).GetValueAsync().ContinueWith(task =>
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
        DefaultCollectionDataDB utcLastGCCDownload = new DefaultCollectionDataDB();
        if (dtSnapshot != null)
        {
            if (dtSnapshot.Exists)
            {
                utcLastGCCDownload = JsonUtility.FromJson<DefaultCollectionDataDB>(dtSnapshot.GetRawJsonValue());
            }
        }

        return utcLastGCCDownload.Amount;
    }
}
