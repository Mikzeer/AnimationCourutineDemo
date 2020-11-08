using UnityEngine;
using System.Security.Cryptography;
using Proyecto26;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;
using Firebase.Auth;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class DatosFirebaseRTHelper : MonoBehaviour
{
    #region SINGLETON

    [SerializeField] protected bool dontDestroy;
    private static DatosFirebaseRTHelper instance;
    public static DatosFirebaseRTHelper Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DatosFirebaseRTHelper>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    instance = obj.AddComponent<DatosFirebaseRTHelper>();
                }
            }
            return instance;
        }
    }

    #endregion

    private const string projectId = "kimbokotooltest"; // You can find this in your Firebase project settings
    private static readonly string databaseURL = $"https://{projectId}.firebaseio.com/";
    private DatabaseReference reference;

    private FirebaseAuth auth;
    private FirebaseUser user;
    private DependencyStatus dependencyStatus;

    private const string usersTable = "Users";
    private const string cardsTable = "Cards";
    private const string DefaultCollectionTable = "DefaultCollection";
    private const string UsersCardCollectionTable = "UsersCardCollection";
    private bool isInit = false;

    public Image pruebaSprite;
    public Image spriteToCreate;

    public void PruebaButton()
    {
        // conversion to bytes
        byte[] spByte = EncodeSpriteToByteArray(pruebaSprite.sprite);

        Texture2D texture = duplicateTexture(pruebaSprite.sprite.texture);

        spriteToCreate.sprite = GetSpriteFromByteArray(spByte);
    }


    Texture2D duplicateTexture(Texture2D source)
    {
        // Use RenderTexture. Put the source Texture2D into RenderTexture with 
        // Graphics.Blit then use Texture2D.ReadPixels to read the image from RenderTexture into the new Texture2D.
        RenderTexture renderTex = RenderTexture.GetTemporary(
                    source.width,
                    source.height,
                    0,
                    RenderTextureFormat.Default, 
                    RenderTextureReadWrite.Linear); // Default RGBA32

        Graphics.Blit(source, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        Texture2D readableText = new Texture2D(source.width, source.height);
        readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        readableText.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);
        return readableText;
    }

    Texture2D duplicateTextureBadWay(Texture2D source)
    {
        //2.Use Texture2D.GetRawTextureData() + Texture2D.LoadRawTextureData():
        //You can't use GetPixels32() because the Texture2D is not readable. You were so close about using GetRawTextureData().
        //You failed when you used Texture2D.LoadImage() to load from GetRawTextureData().
        //Texture2D.LoadImage() is only used to load PNG / JPG array bytes not Texture2D array byte.
        //If you read with Texture2D.GetRawTextureData(), you must write with Texture2D.LoadRawTextureData() not Texture2D.LoadImage().
        //There will be no error with the code above in the Editor but there should be an error in standalone build. 
        //Besides, it should still work even with the error in the standalone build. I think that error is more like a warning.
        byte[] pix = source.GetRawTextureData();
        Texture2D readableText = new Texture2D(source.width, source.height, source.format, false);
        readableText.LoadRawTextureData(pix);
        readableText.Apply();
        return readableText;
    }


    public Sprite GetSpriteFromByteArray(Byte[] bytes)
    {
        // create a Texture2D object that is used to stream data into Texture2D
        // No impota la medida que le asignemos a la textura ya que cuando ejecutamos el Load Image toma la medida real
        Texture2D texture = new Texture2D(10, 10);
        texture.LoadImage(bytes); // stream data into Texture2D
                                  // Create a Sprite, to Texture2D object basis
        Sprite sp = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        return sp;
    }

    public byte[] EncodeSpriteToByteArray(Sprite sp)
    {
        // EL SPRITE TIENE QUE TENER HABILITADO LO SIGUIENTE
        // /Advance/Read/Write Enable = true
        // /Default/Compression = None

        // convert Texture
        Texture2D temp = sp.texture;
        // conversion to bytes
        byte[] photoByte = temp.EncodeToPNG();

        return photoByte;
    }

    public void WriteTextureToPlayerPrefs(string tag, Texture2D tex)
    {
        // if texture is png otherwise you can use tex.EncodeToJPG().
        byte[] texByte = tex.EncodeToPNG();

        // convert byte array to base64 string
        string base64Tex = Convert.ToBase64String(texByte);

        // write string to playerpref
        PlayerPrefs.SetString(tag, base64Tex);
        PlayerPrefs.Save();
    }

    public Texture2D ReadTextureFromPlayerPrefs(string tag)
    {
        // load string from playerpref
        string base64Tex = PlayerPrefs.GetString(tag, null);

        if (!string.IsNullOrEmpty(base64Tex))
        {
            // convert it to byte array
            byte[] texByte = System.Convert.FromBase64String(base64Tex);
            Texture2D tex = new Texture2D(2, 2);

            //load texture from byte array
            if (tex.LoadImage(texByte))
            {
                return tex;
            }
        }

        return null;
    }

    public PositionerDemo.ACTIVATIONTYPE GetActivationTypeFromInt(int pIDActType)
    {
        //var myEnumMemberCount = Enum.GetNames(typeof(MyEnum)).Length;
        //YourEnum foo = (YourEnum)yourInt;
        int acTypeTotalCount = Enum.GetNames(typeof(PositionerDemo.ACTIVATIONTYPE)).Length;

        if (pIDActType >= acTypeTotalCount)
        {
            return PositionerDemo.ACTIVATIONTYPE.NONE;
        }

        PositionerDemo.ACTIVATIONTYPE acType = (PositionerDemo.ACTIVATIONTYPE)pIDActType;
        return acType;
    }

    private static DateTime UnixTimeStampToDateTimeSeconds(long unixTimeStamp)
    {
        DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        dtDateTime = dtDateTime.AddSeconds(unixTimeStamp);
        return dtDateTime;
    }

    private static DateTime UnixTimeStampToDateTimeMiliseconds(long unixTimeStamp)
    {
        DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp);
        return dtDateTime;
    }

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(databaseURL);

        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebaseAuthentication();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependecies: " + dependencyStatus);
            }

            if (long.TryParse(ServerValue.Timestamp.ToString(), out utcCreatedTimestamp))
            {
                Debug.Log("utcCreatedTimestamp " + utcCreatedTimestamp);
            }
            else
            {
                Debug.Log("NO utcCreatedTimestamp ");
                Debug.Log("ServerValue.Timestamp " + ServerValue.Timestamp);
                Debug.Log("ServerValue.Timestamp.ToString() " + ServerValue.Timestamp.ToString());
                //string timestampAdd = @"timestamp"": {"".sv"" : ""timestamp""} } ";

                reference.Child("Users").Child("new1").UpdateChildrenAsync(new Dictionary<string, object> { { "utcLastDownloadCollectionUnix", ServerValue.Timestamp } , { "utcLastDownloadOwnedCards", ServerValue.Timestamp } });
                //reference.Child("Users").Child("pepe").SetRawJsonValueAsync(timestampAdd);
                //DateTime dtTest = ;
            }

        });
    }

    private async Task<bool> SaveExist(string referencePath)
    {
        //var dataSnapshot = await reference.Database.GetReference("Users").GetValueAsync();
        var dataSnapshot = await reference.Database.GetReference(referencePath).GetValueAsync();

        return dataSnapshot.Exists;
    }

    private void InitializeFirebaseAuthentication()
    {
        Debug.Log("Setting up Firebase Auth");
        auth = FirebaseAuth.DefaultInstance;
        isInit = true;
    }

    //readonly object Timestamp = ServerValue.Timestamp.CreateServerValuePlaceholder("timestamp")
    public long utcCreatedTimestamp;


    public async Task CreateNewUser(UserDB userDB, string password, string email)
    {
        if (isInit == false) return;

        bool isDataCorrect = await CheckIfDataIsCorrect(userDB.Name);
        if (isDataCorrect)
        {
            //Every time you call Push(), Firebase generates a unique key that can also be used as a unique identifier,
            // such as user-scores/<user-id>/<unique-score-id>.
            string key = reference.Child(usersTable).Push().Key;
            userDB.ID = key;
            userDB.IsFirstTime = true;
            string json = JsonUtility.ToJson(userDB);

            //PostUserWithRestApi(userDB);
            //PostUserWithFireBaseRT(key, json);

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
        reference.Child(usersTable).Child(key).SetRawJsonValueAsync(json);
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

    public async Task<UserDB> UserLoginMultipleInterface(string ussername, string password = "123456", string email = "test@test.com")
    {
        if (isInit == false) return null;

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
                        List<DefaultCollectionDataDB> allCardList = await GetAndSetDefaultCardCollection();
                        
                        if (allCardList != null && allCardList.Count > 0)
                        {
                            DefCollectionListDB df = new DefCollectionListDB(allCardList);
                            await reference.Child(usersTable).Child(user.Name.ToLower()).Child("IsFirstTime").SetValueAsync(false);

                            foreach (DefaultCollectionDataDB dcData in allCardList)
                            {
                                string json = JsonUtility.ToJson(dcData);
                                await reference.Child(UsersCardCollectionTable).Child(user.Name.ToLower()).Child(dcData.ID).SetRawJsonValueAsync(json);
                            }

                        }
                    }

                    //DateTime createdDate = UnixTimeStampToDateTimeSeconds(user.utcCreatedTimestamp);

                    //Debug.Log("CREATED DATE " + createdDate);

                    Debug.Log("user.utcCreatedUnix " + user.utcLastDownloadCollectionUnix);
                    Debug.Log("user.utcCreated " + user.utcDownloadCollection);

                    long milliseconds;
                    if (long.TryParse(user.utcLastDownloadCollectionUnix.ToString(), out milliseconds))
                    {
                        utcCreatedTimestamp = milliseconds;

                        DateTime createdDate = UnixTimeStampToDateTimeMiliseconds(utcCreatedTimestamp);

                        Debug.Log("CREATED DATE " + createdDate);

                        //DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                    }

                    CardCollection.Instance.LastCollectionUpdate();

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

    public async Task<List<CardDataRT>> GetAllCardCollectionLibrary()
    {
        if (isInit == false) return null;

        List<CardDataRT> allCardList = new List<CardDataRT>();

        await FirebaseDatabase.DefaultInstance.GetReference(cardsTable).GetValueAsync().ContinueWith(task =>
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
                    CardDataRT card = JsonUtility.FromJson<CardDataRT>(child.GetRawJsonValue());
                    allCardList.Add(card);
                }
            }
        });

        return allCardList;
    }

    public async Task<List<DefaultCollectionDataDB>> GetAndSetDefaultCardCollection()
    {
        if (isInit == false) return null;

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

    private void PostUserWithRestApi(UserDB user)
    {
        string key = reference.Child(usersTable).Push().Key;
        RestClient.Put<UserDB>($"{databaseURL}Users/{key}.json", user);
    }

    private void DeleteUserWithFireBaseRT(string key)
    {
        reference.Child(usersTable).Child(key).SetValueAsync(null);
    }

}

[Serializable]
public class DefaultCollectionDataDB
{
    public string ID;
    public int Amount;

    public DefaultCollectionDataDB(string ID, int Amount)
    {
        this.ID = ID;
        this.Amount = Amount;
    }

    public Dictionary<string, System.Object> ToDictionary()
    {
        Dictionary<string, System.Object> result = new Dictionary<string, System.Object>();
        result["ID"] = ID;
        result["Amount"] = Amount;

        return result;
    }
}

[Serializable]
public class DefCollectionListDB
{
    public List<DefaultCollectionDataDB> CardCollection;

    public DefCollectionListDB(List<DefaultCollectionDataDB> CardCollection)
    {
        this.CardCollection = CardCollection;
    }

    public Dictionary<string, System.Object> ToDictionary()
    {
        Dictionary<string, System.Object> result = new Dictionary<string, System.Object>();
        result["defData"] = CardCollection;

        return result;
    }
}