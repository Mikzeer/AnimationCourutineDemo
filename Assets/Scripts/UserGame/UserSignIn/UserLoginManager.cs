using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using UnityEngine;

public class UserLoginManager
{
    FbUserLogin fbUserLogin;
    Action<string> OnUserLoginErrorMesage;

    HelperCardCollectionJsonKimboko helperCardCollectionJsonKimboko;

    public UserLoginManager(Action<string> OnUserLoginErrorMesage)
    {
        this.OnUserLoginErrorMesage = OnUserLoginErrorMesage;
    }

    public void OnTryLoginUser(UserRegistrationData usRegData)
    {
        GameSceneManager.Instance.SetActiveWaitForLoad(true);
        LoginUser(usRegData);
    }

    private async void LoginUser(UserRegistrationData usRegData)
    {
        fbUserLogin = new FbUserLogin();
        UserDB logedUser = await fbUserLogin.UserLoginMultipleInterface(usRegData.UserName, OnUserLoginErrorMesage, usRegData.Pass, usRegData.Email);

        if (logedUser == null)
        {
            Debug.Log("Logged User NULL");
            GameSceneManager.Instance.SetActiveWaitForLoad(false);
            return;
        }
        ConfigurationData configurationData = new ConfigurationData();

        configurationData.user = logedUser;
        configurationData.autoLogin = usRegData.autoLogin;
        configurationData.email = usRegData.Email;
        configurationData.password = usRegData.Pass;

        helperCardCollectionJsonKimboko = new HelperCardCollectionJsonKimboko();
        helperCardCollectionJsonKimboko.SetConfigurationDataToJson(configurationData);

        GameSceneManager.Instance.SetActiveWaitForLoad(false);
        GameSceneManager.Instance.LoadSceneAsync(GameSceneManager.GAMESCENE.MAINMENU);
    }

}
