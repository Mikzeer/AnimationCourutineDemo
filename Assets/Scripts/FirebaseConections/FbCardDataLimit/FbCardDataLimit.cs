using UnityEngine;
using Firebase.Database;
using System.Threading.Tasks;

public class FbCardDataLimit
{
    public async Task<CardDataLimit> GetCardsLimitData(UserDB pUser)
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return null;

        CardDataLimit cardsLimitData = new CardDataLimit();

        DatosFirebaseRTHelper.Instance.reference.Child(DatosFirebaseRTHelper.Instance.CardsLimitDataTable).KeepSynced(true);
        await DatosFirebaseRTHelper.Instance.reference.Child(DatosFirebaseRTHelper.Instance.CardsLimitDataTable).GetValueAsync().ContinueWith(task =>
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

                FbCardDataLimitUpdater cLimitUpdt = new FbCardDataLimitUpdater();
                cLimitUpdt.UpdateLastUserCardLimitDataDownloadTimestamp(pUser);
            }
        });



        return cardsLimitData;
    }
}

