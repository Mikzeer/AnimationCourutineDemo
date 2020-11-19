using UnityEngine;
using Proyecto26;

public class UserLoginRestAPIFirebase : MonoBehaviour
{
    private const string usersTable = "Users";

    private void PostUserWithRestApi(UserDB user)
    {
        string key = DatosFirebaseRTHelper.Instance.reference.Child(usersTable).Push().Key;
        RestClient.Put<UserDB>($"{DatosFirebaseRTHelper.Instance.dbURL}Users/{key}.json", user);
    }
}

