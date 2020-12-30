using UnityEngine;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System;

public class UserRegistrationManager
{
    FbUserRegistration fbUserRegistration;
    Action<string> OnUserRegistrationErrorMesage;

    public UserRegistrationManager(Action<string> OnUserRegistrationErrorMesage)
    {
        this.OnUserRegistrationErrorMesage = OnUserRegistrationErrorMesage;
    }

    public void OnTryCreateUser(UserRegistrationData usRegData)
    {
        if (IsUserRegistrationDataComplete(usRegData))
        {
            GameSceneManager.Instance.SetActiveWaitForLoad(true);
            RegistrerNewUser(usRegData);
            OnUserRegistrationErrorMesage("REGISTRATION COMPLETE");
        }
    }

    private async void RegistrerNewUser(UserRegistrationData usRegData)
    {
        fbUserRegistration = new FbUserRegistration();
        string macAddres = HelperUserData.GetMacAddress();
        string localIP = HelperUserData.GetLocalIPAddress();
        HashWithSaltResult hashSalt = HelperUserData.HashWithSalt(usRegData.Pass, 32, new SHA256Managed());
        string pSalt = hashSalt.Salt;
        string hasPass = hashSalt.Digest;
        UserDB userDB = new UserDB(usRegData.UserName, macAddres, pSalt, hasPass);
        await fbUserRegistration.CreateNewUser(userDB, usRegData.Pass, usRegData.Email, OnUserRegistrationErrorMesage);
        GameSceneManager.Instance.SetActiveWaitForLoad(false);
    }

    private bool IsUserRegistrationDataComplete(UserRegistrationData usRegData)
    {
        // CHEQUEAR QUE EL NOMBRE TENGA MAS DE UNA CANTIDAD DETERMINADA DE CARACTERES
        // CHEQUEAR SI HAY ALGUN CARACTER QUE NO SE PERMITE
        // CHEQUEAR QUE NO SEA UN NOMBRE OFENSIVO DE USUARIO
        if (usRegData.UserName.Length < 8)
        {
            OnUserRegistrationErrorMesage?.Invoke("Your name is to short, must be at least  8 characters long");
            return false;
        }
        if (usRegData.Pass.Length < 6)
        {
            OnUserRegistrationErrorMesage?.Invoke("Your name is to short, must be at least  8 characters long");
            return false;
        }
        return true;
    }
    
}