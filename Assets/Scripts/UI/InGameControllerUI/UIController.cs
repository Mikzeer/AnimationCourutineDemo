using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    #region VARIABLES

    [SerializeField] private TimePanelUI timePanelUI;

    [SerializeField] private Button btnTakeCard;
    public delegate void ClickTakeCardAction();
    public static event ClickTakeCardAction OnTakeCardActionClicked;

    [SerializeField] private Button btnEndTurn;
    public delegate void ClickEndTurnAction();
    public static event ClickEndTurnAction OnEndTurnActionClicked;

    #endregion

    #region UNITY METHODS

    private void Awake()
    {
        btnTakeCard.onClick.AddListener(OnBtnTakeCardClick);
        btnEndTurn.onClick.AddListener(OnBtnEndTurnClick);
        btnEndTurn.gameObject.SetActive(false);
        btnTakeCard.gameObject.SetActive(false);
        GameCreator.OnTimeStart += SetTimer;
        GameCreator.OnTakeCardAvailable += SetCardButtonVisibility;
        GameCreator.OnEndTurnAvailable += SetEndTurnVisibility;
    }


    private void OnDestroy()
    {
        btnTakeCard.onClick.RemoveListener(OnBtnTakeCardClick);
        btnEndTurn.onClick.RemoveListener(OnBtnEndTurnClick);
    }

    #endregion

    public void SetTimer()
    {
        GameTimer.OnTimeChange += UpdateTime;
    }

    private void UpdateTime(string obj)
    {
        timePanelUI.SetTime(obj);
    }

    private void SetCardButtonVisibility(bool obj)
    {
        btnTakeCard.gameObject.SetActive(obj);
    }

    private void SetEndTurnVisibility(bool obj)
    {
        btnEndTurn.gameObject.SetActive(obj);
    }

    #region BUTTON EVENT REGION

    void OnBtnTakeCardClick()
    {
        OnTakeCardActionClicked?.Invoke();
    }

    void OnBtnEndTurnClick()
    {
        OnEndTurnActionClicked?.Invoke();
    }

    #endregion

}