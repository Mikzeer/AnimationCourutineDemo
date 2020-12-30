using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class UserSignInUpManager : MonoBehaviour
{
    [SerializeField] private GameObject inputTextPrefab;
    [SerializeField] private RectTransform inputTextParent;
    [SerializeField] private RectTransform autoLoginPanel;
    [SerializeField] private Text txtErrorMessage;
    [SerializeField] private Canvas canvas; // Este canvas sirva para llevar al frente de todo siempre lo que drageamos
    [SerializeField] private Button btnSignUp;//Ir a pantalla de Registro
    [SerializeField] private Button btnSignIn;//Ir a pantalla de Login
    [SerializeField] private Button btnBack; //Ir a pantalla principal de SignIn/Up
    [SerializeField] private Button btnConfirm;// Logear/Registrar al usuario segun modo
    [SerializeField] private Text txtConfirmButton;
    [SerializeField] private Toggle autoLoginToggle;
    private string email;
    private string userName;
    private string pass;
    private string repass;
    private bool autoLogin;
    private List<InputFiledDisplay> inputFileds;
    private bool isLogin;
    private UserLoginManager userLoginManager;
    private UserRegistrationManager userRegistrationManager;
    private AutoLoginManager autoLoginManager;

    private void Awake()
    {
        CreateInputUI();
        userLoginManager = new UserLoginManager(ShowErrorMessage);
        userRegistrationManager = new UserRegistrationManager(ShowErrorMessage);
        autoLoginManager = new AutoLoginManager(ShowErrorMessage);

    }

    public void Start()
    {
        GameSceneManager.Instance.SetActiveWaitForLoad(true);
        StartCoroutine(WaitForDatabaseToLoad());
        //autoLoginManager.AutoLoginUser();
    }

    public IEnumerator WaitForDatabaseToLoad()
    {
        while (DatosFirebaseRTHelper.Instance.isInit == false)
        {
            Debug.Log("WAITING");
            yield return null;
        }
        //GameSceneManager.Instance.SetActiveWaitForLoad(false);
        autoLoginManager.AutoLoginUser();
    }

    private void OnEnable()
    {
        btnBack.onClick.AddListener(Back);
        btnSignUp.onClick.AddListener(OpenRegisterMenu);
        btnSignIn.onClick.AddListener(OpenLoginMenu);
        autoLoginToggle.onValueChanged.AddListener(OnIsAutoLoginToggleChange);
    }

    private void OnDisable()
    {
        btnBack.onClick.RemoveAllListeners();
        btnSignUp.onClick.RemoveAllListeners();
        btnSignIn.onClick.RemoveAllListeners();
        autoLoginToggle.onValueChanged.RemoveAllListeners();
    }

    public void OnIsAutoLoginToggleChange(bool isOn)
    {
        autoLogin = isOn;
    }

    private void OpenLoginMenu()
    {
        btnConfirm.onClick.AddListener(LoginUserButton);
        txtConfirmButton.text = "SIGN IN";
        for (int i = 0; i < inputFileds.Count; i++)
        {
            if (inputFileds[i].inputType == INPUTFIELDTYPE.REPASS)
            {
                inputFileds[i].gameObject.SetActive(false);
            }
        }
        canvas.gameObject.SetActive(true);
        autoLoginPanel.gameObject.SetActive(true);
        isLogin = true;
    }

    private void OpenRegisterMenu()
    {
        btnConfirm.onClick.AddListener(RegisterUserButton);
        txtConfirmButton.text = "SIGN UP";
        for (int i = 0; i < inputFileds.Count; i++)
        {
            if (inputFileds[i].inputType == INPUTFIELDTYPE.REPASS)
            {
                inputFileds[i].gameObject.SetActive(true);
            }
        }
        canvas.gameObject.SetActive(true);
        autoLoginPanel.gameObject.SetActive(false);
        isLogin = false;
    }

    private void Back()
    {
        ClearUI();
        canvas.gameObject.SetActive(false);
        btnConfirm.onClick.RemoveAllListeners();
    }

    private void RegisterUserButton()
    {
        if (IsDataComplete())
        {
            UserRegistrationData userRegistrationData = new UserRegistrationData(email, userName, pass);
            userRegistrationManager.OnTryCreateUser(userRegistrationData);
        }
    }

    private void LoginUserButton()
    {
        if (IsDataComplete())
        {
            UserRegistrationData userRegistrationData = new UserRegistrationData(email, userName, pass);
            userRegistrationData.autoLogin = autoLogin;
            userLoginManager.OnTryLoginUser(userRegistrationData);
        }
    }

    private void CreateInputUI()
    {
        inputFileds = new List<InputFiledDisplay>();
        InputFiledDisplay ifdEmail = HelperUI.CreateInputTextDisplay(inputTextPrefab, inputTextParent);
        ifdEmail.SetDisplay("Enter Email...", OnInputTextChange, INPUTFIELDTYPE.EMAIL);
        InputFiledDisplay ifdUser = HelperUI.CreateInputTextDisplay(inputTextPrefab, inputTextParent);
        ifdUser.SetDisplay("Enter User Name...", OnInputTextChange, INPUTFIELDTYPE.USER);
        InputFiledDisplay ifdPass = HelperUI.CreateInputTextDisplay(inputTextPrefab, inputTextParent);
        ifdPass.SetDisplay("Enter Password...", OnInputTextChange, INPUTFIELDTYPE.PASS);
        InputFiledDisplay ifdRepass = HelperUI.CreateInputTextDisplay(inputTextPrefab, inputTextParent);
        ifdRepass.SetDisplay("Reenter Password...", OnInputTextChange, INPUTFIELDTYPE.REPASS);
        inputFileds.Add(ifdEmail);
        inputFileds.Add(ifdUser);
        inputFileds.Add(ifdPass);
        inputFileds.Add(ifdRepass);
    }

    private void OnInputTextChange(string inputText, INPUTFIELDTYPE inputType)
    {
        switch (inputType)
        {
            case INPUTFIELDTYPE.EMAIL:
                email = inputText;
                break;
            case INPUTFIELDTYPE.USER:
                userName = inputText;
                break;
            case INPUTFIELDTYPE.PASS:
                pass = inputText;
                break;
            case INPUTFIELDTYPE.REPASS:
                repass = inputText;
                break;
            default:
                break;
        }
    }

    private void ClearUI()
    {
        email = string.Empty;
        userName = string.Empty;
        pass = string.Empty;
        repass = string.Empty;
        for (int i = 0; i < inputFileds.Count; i++)
        {
            inputFileds[i].ClearDisplay();
        }
        ShowErrorMessage("");
    }

    public void ShowErrorMessage(string errorMessage)
    {
        txtErrorMessage.text = errorMessage;
    }

    private bool IsDataComplete()
    {
        if (Helper.IsEmptyString(email))
        {
            ShowErrorMessage("No Email");
            return false;
        }

        if (Helper.IsEmptyString(userName))
        {
            ShowErrorMessage("No user Name");
            return false;
        }

        if (Helper.IsEmptyString(pass))
        {
            ShowErrorMessage("No Password");
            return false;
        }

        if (isLogin)
        {
            return true;
        }

        if (Helper.IsEmptyString(repass))
        {
            ShowErrorMessage("No RePassword");
            return false;
        }

        if (repass != pass)
        {
            ShowErrorMessage("Your Pass and Repass are not the same");
            return false;
        }

        return true;
    }
}