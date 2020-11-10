using UnityEngine;
using UnityEngine.UI;
using System.Security.Cryptography;

public class UserRegistrationManager : MonoBehaviour
{
    [SerializeField] private Text txtEmailRegister;
    [SerializeField] private Text txtUserNameRegister;
    [SerializeField] private Text txtPassRegister;
    [SerializeField] private Text txtRePassRegister;

    [SerializeField] private Button btnRegister;
    [SerializeField] private UserRegistrationFirebase userRegistrationFirebase;

    private void OnEnable()
    {
        btnRegister.onClick.AddListener(RegisterUserButton);
    }

    private void OnDisable()
    {
        btnRegister.onClick.RemoveAllListeners();
    }

    public void RegisterUserButton()
    {
        CreateNewUser();
    }

    public async void CreateNewUser()
    {
        if (IsRegistrationDataComplete())
        {
            string userName = txtUserNameRegister.text;
            string userPass = txtPassRegister.text;
            string macAddres = HelperUserData.GetMacAddress();
            string localIP = HelperUserData.GetLocalIPAddress();

            HashWithSaltResult hashSalt = HelperUserData.HashWithSalt(userPass, 32, new SHA256Managed());
            string pSalt = hashSalt.Salt;
            string hasPass = hashSalt.Digest;

            UserDB userDB = new UserDB(userName, macAddres, pSalt, hasPass);

            string email = txtEmailRegister.text;

            await userRegistrationFirebase.CreateNewUser(userDB, userPass, email);
        }
    }

    private bool IsRegistrationDataComplete()
    {
        if (txtEmailRegister.text == "" || txtEmailRegister.text == string.Empty)
        {
            Debug.Log("No Email");
            return false;
        }

        if (txtUserNameRegister.text == "" || txtUserNameRegister.text == string.Empty)
        {
            Debug.Log("No user Name");
            return false;
        }

        // CHEQUEAR QUE EL NOMBRE TENGA MAS DE UNA CANTIDAD DETERMINADA DE CARACTERES
        // CHEQUEAR SI HAY ALGUN CARACTER QUE NO SE PERMITE
        // CHEQUEAR QUE NO SEA UN NOMBRE OFENSIVO DE USUARIO

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

}
