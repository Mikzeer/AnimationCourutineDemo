using UnityEngine;

public class UserDeletFirebase : MonoBehaviour
{
    private const string usersTable = "Users";

    public void DeleteUserWithFireBaseRT(string key)
    {
        DatosFirebaseRTHelper.Instance.reference.Child(usersTable).Child(key).SetValueAsync(null);
    }
}

