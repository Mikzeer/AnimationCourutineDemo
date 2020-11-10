using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using System.Threading.Tasks;
using System.Collections;

public class UserRegistrationFirebase : MonoBehaviour
{
    private const string usersTable = "Users";
    private FirebaseUser user;

    public async Task CreateNewUser(UserDB userDB, string password, string email)
    {
        if (DatosFirebaseRTHelper.Instance.isInit == false) return;

        bool isDataCorrect = await CheckIfDataIsCorrect(userDB.Name);
        if (isDataCorrect)
        {
            //Every time you call Push(), Firebase generates a unique key that can also be used as a unique identifier,
            // such as user-scores/<user-id>/<unique-score-id>.
            string key = DatosFirebaseRTHelper.Instance.reference.Child(usersTable).Push().Key;
            userDB.ID = key;
            string json = JsonUtility.ToJson(userDB);


            //string timestampAdd = @"timestamp"": {"".sv"" : ""timestamp""} } ";
            //reference.Child("Users").Child("new1").UpdateChildrenAsync(new Dictionary<string, object> { { "utcLastDownloadCollectionUnix", ServerValue.Timestamp } , { "utcLastDownloadOwnedCards", ServerValue.Timestamp } });
            //reference.Child("Users").Child("pepe").SetRawJsonValueAsync(timestampAdd);



            // TAL VEZ EL ID DE ESTO DEBERIA SER EL 
            PostUserWithFireBaseRT(userDB.Name.ToLower(), json);

            StartCoroutine(RegisterUserWithAuthentication(email, userDB.Name, password));
        }
    }

    private async Task<bool> CheckIfDataIsCorrect(string name)
    {
        // CHEQUEAR SI HAY ALGUN CARACTER QUE NO SE PERMITE
        // CHEQUEAR QUE NO SEA UN NOMBRE OFENSIVO DE USUARIO
        // CHEQUEAR QUE EL NOMBRE TENGA MAS DE UNA CANTIDAD DETERMINADA DE CARACTERES

        // CHEQUEAR SI HAY UN USUARIO CON EL MISMO NOMBRE
        bool us = await NameExistNew(name);

        if (us)
        {
            Debug.Log("User Name Not Valid");
            return false;
        }

        return true;
    }

    private async Task<bool> NameExistNew(string name)
    {
        bool exist = false;
        name = name.ToLower();

        await FirebaseDatabase.DefaultInstance.GetReference(usersTable).OrderByKey().GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("NoChild");
                // Handle the error...
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                exist = snapshot.Child(name).Exists;
            }
        });

        return exist;
    }

    private void PostUserWithFireBaseRT(string key, string json)
    {
        DatosFirebaseRTHelper.Instance.reference.Child(usersTable).Child(key).SetRawJsonValueAsync(json);
    }

    private IEnumerator RegisterUserWithAuthentication(string email, string username, string password)
    {
        var RegisterTask = DatosFirebaseRTHelper.Instance.auth.CreateUserWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

        if (RegisterTask.Exception != null)
        {
            Debug.LogWarning(message: $"failed to register task with {RegisterTask.Exception}");
            FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
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
                    Debug.Log("Error Code " + errorCode.ToString());
                    break;
            }

            Debug.Log(message);

        }
        else
        {
            user = RegisterTask.Result;
            if (user != null)
            {
                UserProfile profile = new UserProfile { DisplayName = user.DisplayName };

                var ProfileTask = user.UpdateUserProfileAsync(profile);
                yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                if (ProfileTask.Exception != null)
                {
                    Debug.LogWarning(message: $"failed to register task with {ProfileTask.Exception}");
                    FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                    AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                    Debug.Log("Username set failed");
                }
            }
        }
    }

}

