using System;
using UnityEngine;

public class AutoLoginManager
{
    FbUserLogin fbUserLogin;
    Action<string> OnUserLoginErrorMesage;
    HelperCardCollectionJsonKimboko helperCardCollectionJsonKimboko;

    public AutoLoginManager(Action<string> OnUserLoginErrorMesage)
    {
        this.OnUserLoginErrorMesage = OnUserLoginErrorMesage;
    }

    public async void AutoLoginUser()
    {
        fbUserLogin = new FbUserLogin();

        helperCardCollectionJsonKimboko = new HelperCardCollectionJsonKimboko();
        ConfigurationData cnfDat = helperCardCollectionJsonKimboko.GetConfigurationDataFromJson();

        if (cnfDat == null)
        {
            GameSceneManager.Instance.SetActiveWaitForLoad(false);
            return;
        }

        if (cnfDat.autoLogin == false)
        {
            GameSceneManager.Instance.SetActiveWaitForLoad(false);
            return;
        }

        UserDB logedUser = await fbUserLogin.UserAutoLoginMultipleInterface(cnfDat.user.Name, OnUserLoginErrorMesage, cnfDat.user.Password, cnfDat.password, cnfDat.email);

        if (logedUser == null)
        {
            Debug.Log("Logged User NULL");
            GameSceneManager.Instance.SetActiveWaitForLoad(false);
            return;
        }

        GameSceneManager.Instance.SetActiveWaitForLoad(false);
        GameSceneManager.Instance.LoadSceneAsync(GameSceneManager.GAMESCENE.MAINMENU);
    }
}