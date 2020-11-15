using UnityEngine;
using Firebase.Database;
using System.Threading.Tasks;
using System.Collections.Generic;

public class UserResourcesFirebase : MonoBehaviour
{
    private const string GamePricesTable = "GamePrices";
    private const string GameRewardsTable = "GameRewards";
    private const string UsersResourcesTable = "UsersResources";
    private const string StartingResourcesTable = "StartingResources";

    public async void CreateNewUserResources(UserDB user)
    {
        UserResources userResources = await GetStartingResources();

        if (userResources != null)
        {
            string json = JsonUtility.ToJson(userResources);
            //await DatosFirebaseRTHelper.Instance.reference.Child(UsersResourcesTable).SetRawJsonValueAsync(user.Name.ToLower());
            await DatosFirebaseRTHelper.Instance.reference.Child(UsersResourcesTable).Child(user.Name.ToLower()).Child("Resources").SetRawJsonValueAsync(json);
        }
    }

    public async Task<UserResources> GetUserResources(UserDB pUser)
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return null;
        UserResources userResources = new UserResources();
        await FirebaseDatabase.DefaultInstance.GetReference(UsersResourcesTable).Child(pUser.Name.ToLower()).Child("Resources").GetValueAsync().ContinueWith(task =>
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

    public async Task<UserResources> GetStartingResources()
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return null;
        UserResources userResources = new UserResources();
        await FirebaseDatabase.DefaultInstance.GetReference(StartingResourcesTable).Child("Resources").GetValueAsync().ContinueWith(task =>
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

    public async void BuyNewPack(UserDB pUserDB, CARDPACKTYPE cardPackType)
    {
        GamePricesData priceData = await GetGamePricesData();
        Debug.Log("BUY NEW PACK USER RESORUCES");

        switch (cardPackType)
        {
            case CARDPACKTYPE.NORMAL:
                SetNewGoldAmountToUser(pUserDB, -priceData.NormalPackPrices);
                SetNewUnopenPackAmountToUser(pUserDB, 1);
                break;
            case CARDPACKTYPE.SPECIAL:
                SetNewGoldAmountToUser(pUserDB, -1000);
                break;
            default:
                break;
        }
    }

    public async void SetNewGoldAmountToUser(UserDB pUserDB, int amountToAddOrRest)
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return;

        int goldAmount = await GetUserGoldAmount(pUserDB);
        goldAmount += amountToAddOrRest;

        await DatosFirebaseRTHelper.Instance.reference.Child(UsersResourcesTable)
                                                      .Child(pUserDB.Name.ToLower())
                                                      .Child("Resources")
                                                      .UpdateChildrenAsync(new Dictionary<string, object> { { "Gold", goldAmount } });
    }

    public async void SetGoldAfterWiningAMatch(UserDB pUserDB)
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return;

        int goldAmount = await GetUserGoldAmount(pUserDB);
        GameRewardsData gameRewards = await GetGameRewardsData();
        int goldAfterMatch = gameRewards.GoldWinMatch;
        goldAmount += goldAfterMatch;

        await DatosFirebaseRTHelper.Instance.reference.Child(UsersResourcesTable)
                                                      .Child(pUserDB.Name.ToLower())
                                                      .Child("Resources")
                                                      .UpdateChildrenAsync(new Dictionary<string, object> { { "Gold", goldAmount } });
    }

    public async void SetNewUnopenPackAmountToUser(UserDB pUserDB, int amountToAddOrRest)
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return;

        int unopenPacks = await GetUserUnopenPacksAmount(pUserDB);
        unopenPacks += amountToAddOrRest;

        await DatosFirebaseRTHelper.Instance.reference.Child(UsersResourcesTable)
                                                      .Child(pUserDB.Name.ToLower())
                                                      .Child("Resources")
                                                      .UpdateChildrenAsync(new Dictionary<string, object> { { "UnopendPacks", unopenPacks } });
    }

    public async Task<int> GetUserGoldAmount(UserDB pUserDB)
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return 0;

        UserResources userResources = await GetUserResources(pUserDB);

        if (userResources!=null)
        {
            return userResources.Gold;
        }
        else
        {
            return 0;
        }
    }

    public async Task<int> GetUserUnopenPacksAmount(UserDB pUserDB)
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return 0;

        UserResources userResources = await GetUserResources(pUserDB);

        if (userResources != null)
        {
            return userResources.UnopendPacks;
        }
        else
        {
            return 0;
        }
    }

    public async Task<GamePricesData> GetGamePricesData()
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return null;

        DataSnapshot dtSnapshot = null;
        await FirebaseDatabase.DefaultInstance.GetReference(GamePricesTable).GetValueAsync().ContinueWith(task =>
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

    public async Task<GameRewardsData> GetGameRewardsData()
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return null;

        DataSnapshot dtSnapshot = null;
        await FirebaseDatabase.DefaultInstance.GetReference(GameRewardsTable).GetValueAsync().ContinueWith(task =>
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

    public async Task<bool> IsUserAllowToBuyAPack(UserDB pUserDB, CARDPACKTYPE cardPackType)
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return false;

        int userGold = await GetUserGoldAmount(pUserDB);
        GamePricesData priceData = await GetGamePricesData();

        Debug.Log("CAN TRY USER GOLD " + userGold);
        Debug.Log("CAN TRY PRICE DATA " + priceData.NormalPackPrices);

        switch (cardPackType)
        {
            case CARDPACKTYPE.NORMAL:
                if (userGold < priceData.NormalPackPrices)
                {
                    return false;
                }
                else
                {
                    return true;
                }                
            case CARDPACKTYPE.SPECIAL:
                break;
            default:
                break;
        }

        return false;
    }
}

