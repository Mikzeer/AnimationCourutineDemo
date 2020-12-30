using UnityEngine;
using Firebase.Database;
using System.Threading.Tasks;
using System.Collections.Generic;

public class FbGameCardCollectionUpdater
{
    private string gameLastDownload = "utcLastDownloadGameCollectionUnix";
    private bool debugOn = false;
    public void UpdateLastGameCardCollectionDownloadTimestamp(UserDB userDB)
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return;

        DatosFirebaseRTHelper.Instance.reference.Child(DatosFirebaseRTHelper.Instance.usersTable)
                                                .Child(userDB.Name.ToLower()).UpdateChildrenAsync(
            new Dictionary<string, object>
            {
                { gameLastDownload, ServerValue.Timestamp }
            });
    }

    public async Task<long> GetLastGameCardCollectionDownloadTimestampUser(string name)
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return 0;

        FbUserChecker fbUser = new FbUserChecker();
        DataSnapshot userNameExist = await fbUser.UserDataSnapshotExistByName(name.ToLower());

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

    public async Task<long> GetLastGameCardCollectionUpdateTimestamp()
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return 0;
        DataSnapshot dtSnapshot = null;

        DatosFirebaseRTHelper.Instance.reference.Child(DatosFirebaseRTHelper.Instance.GameCardCollectionLastUpdateTable)
                                                .Child("uctCreatedUnix").KeepSynced(true);
        await DatosFirebaseRTHelper.Instance.reference.Child(DatosFirebaseRTHelper.Instance.GameCardCollectionLastUpdateTable).GetValueAsync().ContinueWith(task =>
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
                if(debugOn) Debug.Log("LAST GAME UPDATE " + utcLastGCCDownload.uctCreatedUnix);
            }
        }

        return utcLastGCCDownload.uctCreatedUnix;
    }
}
