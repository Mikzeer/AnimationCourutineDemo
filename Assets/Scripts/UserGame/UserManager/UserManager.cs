using UnityEngine;

public class UserManager
{
    private UserDB user;
    private bool isUserLoaded = false;

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
    }
   
}