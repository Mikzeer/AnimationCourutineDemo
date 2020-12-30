using UnityEngine;
using Firebase.Database;
using System.Threading.Tasks;
using System.Collections.Generic;

public class FbGameCardCollection
{
    public async Task<List<CardDataRT>> GetGameCardCollection(UserDB pUser)
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return null;

        List<CardDataRT> allCardList = new List<CardDataRT>();

        DatosFirebaseRTHelper.Instance.reference.Child(DatosFirebaseRTHelper.Instance.cardsTable).KeepSynced(true);
        await DatosFirebaseRTHelper.Instance.reference.Child(DatosFirebaseRTHelper.Instance.cardsTable).GetValueAsync().ContinueWith(task =>
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
            FbGameCardCollectionUpdater fbGameCollUpd = new FbGameCardCollectionUpdater();
            fbGameCollUpd.UpdateLastGameCardCollectionDownloadTimestamp(pUser);
        }

        return allCardList;
    }

    public async Task<List<CardDataRT>> GetGameCardCollection()
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return null;

        List<CardDataRT> allCardList = new List<CardDataRT>();

        DatosFirebaseRTHelper.Instance.reference.Child(DatosFirebaseRTHelper.Instance.cardsTable).KeepSynced(true);
        await DatosFirebaseRTHelper.Instance.reference.Child(DatosFirebaseRTHelper.Instance.cardsTable).GetValueAsync().ContinueWith(task =>
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

    public void UpdateLastGameCardCollectionUpdateTOERASELATERJUSTTOTEST()
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return;

        DatosFirebaseRTHelper.Instance.reference.Child(DatosFirebaseRTHelper.Instance.GameCardCollectionLastUpdateTable).UpdateChildrenAsync(
            new Dictionary<string, object> { { "uctCreatedUnix", ServerValue.Timestamp } });
    }
}
