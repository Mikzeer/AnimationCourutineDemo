using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;

public class FbUserResourcesPacks
{
    private string resources = "Resources";
    private string unopenPacksstr = "UnopendPacks";

    public async void BuyNewPack(UserDB pUserDB, CARDPACKTYPE cardPackType)
    {
        FbGamePriceData fbPData = new FbGamePriceData();
        GamePricesData priceData = await fbPData.GetGamePricesData();
        Debug.Log("BUY NEW PACK USER RESORUCES");
        FbUserResourcesGold userGold = new FbUserResourcesGold();
        switch (cardPackType)
        {
            case CARDPACKTYPE.NORMAL:
                userGold.SetNewGoldAmountToUser(pUserDB, -priceData.NormalPackPrices);
                SetNewUnopenPackAmountToUser(pUserDB, 1);
                break;
            case CARDPACKTYPE.SPECIAL:
                userGold.SetNewGoldAmountToUser(pUserDB, -1000);
                break;
            default:
                break;
        }
    }

    public async void SetNewUnopenPackAmountToUser(UserDB pUserDB, int amountToAddOrRest)
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return;

        int unopenPacks = await GetUserUnopenPacksAmount(pUserDB);
        unopenPacks += amountToAddOrRest;

        await DatosFirebaseRTHelper.Instance.reference.Child(DatosFirebaseRTHelper.Instance.UsersResourcesTable)
                                                      .Child(pUserDB.Name.ToLower())
                                                      .Child(resources)
                                                      .UpdateChildrenAsync(new Dictionary<string, object> { { unopenPacksstr, unopenPacks } });
    }

    public async Task<int> GetUserUnopenPacksAmount(UserDB pUserDB)
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return 0;
        FbUserResources fbUserResources = new FbUserResources();
        UserResources userResources = await fbUserResources.GetUserResources(pUserDB);

        if (userResources != null)
        {
            return userResources.UnopendPacks;
        }
        else
        {
            return 0;
        }
    }

    public async Task<bool> IsUserAllowToBuyAPack(UserDB pUserDB, CARDPACKTYPE cardPackType)
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return false;

        FbGamePriceData pData = new FbGamePriceData();
        FbUserResourcesGold goldData = new FbUserResourcesGold();
        int userGold = await goldData.GetUserGoldAmount(pUserDB);
        GamePricesData priceData = await pData.GetGamePricesData();

        //Debug.Log("CAN TRY USER GOLD " + userGold);
        //Debug.Log("CAN TRY PRICE DATA " + priceData.NormalPackPrices);

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

