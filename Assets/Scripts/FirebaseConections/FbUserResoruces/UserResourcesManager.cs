using System.Threading.Tasks;

public class UserResourcesManager
{
    FbUserResources fbUserResources;
    FbGamePriceData fbGamePriceData;
    FbUserResourcesPacks fbUserResourcesPacks;
    UserResources resources;
    GamePricesData prices;

    public UserResourcesManager()
    {
        fbUserResources = new FbUserResources();
        fbGamePriceData = new FbGamePriceData();
        fbUserResourcesPacks = new FbUserResourcesPacks();
    }

    public void CreateNewUserResources(UserDB pUser)
    {
        fbUserResources.CreateNewUserResources(pUser);
    }

    public async Task<UserResources> LoadUserResourcesFromFirebase(UserDB pUser)
    {
        UserResources userResources = await fbUserResources.GetUserResources(pUser);
        resources = userResources;
        return userResources;
    }

    public async Task<GamePricesData> LoadPriceDataFromFirebase()
    {
        GamePricesData priceData = await fbGamePriceData.GetGamePricesData();
        prices = priceData;
        return priceData;
    }

    public void BuyPackDB(UserDB user)
    {
        fbUserResourcesPacks.BuyNewPack(user, CARDPACKTYPE.NORMAL);
    }

    public async Task<bool> CanUserBuyAPackANormalPack(UserDB user)
    {
        bool canOpen = await fbUserResourcesPacks.IsUserAllowToBuyAPack(user, CARDPACKTYPE.NORMAL);
        return canOpen;
    }

    public void RestOneOpenPackFromFirebase(UserDB pUser)
    {
        fbUserResourcesPacks.SetNewUnopenPackAmountToUser(pUser, -1);
    }

}

