using UnityEngine;
using Firebase.Database;
using System.Threading.Tasks;
using System.Collections.Generic;

public class FbUserCardCollection
{
    private string cardAmountstr = "Amount";

    public async Task<List<DefaultCollectionDataDB>> GetUserCardCollection(UserDB pUser)
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return null;

        List<DefaultCollectionDataDB> allCardList = new List<DefaultCollectionDataDB>();
        //FirebaseDatabase.DefaultInstance.GetReference(UsersCardCollectionTable).Child(pUser.Name.ToLower())
        DatosFirebaseRTHelper.Instance.reference.Child(DatosFirebaseRTHelper.Instance.UsersCardCollectionTable)
                                                .Child(pUser.Name.ToLower()).KeepSynced(true);
        await DatosFirebaseRTHelper.Instance.reference.Child(DatosFirebaseRTHelper.Instance.UsersCardCollectionTable)
                                                      .Child(pUser.Name.ToLower()).GetValueAsync().ContinueWith(task =>
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
            FbUserCardCollectionUpdater userCollUpd = new FbUserCardCollectionUpdater();
            userCollUpd.UpdateLastUserCardCollectionDownloadTimestamp(pUser);        
        }

        return allCardList;
    }

    public async Task<bool> SetNewCardToUserCardCollection(DefaultCollectionDataDB pCardData, UserDB pUserDB)
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return false;

        int cardAmount = await GetUserCardCollectionCardAmount(pCardData, pUserDB);

        if (cardAmount == 0)
        {
            pCardData.Amount = 1;
            string json = JsonUtility.ToJson(pCardData);
            await DatosFirebaseRTHelper.Instance.reference.Child(DatosFirebaseRTHelper.Instance.UsersCardCollectionTable)
                                                          .Child(pUserDB.Name.ToLower())
                                                          .Child(pCardData.ID)
                                                          .SetRawJsonValueAsync(json);
        }
        else
        {
            cardAmount++;
            await DatosFirebaseRTHelper.Instance.reference.Child(DatosFirebaseRTHelper.Instance.UsersCardCollectionTable)
                                                          .Child(pUserDB.Name.ToLower())
                                                          .Child(pCardData.ID)
                                                          .UpdateChildrenAsync(new Dictionary<string, object> { { cardAmountstr, cardAmount } });
        }

        int newCardAmount = await GetUserCardCollectionCardAmount(pCardData, pUserDB);
        //Debug.Log("NEW CARD AMOUNT SETED ON DB CARD ID " + pCardData.ID + " Amount: " + cardAmount);
        if (cardAmount == newCardAmount)
        {
            //Debug.Log("IS THE SAME AMOUNT TRUE");
            FbUserCardCollectionUpdater userCollUpd = new FbUserCardCollectionUpdater();
            userCollUpd.UpdateLastUserCardCollectionModifyUpdateTimestamp(pUserDB);
            return true;
        }
        else
        {
            return false;
        }


    }

    public async Task<int> GetUserCardCollectionCardAmount(DefaultCollectionDataDB pCardData, UserDB pUserDB)
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return 0;

        DataSnapshot dtSnapshot = null;
        DatosFirebaseRTHelper.Instance.reference.Child(DatosFirebaseRTHelper.Instance.UsersCardCollectionTable)
                                                .Child(pUserDB.Name.ToLower())
                                                .Child(pCardData.ID).KeepSynced(true);

        await DatosFirebaseRTHelper.Instance.reference.Child(DatosFirebaseRTHelper.Instance.UsersCardCollectionTable)
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

    public async void RestCardAmountFromCardCollection(DefaultCollectionDataDB pCardData, UserDB pUserDB)
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return;

        int cardAmount = await GetUserCardCollectionCardAmount(pCardData, pUserDB);
        if (cardAmount <= 1)
        {
            await DatosFirebaseRTHelper.Instance.reference.Child(DatosFirebaseRTHelper.Instance.UsersCardCollectionTable)
                                                          .Child(pUserDB.Name.ToLower())
                                                          .Child(pCardData.ID).SetValueAsync(null);
        }
        else
        {
            cardAmount--;
            await DatosFirebaseRTHelper.Instance.reference.Child(DatosFirebaseRTHelper.Instance.UsersCardCollectionTable)
                                                          .Child(pUserDB.Name.ToLower())
                                                          .Child(pCardData.ID)
                                                          .UpdateChildrenAsync(new Dictionary<string, object> { { cardAmountstr, cardAmount } });
        }

        FbUserCardCollectionUpdater userCollUpd = new FbUserCardCollectionUpdater();
        userCollUpd.UpdateLastUserCardCollectionModifyUpdateTimestamp(pUserDB);
    }

}
