using UnityEngine;
using UnityEngine.UI;

public class UserLoginManager : MonoBehaviour
{
    [SerializeField] private Text txtEmail;
    [SerializeField] private Text txtUserNameLogin;
    [SerializeField] private Text txtPassLogin;
    [SerializeField] private Button btnLogin;
    [SerializeField] private UserLoginFirebase userLoginFirebase;
    private void OnEnable()
    {
        btnLogin.onClick.AddListener(LoginUserButton);
    }

    private void OnDisable()
    {
        btnLogin.onClick.RemoveAllListeners();
    }

    public void LoginUserButton()
    {
        LoginUser();
    }

    private async void LoginUser()
    {
        if (IsLoginDataComplete() == true)
        {
            UserDB logedUser = await userLoginFirebase.UserLoginMultipleInterface(txtUserNameLogin.text, txtPassLogin.text, txtEmail.text);

            UserManager.Instance.SetUser(logedUser);
        }
    }

    private bool IsLoginDataComplete()
    {
        if (txtEmail.text == "" || txtEmail.text == string.Empty)
        {
            Debug.Log("No Email");
            return false;
        }

        if (txtUserNameLogin.text == "" || txtUserNameLogin.text == string.Empty)
        {
            Debug.Log("No user Name");
            return false;
        }

        if (txtPassLogin.text == "" || txtPassLogin.text == string.Empty)
        {
            Debug.Log("No Password");
            return false;
        }

        return true;
    }

}
