﻿using UnityEngine;
using Firebase.Database;
using System.Threading.Tasks;
using System.Collections.Generic;

public class CardCollectionFirebase : MonoBehaviour
{
    private const string usersTable = "Users";
    private const string cardsTable = "Cards";
    private const string UsersCardCollectionTable = "UsersCardCollection";
    private const string DefaultCollectionTable = "DefaultCollection";
    private const string GameCardCollectionLastUpdateTable = "GameCollectionLastUpdate";

    public async Task<List<DefaultCollectionDataDB>> CreateNewUserCollection(UserDB user)
    {       
        List<DefaultCollectionDataDB> allCardList = await GetAndSetDefaultCardCollection();

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

            //long lastUpdate = await GetLastGameCardCollectionDownloadTimestampUser(user.Name.ToLower());
            //CardCollection.Instance.SetLastUserCollectionUpdateToJson(lastUpdate);
        }
        //long milliseconds;
        //if (long.TryParse(user.utcLastDownloadGameCollectionUnix.ToString(), out milliseconds))
        //{
        //    long utcCreatedTimestamp = milliseconds;
        //    DateTime createdDate = Helper.UnixTimeStampToDateTimeMiliseconds(utcCreatedTimestamp);
        //}
        //CardCollection.Instance.GetLastGameCollectionUpdateFromJsonDateTime();

        //NO HACE FALTA CARGA LA USER CARD COLLECTION YA QUE ESTA FUNCION DEVUELVE LA CARD LIST QUE SE NECESITA PARA ESO
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
            //long lastUpdate = await GetLastGameCardCollectionDownloadTimestampUser(pUser.Name.ToLower());
            //CardCollection.Instance.SetLastGameCollectionUpdateToJson(lastUpdate);
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
            //long lastUpdate = await GetLastGameCardCollectionDownloadTimestampUser(pUser.Name.ToLower());
            //CardCollection.Instance.SetLastGameCollectionUpdateToJson(lastUpdate);
        }

        return allCardList;
    }

    public void UpdateLastGameCardCollectionDownloadTimestamp(UserDB userDB)
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return;

        DatosFirebaseRTHelper.Instance.reference.Child("Users").Child(userDB.Name.ToLower()).UpdateChildrenAsync(
            new Dictionary<string, object> { { "utcLastDownloadGameCollectionUnix", ServerValue.Timestamp } });


        //string timestampAdd = @"timestamp"": {"".sv"" : ""timestamp""} } ";
        //reference.Child("Users").Child("new1").UpdateChildrenAsync(new Dictionary<string, object> { { "utcLastDownloadCollectionUnix", ServerValue.Timestamp } , { "utcLastDownloadOwnedCards", ServerValue.Timestamp } });
        //reference.Child("Users").Child("pepe").SetRawJsonValueAsync(timestampAdd);
    }

    public void UpdateLastUserCardCollectionDownloadTimestamp(UserDB userDB)
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return;

        DatosFirebaseRTHelper.Instance.reference.Child("Users").Child(userDB.Name.ToLower()).UpdateChildrenAsync(
            new Dictionary<string, object> { { "utcLastDownloadUserCollectionUnix", ServerValue.Timestamp } });
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

    public async Task<long> GetLastGameCardCollectionUpdateTimestamp()
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return 0;

        DataSnapshot dtSnapshot = null;
        await FirebaseDatabase.DefaultInstance.GetReference(GameCardCollectionLastUpdateTable).GetValueAsync().ContinueWith(task =>
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

    public async Task<List<DefaultCollectionDataDB>> GetAndSetDefaultCardCollection()
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


}