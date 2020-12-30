using UnityEngine;
using Firebase.Database;
using System.Threading.Tasks;

public class FbGamePriceData
{
    public async Task<GamePricesData> GetGamePricesData()
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return null;

        DataSnapshot dtSnapshot = null;
        DatosFirebaseRTHelper.Instance.reference.Child(DatosFirebaseRTHelper.Instance.GamePricesTable).KeepSynced(true);
        await DatosFirebaseRTHelper.Instance.reference.Child(DatosFirebaseRTHelper.Instance.GamePricesTable).GetValueAsync().ContinueWith(task =>
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

        GamePricesData gamePricesData = new GamePricesData();
        if (dtSnapshot != null)
        {
            if (dtSnapshot.Exists)
            {
                gamePricesData = JsonUtility.FromJson<GamePricesData>(dtSnapshot.GetRawJsonValue());
            }
        }

        return gamePricesData;
    }
}

