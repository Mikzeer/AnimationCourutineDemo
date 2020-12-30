using UnityEngine;
using Firebase.Database;
using System.Threading.Tasks;
using System.Collections.Generic;

public class FbUserCardCollectionUpdater
{
    private string userLastDownload = "utcLastDownloadUserCollectionUnix";
    private string userLastModification = "utcLastModificationUserCollectionUnix";

    public async void UpdateLastUserCardCollectionDownloadTimestamp(UserDB userDB)
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return;

        await DatosFirebaseRTHelper.Instance.reference.Child(DatosFirebaseRTHelper.Instance.usersTable)
                                                      .Child(userDB.Name.ToLower()).UpdateChildrenAsync(
                                                      new Dictionary<string, object>
                                                      {
                                                          { userLastDownload, ServerValue.Timestamp }
                                                      });

        long lastUpdate = await GetLastUserCardCollectionDownloadTimestampUser(userDB.Name.ToLower());
        //Debug.Log("LONG inside " + lastUpdate);

        HelperCardCollectionJsonKimboko helperCardCollectionJsonKimboko = new HelperCardCollectionJsonKimboko();
        helperCardCollectionJsonKimboko.SetLastUserCollectionUpdateToJson(lastUpdate);
    }

    public async Task<long> GetLastUserCardCollectionDownloadTimestampUser(string name)
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return 0;

        FbUserChecker fbUser = new FbUserChecker();
        DataSnapshot userNameExist = await fbUser.UserDataSnapshotExistByName(name.ToLower());

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

    public async void UpdateLastUserCardCollectionModifyUpdateTimestamp(UserDB userDB)
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return;

        await DatosFirebaseRTHelper.Instance.reference.Child(DatosFirebaseRTHelper.Instance.usersTable)
                                                      .Child(userDB.Name.ToLower()).UpdateChildrenAsync(
            new Dictionary<string, object>
            {
                { userLastModification, ServerValue.Timestamp }
            });
    }

    public async Task<long> GetLastUserCardCollectioModificationTimestampUser(string name)
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false)
        {
            //Debug.Log("FIREBASE NOT INITIALIZE");
            return 0;
        }

        //FirebaseDatabase.DefaultInstance.GetReference("Users").Child(name).KeepSynced(true);

        FbUserChecker fbUser = new FbUserChecker();
        DataSnapshot userNameExist = await fbUser.UserDataSnapshotExistByName(name.ToLower());

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
}
