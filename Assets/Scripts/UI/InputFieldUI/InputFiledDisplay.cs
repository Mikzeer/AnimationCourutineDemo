using UnityEngine;
using UnityEngine.UI;
using System;

public class InputFiledDisplay : MonoBehaviour
{
    [SerializeField] private InputField inputField;
    [SerializeField] private Text txtPlaceholder;
    private string strPlaceHolder;
    private Action<string, INPUTFIELDTYPE> OnDataChanges;
    public INPUTFIELDTYPE inputType { get; set; }
    private void OnEnable()
    {
        inputField.onEndEdit.AddListener(OnInputTextChange);
    }

    private void OnDisable()
    {
        inputField.onEndEdit.RemoveAllListeners();
    }

    private void OnInputTextChange(string inputText)
    {
        if (inputText == string.Empty || inputText == "" || inputText == null)
        {
            OnDataChanges?.Invoke(string.Empty, inputType);
        }
        else
        {
            OnDataChanges?.Invoke(inputText, inputType);
        }
    }

    public void SetDisplay(string phDescription, Action<string, INPUTFIELDTYPE> OnDataChanges, INPUTFIELDTYPE inputType)
    {
        strPlaceHolder = phDescription;
        txtPlaceholder.text = phDescription;
        this.OnDataChanges = OnDataChanges;
        this.inputType = inputType;
        if (inputType == INPUTFIELDTYPE.PASS || inputType == INPUTFIELDTYPE.REPASS)
        {
            inputField.contentType = InputField.ContentType.Password;
        }        
    }

    public void ClearDisplay()
    {
        inputField.text = string.Empty;
        txtPlaceholder.text = strPlaceHolder;
    }

}