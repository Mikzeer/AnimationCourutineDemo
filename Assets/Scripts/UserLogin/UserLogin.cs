using UnityEngine;
using UnityEngine.UI;
using System.Security.Cryptography;
using Proyecto26;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;
using Firebase.Auth;
using System.Threading.Tasks;
using System.Collections;

public class UserLogin : MonoBehaviour
{
    [SerializeField] private Text txtUserNameRegister;
    [SerializeField] private Text txtPassRegister;
    [SerializeField] private Text txtRePassRegister;

    [SerializeField] private Text txtUserNameLogin;
    [SerializeField] private Text txtPassLogin;

    FirebaseAuth auth;
    FirebaseUser user;
    DependencyStatus dependencyStatus;

    private const string projectId = "kimbokotooltest"; // You can find this in your Firebase project settings
    private static readonly string databaseURL = $"https://{projectId}.firebaseio.com/";
    private DatabaseReference reference;

    private void Start()
    {
        //string plaintext = "Mikzeer";
        //string passwordHashMD5 = ComputeHash(plaintext, "MD5", null);
        //string passwordHashSha1 = ComputeHash(plaintext, "SHA1", null);
        //string passwordHashSha256 = ComputeHash(plaintext, "SHA256", null);
        //string passwordHashSha384 = ComputeHash(plaintext, "SHA384", null);
        //string passwordHashSha512 = ComputeHash(plaintext, "SHA512", null);

        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(databaseURL);

        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;



        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependecies: " + dependencyStatus);
            }
        });
    }

    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        auth = FirebaseAuth.DefaultInstance;
    }

    public async void TestCreateUserButton()
    {
        await CreateNewUser();

        //await DatosFirebaseRTHelper.Instance.CreateNewUser();
    }

    public async void TestButton()
    {
        await GetValueTest();
    }

    public void TestButtonNormal()
    {
        //DeleteUserWithFireBaseRT("Ru");
    }

    public async void TestBtnLogin()
    {
        UserDB logedUser = await NewLoginUserData();

        if (logedUser == null)
        {
            Debug.Log("No User");
        }
        else
        {
            Debug.Log("Login SUccesfull");
        }

        StartCoroutine(LoginUserWithAuthentication("test@test.com", "123456"));
    }

    public async Task CreateNewUser()
    {
        bool isDataCorrect = await CheckIfDataIsCorrect();
        if (isDataCorrect)
        {
            string userName = txtUserNameRegister.text;
            string userPass = txtPassRegister.text;
            string macAddres = HelperUserData.GetMacAddress();
            string localIP = HelperUserData.GetLocalIPAddress();

            HashWithSaltResult hashSalt = HelperUserData.HashWithSalt(userPass, 32, new SHA256Managed());
            string pSalt = hashSalt.Salt;
            string hasPass = hashSalt.Digest;

            UserDB userDB = new UserDB(userName, macAddres, pSalt, hasPass);

            //Every time you call Push(), Firebase generates a unique key that can also be used as a unique identifier,
            // such as user-scores/<user-id>/<unique-score-id>.
            string key = reference.Child("Users").Push().Key;
            userDB.ID = key;
            string json = JsonUtility.ToJson(userDB);
            
            //PostUserWithRestApi(userDB);
            //PostUserWithFireBaseRT(key, json);
            PostUserWithFireBaseRT(userName.ToLower(), json);

            StartCoroutine(RegisterUserWithAuthentication("test@test.com", userName, "123456"));
        }
    }

    public async Task<UserDB> NewLoginUserData()
    {
        // CHEQUEAR SI HAY UN USUARIO CON EL MISMO NOMBRE
        DataSnapshot userNameExist = await PlayerDataSnapshotNameExist(txtUserNameLogin.text);
        UserDB logedUser = null;
        if (userNameExist != null)
        {
            if (userNameExist.Exists)
            {
                UserDB user = JsonUtility.FromJson<UserDB>(userNameExist.GetRawJsonValue());

                if (HelperUserData.isCorrectPassword(user.Salt, user.Password, txtPassLogin.text, new SHA256Managed()))
                {
                    Debug.Log("PASSWORD CORRECT");
                    logedUser = user;


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

    public async Task GetValueTest()
    {
        await FirebaseDatabase.DefaultInstance.GetReference("Users").GetValueAsync().ContinueWith(task => 
        {
          if (task.IsFaulted)
          {
                Debug.Log("NoChild");
                  // Handle the error...
          }
          else if (task.IsCompleted)
          {
                DataSnapshot snapshot = task.Result;
                // Do something with snapshot...

                foreach (var child in snapshot.Children)
                {
                    Debug.Log("The Key" + child.Key);
                    if (child.HasChild("Name"))
                    {
                        Debug.Log("NAME  FAUND");
                        if (child.Child("Name").Value.ToString() == "Lakitu")
                        {
                            Debug.Log("LAKITU FAUND");
                            UserDB user = JsonUtility.FromJson<UserDB>(child.GetRawJsonValue());
                            string json = JsonUtility.ToJson(user);

                            Debug.Log(json);
                        }
                    }                    
                }
            }
        });

    }

    private async Task<bool> NameExist(string name)
    {
        bool exist = false;

        await FirebaseDatabase.DefaultInstance.GetReference("Users").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("NoChild");
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
                            Debug.Log("NAME FAUND");
                            exist = true;
                        }
                    }
                }
            }
        });

        return exist;
    }

    private async Task<bool> NameExistNew(string name)
    {
        bool exist = false;
        name = name.ToLower();

        await FirebaseDatabase.DefaultInstance.GetReference("Users").OrderByKey().GetValueAsync().ContinueWith(task =>
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

    private async Task<DataSnapshot> PlayerDataSnapshotNameExist(string name)
    {
        DataSnapshot dtSnapshot = null;
        await FirebaseDatabase.DefaultInstance.GetReference("Users").GetValueAsync().ContinueWith(task =>
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

    private async Task<bool> CheckIfDataIsCorrect()
    {
        if (txtUserNameRegister.text == "" || txtUserNameRegister.text == string.Empty)
        {
            Debug.Log("No user Name");
            return false;
        }

        // CHEQUEAR SI HAY ALGUN CARACTER QUE NO SE PERMITE
        // CHEQUEAR QUE NO SEA UN NOMBRE OFENSIVO DE USUARIO
        // CHEQUEAR QUE EL NOMBRE TENGA MAS DE UNA CANTIDAD DETERMINADA DE CARACTERES


        // CHEQUEAR SI HAY UN USUARIO CON EL MISMO NOMBRE
        //bool userNameExist = await NameExist(txtUserNameRegister.text);
        bool us = await NameExistNew(txtUserNameRegister.text);

        if (us)
        {
            Debug.Log("User Name Not Valid");
            return false;
        }

        if (txtPassRegister.text == "" || txtPassRegister.text == string.Empty)
        {
            Debug.Log("No Password");
            return false;
        }

        if (txtRePassRegister.text == "" || txtRePassRegister.text == string.Empty)
        {
            Debug.Log("No RePassword");
            return false;
        }

        if (txtRePassRegister.text != txtPassRegister.text)
        {
            Debug.Log("Your Password and Repassword must be the same");
            return false;
        }

        return true;
    }

    private void PostUserWithRestApi(UserDB user)
    {
        string key = reference.Child("Users").Push().Key;
        RestClient.Put<UserDB>($"{databaseURL}Users/{key}.json", user);
    }

    private void PostUserWithFireBaseRT(string key, string json)
    {
        reference.Child("Users").Child(key).SetRawJsonValueAsync(json);
    }

    private void DeleteUserWithFireBaseRT(string key)
    {
        reference.Child("Users").Child(key).SetValueAsync(null);
    }






    private void SignUpUserWithAuthentication(string email, string username, string password)
    {
        string userData = $"https://www.googleapis.com/v1/accounts:signUp?key=" + "[API_KEY]";
    }

    private IEnumerator RegisterUserWithAuthentication(string email, string username, string password)
    {
        var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);
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

    private IEnumerator LoginUserWithAuthentication(string email, string password)
    {
        var LoginTask = auth.SignInWithEmailAndPasswordAsync(email, password);

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

}
