using Firebase.Database;
using System.Threading.Tasks;

public class FbUserChecker
{
    public async Task<DataSnapshot> UserDataSnapshotExistByName(string name)
    {
        DataSnapshot dtSnapshot = null;
        DatosFirebaseRTHelper.Instance.reference.Child(DatosFirebaseRTHelper.Instance.usersTable).Child(name).KeepSynced(true);
        await DatosFirebaseRTHelper.Instance.reference.Child(DatosFirebaseRTHelper.Instance.usersTable).Child(name).GetValueAsync().ContinueWith(task =>
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

    public async Task<bool> IsThisNameAvailable(string name)
    {
        bool exist = false;
        name = name.ToLower();

        DatosFirebaseRTHelper.Instance.reference.Child(DatosFirebaseRTHelper.Instance.usersTable).KeepSynced(true);

        await DatosFirebaseRTHelper.Instance.reference.Child(DatosFirebaseRTHelper.Instance.usersTable).OrderByKey().GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                exist = snapshot.Child(name).Exists;
            }
        });

        return exist;
    }

}