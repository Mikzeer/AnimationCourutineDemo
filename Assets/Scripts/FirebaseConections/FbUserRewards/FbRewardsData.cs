using UnityEngine;
using Firebase.Database;
using System.Threading.Tasks;

public class FbRewardsData
{
    public async Task<GameRewardsData> GetGameRewardsData()
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return null;

        DataSnapshot dtSnapshot = null;
        DatosFirebaseRTHelper.Instance.reference.Child(DatosFirebaseRTHelper.Instance.GameRewardsTable).KeepSynced(true);
        await DatosFirebaseRTHelper.Instance.reference.Child(DatosFirebaseRTHelper.Instance.GameRewardsTable).GetValueAsync().ContinueWith(task =>
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

        GameRewardsData gameRewardData = new GameRewardsData();
        if (dtSnapshot != null)
        {
            if (dtSnapshot.Exists)
            {
                gameRewardData = JsonUtility.FromJson<GameRewardsData>(dtSnapshot.GetRawJsonValue());
            }
        }

        return gameRewardData;
    }
}

