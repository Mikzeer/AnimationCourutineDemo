using UnityEngine;
using Firebase.Database;
using System.Threading.Tasks;
using System.Collections.Generic;

public class FbUserCardCollectionCreation
{
    private string isFirstTime = "IsFirstTime";

    public async Task<List<DefaultCollectionDataDB>> CreateNewUserCollection(UserDB user)
    {
        List<DefaultCollectionDataDB> allCardList = await GetDefaultCardCollection();
        
        if (allCardList != null && allCardList.Count > 0)
        {
            // SET ISFIRSTTIME TO FALSE
            
            await DatosFirebaseRTHelper.Instance.reference.Child(DatosFirebaseRTHelper.Instance.usersTable)
                                                          .Child(user.Name.ToLower())
                                                          .Child(isFirstTime).SetValueAsync(false);

            // SET DEFAULT COLLECTION IN THE USERS COLLECTION IN DB
            foreach (DefaultCollectionDataDB dcData in allCardList)
            {
                string json = JsonUtility.ToJson(dcData);
                await DatosFirebaseRTHelper.Instance.reference.Child(DatosFirebaseRTHelper.Instance.UsersCardCollectionTable)
                                                              .Child(user.Name.ToLower())
                                                              .Child(dcData.ID).SetRawJsonValueAsync(json);
            }

            FbUserCardCollectionUpdater userCollUpd = new FbUserCardCollectionUpdater();
            userCollUpd.UpdateLastUserCardCollectionDownloadTimestamp(user);
        }

        return allCardList;
    }

    public async Task<List<DefaultCollectionDataDB>> GetDefaultCardCollection()
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return null;

        List<DefaultCollectionDataDB> allCardList = new List<DefaultCollectionDataDB>();
        DatosFirebaseRTHelper.Instance.reference.Child(DatosFirebaseRTHelper.Instance.DefaultCollectionTable).KeepSynced(true);        
        await DatosFirebaseRTHelper.Instance.reference.Child(DatosFirebaseRTHelper.Instance.DefaultCollectionTable).GetValueAsync().ContinueWith(task =>
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
}
