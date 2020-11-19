using UnityEngine;
using System.Security.Cryptography;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;
using Firebase.Auth;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

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
    public static readonly string databaseURL = $"https://{projectId}.firebaseio.com/";
    public string dbURL { get { return dbURL; }private set { value = databaseURL; } }

    public DatabaseReference reference;
    public bool isInit = false;

    public FirebaseAuth auth { get; private set; }
    private DependencyStatus dependencyStatus;

    private async void Start()
    {
        isInit = await Init();
        if (isInit == true)
        {
            Debug.Log("THE DATA BASE WAS LOADED WITH SUCCSES");
            CardCollection.Instance.LoadCollections(UserManager.Instance.GetUser());
            CardCollection.Instance.LoadCardLimitData(UserManager.Instance.GetUser());
            ShopManager.Instance.LoadPriceDataFromFirebase(UserManager.Instance.GetUser());
            ShopManager.Instance.LoadUserResourcesFromFirebase(UserManager.Instance.GetUser());
        }
        else
        {
            Debug.Log("THE DATA BASE FAILED LOADING");
        }
    }

    private async Task<bool> Init()
    {
        bool isFinish = false;

        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(databaseURL);

        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        await FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebaseAuthentication();
                isFinish = true;
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependecies: " + dependencyStatus);
            }
        });

        return isFinish;
    }

    private void InitializeFirebaseAuthentication()
    {
        Debug.Log("Setting up Firebase Auth");
        auth = FirebaseAuth.DefaultInstance;
        //isInit = true;
    }

    private async Task<bool> SaveExist(string referencePath)
    {
        //var dataSnapshot = await reference.Database.GetReference("Users").GetValueAsync();
        var dataSnapshot = await reference.Database.GetReference(referencePath).GetValueAsync();

        return dataSnapshot.Exists;
    }
   
}
