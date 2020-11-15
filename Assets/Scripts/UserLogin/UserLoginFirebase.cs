using UnityEngine;
using System.Security.Cryptography;
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System;

public class UserLoginFirebase : MonoBehaviour
{
    private const string usersTable = "Users";
    private const string DefaultCollectionTable = "DefaultCollection";
    private const string UsersCardCollectionTable = "UsersCardCollection";
    private FirebaseUser user;

    public async Task<UserDB> UserLoginMultipleInterface(string ussername, string password = "123456", string email = "test@test.com")
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return null;

        UserDB logedUser = await LoginUserWithFirebaseRTCustomLogin(ussername, password);

        if (logedUser == null)
        {
            Debug.Log("No User");
        }
        else
        {
            Debug.Log("Login Succesfull");
        }

        StartCoroutine(LoginUserWithAuthentication(email, password));

        return logedUser;
    }

    private async Task<UserDB> LoginUserWithFirebaseRTCustomLogin(string ussername, string password)
    {
        // CHEQUEAR SI HAY UN USUARIO CON EL MISMO NOMBRE
        DataSnapshot userNameExist = await PlayerDataSnapshotNameExist(ussername);
        UserDB logedUser = null;
        if (userNameExist != null)
        {
            if (userNameExist.Exists)
            {
                UserDB user = JsonUtility.FromJson<UserDB>(userNameExist.GetRawJsonValue());

                if (HelperUserData.isCorrectPassword(user.Salt, user.Password, password, new SHA256Managed()))
                {
                    Debug.Log("PASSWORD CORRECT");
                    Debug.Log("user.Name.ToLower() " + user.Name.ToLower());
                    logedUser = user;
                    if (user.IsFirstTime == true)
                    {
                        CardCollection.Instance.CreateNewUserCollections(user);
                        ShopManager.Instance.CreateNewUserResources(user);
                    }
                }
                else
                {
                    Debug.Log("PASSWORD INCORRECT");
                }
            }
            else
            {
                Debug.Log("NAME INCORRECT");
            }
        }
        return logedUser;
    }

    private IEnumerator LoginUserWithAuthentication(string email, string password)
    {
        var LoginTask = DatosFirebaseRTHelper.Instance.auth.SignInWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            Debug.LogWarning(message: $"failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Login Failed";
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
                    break;
            }
            Debug.Log(message);
        }
        else
        {
            user = LoginTask.Result;
            Debug.LogFormat("User signed in succesfully: {0} ({1})", user.DisplayName, user.Email);
            Debug.Log("Logged IN");
        }
    }

    private async Task<DataSnapshot> PlayerDataSnapshotNameExist(string name)
    {
        DataSnapshot dtSnapshot = null;
        await FirebaseDatabase.DefaultInstance.GetReference(usersTable).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                //Debug.Log("NoChild");
                // Handle the error...
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (var child in snapshot.Children)
                {
                    if (child.HasChild("Name"))
                    {
                        if (child.Child("Name").Value.ToString().ToLower() == name.ToLower())
                        {
                            //Debug.Log("NAME FAUND");
                            dtSnapshot = child;
                        }
                    }
                }
            }
        });

        return dtSnapshot;
    }

    private async Task<List<DefaultCollectionDataDB>> GetAndSetDefaultCardCollection()
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return null;

        List<DefaultCollectionDataDB> allCardList = new List<DefaultCollectionDataDB>();

        await FirebaseDatabase.DefaultInstance.GetReference(DefaultCollectionTable).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                //Debug.Log("NoChild");
                // Handle the error...
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (var child in snapshot.Children)
                {
                    DefaultCollectionDataDB card = JsonUtility.FromJson<DefaultCollectionDataDB>(child.GetRawJsonValue());
                    allCardList.Add(card);
                }
            }
        });

        return allCardList;
    }
}

