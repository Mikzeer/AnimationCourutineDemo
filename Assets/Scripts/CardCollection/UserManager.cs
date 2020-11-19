using UnityEngine;

public class UserManager : MonoBehaviour
{
    #region SINGLETON

    [SerializeField] protected bool dontDestroy;
    private static UserManager instance;
    public static UserManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UserManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    instance = obj.AddComponent<UserManager>();
                }
            }
            return instance;
        }
    }

    #endregion

    private UserDB user;
    private bool isUserLoaded = false;
    private bool isResourcesLoaded = false;
    private UserResources userResources;

    private void Start()
    {
        UserDB fuser = new UserDB("mmm", "", "", "");
        SetUser(fuser);
        //CardCollection.Instance.LoadCollections(user);
    }

    public UserDB GetUser()
    {
        if (isUserLoaded == true)
        {
            return user;
        }
        return null;
    }

    public void SetUser(UserDB pUser)
    {
        UserDB auxUs = new UserDB(pUser.Name);
        user = auxUs;
        isUserLoaded = true;
        //if (isUserLoaded == false)
        //{
        //    UserDB auxUs = new UserDB(pUser.Name);
        //    user = auxUs;
        //    isUserLoaded = true;
        //}
    }
   
    public void SetUserResources(UserResources userResources)
    {
        this.userResources = userResources;
        isResourcesLoaded = true;
    }

    public UserResources GetUserResources()
    {
        if (isResourcesLoaded == true)
        {
            return userResources;
        }
        return null;
    }
}
