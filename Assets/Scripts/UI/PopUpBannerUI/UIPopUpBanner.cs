using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIPopUpBanner : MonoBehaviour
{
    [SerializeField] private Button btnCancel;
    [SerializeField] private Button btnAccept;
    [SerializeField] private Text txtTitle;
    [SerializeField] private Text txtDescription;

    [SerializeField] private GameObject canvasUI;

    float destroyTime = 1.5f;
    bool autoClose;

    private void OnEnable()
    {
        btnCancel.onClick.AddListener(Hide);
        btnAccept.onClick.AddListener(Hide);
        if (autoClose)
        {
            StartCoroutine(DestroyAfterSomeTime());
        }
    }

    private void OnDisable()
    {
        btnCancel.onClick.RemoveAllListeners();
        btnAccept.onClick.RemoveAllListeners();
    }

    public void SetPopUpData(PopUpBannerData popUpBannerData, PopUpBannerAction popUpBannerAction, bool autoClose = false)
    {
        txtTitle.text = popUpBannerData.title;
        txtDescription.text = popUpBannerData.description;

        if (autoClose)
        {
            btnCancel.gameObject.SetActive(false);
            btnAccept.gameObject.SetActive(false);            
            this.autoClose = autoClose;
            return;
        }

        if (popUpBannerAction.OnCancelButtonPress == null)
        {
            btnCancel.gameObject.SetActive(false);
        }
        else
        {
            btnCancel.onClick.AddListener(popUpBannerAction.OnCancelButtonPress);
        }

        if (popUpBannerAction.OnAcceptButtonPress != null)
        {
            btnAccept.onClick.AddListener(popUpBannerAction.OnAcceptButtonPress);
        }       
    }

    public void Show()
    {
        canvasUI.SetActive(true);
    }

    private void Hide()
    {
        canvasUI.SetActive(false);
        Destroy(gameObject);
    }

    private IEnumerator DestroyAfterSomeTime()
    {
        float actualTime = 0;

        while (actualTime <= destroyTime)
        {
            actualTime += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}
