using UnityEngine;
using Firebase;
using Firebase.Auth;
using System.Threading.Tasks;
using System;

public class FbUserRegistration
{
    string userName;
    string json;
    public async Task CreateNewUser(UserDB userDB, string password, string email, Action<string> OnErrorCallback)
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return;

        bool isDataCorrect = await CheckIfDataIsCorrect(userDB.Name, OnErrorCallback);
        if (isDataCorrect)
        {
            //Every time you call Push(), Firebase generates a unique key that can also be used as a unique identifier,
            // such as user-scores/<user-id>/<unique-score-id>.
            string key = DatosFirebaseRTHelper.Instance.reference.Child(DatosFirebaseRTHelper.Instance.usersTable).Push().Key;
            userDB.ID = key;
            json = JsonUtility.ToJson(userDB);
            userName = userDB.Name.ToLower();

            RegisterUserWithAuthentication(email, userDB.Name, password, OnErrorCallback);
        }
    }

    private async Task<bool> CheckIfDataIsCorrect(string name, Action<string> OnErrorCallback)
    {
        // CHEQUEAR SI HAY ALGUN CARACTER QUE NO SE PERMITE
        // CHEQUEAR QUE NO SEA UN NOMBRE OFENSIVO DE USUARIO
        // CHEQUEAR QUE EL NOMBRE TENGA MAS DE UNA CANTIDAD DETERMINADA DE CARACTERES

        // CHEQUEAR SI HAY UN USUARIO CON EL MISMO NOMBRE
        FbUserChecker userChk = new FbUserChecker();
        bool us = await userChk.IsThisNameAvailable(name);

        if (us)
        {
            OnErrorCallback?.Invoke("User Name Not Valid");            
            return false;
        }

        return true;
    }

    private void PostUserWithFireBaseRT(string key, string json)
    {
        DatosFirebaseRTHelper.Instance.reference.Child(DatosFirebaseRTHelper.Instance.usersTable).Child(key).SetRawJsonValueAsync(json);
    }

    private async void RegisterUserWithAuthentication(string email, string username, string password, Action<string> OnErrorCallback)
    {
        FirebaseUser user = null;
        string message = "Login Failed";
        await DatosFirebaseRTHelper.Instance.auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                message = "Mail or User Not Available";
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
                            message = "Error Code " + errorCode.ToString();
                            break;
                    }
                }
                else
                {
                    user = task.Result;
                    if (user != null)
                    {
                        UpdateUserProfile(user);
                    }
                }
            }
        });

        if (user == null)
        {
            OnErrorCallback?.Invoke(message);
        }
    }

    public async void UpdateUserProfile(FirebaseUser user)
    {
        UserProfile profile = new UserProfile { DisplayName = user.DisplayName };
        await user.UpdateUserProfileAsync(profile).ContinueWith(profileTask =>
        {
            if (profileTask.IsFaulted)
            {
            }
            else if (profileTask.IsCompleted)
            {
                if (profileTask.Exception != null)
                {
                    Debug.LogWarning(message: $"failed to register task with {profileTask.Exception}");
                    FirebaseException firebaseEx = profileTask.Exception.GetBaseException() as FirebaseException;
                    AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                    Debug.Log("Username set failed");
                }
                else
                {
                    Debug.Log("ALL OK WITH AUTHENTICATION");
                    PostUserWithFireBaseRT(userName, json);
                }
            }
        });
    }
}

