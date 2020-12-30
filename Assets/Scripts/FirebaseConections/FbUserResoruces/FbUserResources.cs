using UnityEngine;
using Firebase.Database;
using System.Threading.Tasks;

public class FbUserResources
{
    private string resources = "Resources";

    public async void CreateNewUserResources(UserDB user)
    {
        UserResources userResources = await GetStartingResources();

        if (userResources != null)
        {
            string json = JsonUtility.ToJson(userResources);
            await DatosFirebaseRTHelper.Instance.reference.Child(DatosFirebaseRTHelper.Instance.UsersResourcesTable)
                                                          .Child(user.Name.ToLower())
                                                          .Child(resources).SetRawJsonValueAsync(json);
        }
    }

    public async Task<UserResources> GetStartingResources()
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return null;
        UserResources userResources = new UserResources();
        DatosFirebaseRTHelper.Instance.reference.Child(DatosFirebaseRTHelper.Instance.StartingResourcesTable).KeepSynced(true);
        await FirebaseDatabase.DefaultInstance.GetReference(DatosFirebaseRTHelper.Instance.StartingResourcesTable)
                                              .Child(resources).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                //Debug.Log("NoChild");
                // Handle the error...
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                userResources = JsonUtility.FromJson<UserResources>(snapshot.GetRawJsonValue());
            }
        });
        return userResources;
    }

    public async Task<UserResources> GetUserResources(UserDB pUser)
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return null;
        UserResources userResources = new UserResources();

        DatosFirebaseRTHelper.Instance.reference.Child(DatosFirebaseRTHelper.Instance.UsersResourcesTable)
                                                .Child(pUser.Name.ToLower()).KeepSynced(true);
        await DatosFirebaseRTHelper.Instance.reference.Child(DatosFirebaseRTHelper.Instance.UsersResourcesTable)
                                                      .Child(pUser.Name.ToLower())
                                                      .Child(resources).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                //Debug.Log("NoChild");
                // Handle the error...
                userResources = null;
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                userResources = JsonUtility.FromJson<UserResources>(snapshot.GetRawJsonValue());
            }
        });
        return userResources;
    }
}

