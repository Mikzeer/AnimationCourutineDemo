using System.Threading.Tasks;
using System.Collections.Generic;

public class FbUserResourcesGold
{
    private string resources = "Resources";
    private string gold = "Gold";
    public async void SetNewGoldAmountToUser(UserDB pUserDB, int amountToAddOrRest)
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return;

        int goldAmount = await GetUserGoldAmount(pUserDB);
        goldAmount += amountToAddOrRest;

        await DatosFirebaseRTHelper.Instance.reference.Child(DatosFirebaseRTHelper.Instance.UsersResourcesTable)
                                                      .Child(pUserDB.Name.ToLower())
                                                      .Child("Resources")
                                                      .UpdateChildrenAsync(new Dictionary<string, object> { { "Gold", goldAmount } });
    }

    public async Task<int> GetUserGoldAmount(UserDB pUserDB)
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return 0;
        FbUserResources fbUserResources = new FbUserResources();
        UserResources userResources = await fbUserResources.GetUserResources(pUserDB);

        if (userResources != null)
        {
            return userResources.Gold;
        }
        else
        {
            return 0;
        }
    }

    public async void SetGoldAfterWiningAMatch(UserDB pUserDB)
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return;

        int goldAmount = await GetUserGoldAmount(pUserDB);
        FbRewardsData rewData = new FbRewardsData();
        GameRewardsData gameRewards = await rewData.GetGameRewardsData();
        int goldAfterMatch = gameRewards.GoldWinMatch;
        goldAmount += goldAfterMatch;

        await DatosFirebaseRTHelper.Instance.reference.Child(DatosFirebaseRTHelper.Instance.UsersResourcesTable)
                                                      .Child(pUserDB.Name.ToLower())
                                                      .Child(resources)
                                                      .UpdateChildrenAsync(new Dictionary<string, object> { { gold, goldAmount } });
    }
}

