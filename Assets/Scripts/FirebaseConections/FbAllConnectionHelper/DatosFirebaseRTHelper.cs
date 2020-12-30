using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using System.Threading.Tasks;

public class DatosFirebaseRTHelper : GenericSingleton<DatosFirebaseRTHelper>
{
    private const string projectId = "kimbokotooltest"; // You can find this in your Firebase project settings
    private static readonly string databaseURL = $"https://{projectId}.firebaseio.com/";
    public string dbURL { get { return databaseURL; }private set { value = databaseURL; } }

    public readonly string usersTable = "Users";
    public readonly string cardsTable = "Cards";
    public readonly string UsersCardCollectionTable = "UsersCardCollection";
    public readonly string DefaultCollectionTable = "DefaultCollection";
    public readonly string GameCardCollectionLastUpdateTable = "GameCollectionLastUpdate";
    public readonly string CardsLimitDataTable = "CardsLimitData";
    public readonly string CardsLimitDataLastUpdateTable = "CardsLimitDataLastUpdate";
    public readonly string GamePricesTable = "GamePrices";
    public readonly string GameRewardsTable = "GameRewards";
    public readonly string UsersResourcesTable = "UsersResources";
    public readonly string StartingResourcesTable = "StartingResources";

    public DatabaseReference reference;
    public FirebaseApp app;
    public bool isInit = false;

    public FirebaseAuth auth { get; private set; }
    private DependencyStatus dependencyStatus;
    private bool debugOn = true;

    private async void Start()
    {
        isInit = await Init();
        if (isInit == true)
        {
            if (debugOn == true) Debug.Log("THE DATA BASE WAS LOADED WITH SUCCSES");
        }
        else
        {
            if (debugOn == true) Debug.Log("THE DATA BASE FAILED LOADING");
        }
    }

    private async Task<bool> Init()
    {
        if (debugOn == true) Debug.Log("UNO");
        bool isFinish = false;
        // Set up the Editor before calling into the realtime database.
        //FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(databaseURL);
        if (debugOn == true) Debug.Log("DOS");
        System.Uri uri = new System.Uri(databaseURL);
        FirebaseApp.DefaultInstance.Options.DatabaseUrl = uri;
        if (debugOn == true) Debug.Log("TRES");
        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        if (debugOn == true) Debug.Log("CUATRO");
        await FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (debugOn == true) Debug.Log("CINCO");
            dependencyStatus = task.Result;
            if (debugOn == true) Debug.Log("SEIS");
            if (dependencyStatus == DependencyStatus.Available)
            {
                if (debugOn == true) Debug.Log("SIETE");
                auth = FirebaseAuth.DefaultInstance;
                app = FirebaseApp.DefaultInstance;

                FirebaseDatabase.DefaultInstance.SetPersistenceEnabled(false);
                reference.Database.SetPersistenceEnabled(false);                
                isFinish = true;
            }
            else
            {
                if (debugOn == true) Debug.LogError("Could not resolve all Firebase dependecies: " + dependencyStatus);
            }
        });
        if (debugOn == true) Debug.Log("OCHO");
        return isFinish;
    }
   
}