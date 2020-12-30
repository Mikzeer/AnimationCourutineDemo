using UnityEngine;
using System.Security.Cryptography;
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using System.Threading.Tasks;
using System;

public class FbUserLogin
{
    public async Task<UserDB> UserLoginMultipleInterface(string ussername, Action<string> OnErrorCallback, string password = "123456", string email = "test@test.com")
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return null;

        UserDB logedUser = await LoginUserWithFirebaseRTCustomLogin(ussername, password, OnErrorCallback);
        if (logedUser != null)
        {
            FirebaseUser user = await LoginUserWithAuthentication(email, password, OnErrorCallback);
            if (user != null)
            {
                logedUser.fbUser = user;
            }
        }
        return logedUser;
    }

    public async Task<UserDB> UserAutoLoginMultipleInterface(string ussername, Action<string> OnErrorCallback, string hasValue, string password, string email = "test@test.com")
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return null;

        UserDB logedUser = await AutoLoginUserWithFirebaseRTCustomLogin(ussername, hasValue, OnErrorCallback);
        if (logedUser != null)
        {
            FirebaseUser user = await LoginUserWithAuthentication(email, password, OnErrorCallback);
            if (user != null)
            {
                logedUser.fbUser = user;
            }
        }
        return logedUser;
    }

    private async Task<UserDB> AutoLoginUserWithFirebaseRTCustomLogin(string ussername, string password, Action<string> OnErrorCallback)
    {
        // CHEQUEAR SI HAY UN USUARIO CON EL MISMO NOMBRE
        FbUserChecker userChk = new FbUserChecker();
        DataSnapshot userNameExist = await userChk.UserDataSnapshotExistByName(ussername);
        UserDB logedUser = null;
        if (userNameExist != null && userNameExist.Exists)
        {
            UserDB user = JsonUtility.FromJson<UserDB>(userNameExist.GetRawJsonValue());

            if (HelperUserData.isCorrectPassword(user.Password, password))
            {
                Debug.Log("user.Name.ToLower() " + user.Name.ToLower());
                logedUser = user;
            }
            else
            {
                OnErrorCallback?.Invoke("PASSWORD INCORRECT SALT " + user.Salt + " PASS " + user.Password + " password " + password);
            }
        }
        else
        {
            OnErrorCallback?.Invoke("NAME INCORRECT");
        }
        return logedUser;
    }

    private async Task<UserDB> LoginUserWithFirebaseRTCustomLogin(string ussername, string password, Action<string> OnErrorCallback)
    {
        // CHEQUEAR SI HAY UN USUARIO CON EL MISMO NOMBRE
        FbUserChecker userChk = new FbUserChecker();
        DataSnapshot userNameExist = await userChk.UserDataSnapshotExistByName(ussername);
        UserDB logedUser = null;
        if (userNameExist != null && userNameExist.Exists)
        {
            UserDB user = JsonUtility.FromJson<UserDB>(userNameExist.GetRawJsonValue());

            if (HelperUserData.isCorrectPassword(user.Salt, user.Password, password, new SHA256Managed()))
            {
                Debug.Log("user.Name.ToLower() " + user.Name.ToLower());
                logedUser = user;
            }
            else
            {
                OnErrorCallback?.Invoke("PASSWORD INCORRECT SALT " + user.Salt + " PASS " + user.Password + " password " + password);
            }
        }
        else
        {
            OnErrorCallback?.Invoke("NAME INCORRECT");
        }
        return logedUser;
    }

    private async Task<FirebaseUser> LoginUserWithAuthentication(string email, string password, Action<string> OnErrorCallback)
    {
        FirebaseUser user = null;
        string message = "Login Failed";
        await DatosFirebaseRTHelper.Instance.auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                message = "Mail or Pass Invalid";
            }
            else if (task.IsCompleted)
            {
                if (task.Exception != null)
                {
                    Debug.LogWarning(message: $"failed to register task with {task.Exception}");
                    FirebaseException firebaseEx = task.Exception.GetBaseException() as FirebaseException;
                    AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                    switch (errorCode)
                    {
                        case AuthError.EmailAlreadyInUse:
                            break;
                        case AuthError.InvalidEmail:
                            message = "Invalid Email";
                            break;
                        case AuthError.WrongPassword:
                            message = "Wrong Password";
                            break;
                        case AuthError.MissingEmail:
                            message = "Missing Email";
                            break;
                        case AuthError.MissingPassword:
                            message = "Missing Password";
                            break;
                        case AuthError.UserNotFound:
                            message = "Account does not exist";
                            break;                            
                        default:
                            message = errorCode.ToString();
                            break;
                    }
                }
                else
                {
                    user = task.Result;
                    Debug.LogFormat("User signed in succesfully: {0} ({1})", user.DisplayName, user.Email);
                    Debug.Log("Logged IN");
                }
            }
        });

        if (user == null)
        {
            OnErrorCallback?.Invoke(message);
        }

        return user;
    }
}

