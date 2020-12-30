using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] Text txtAmountLoaded;
    [SerializeField] Image barImage;
    [SerializeField] Image roundLogo;
    public Canvas canvas;
    bool isLoading = false;

    private void Awake()
    {
        canvas.worldCamera = Camera.main;
    }

    public void SetLoadedAmount(float amount)
    {
        // 100 es 1
        // amount es X
        float percentLoaded = amount / 100;
        txtAmountLoaded.text = amount + "%";
        barImage.fillAmount = percentLoaded;
    }

    public void StartWaitForLoadScreen()
    {
        gameObject.SetActive(true);
        isLoading = true;
        StartLoadingLogo();
    }

    public void StopWaitForLoadScreen()
    {
        gameObject.SetActive(false);
        isLoading = false;
    }

    private void StartLoadingLogo()
    {
        roundLogo.gameObject.SetActive(true);
        StartCoroutine(Rotate());
    }

    private IEnumerator Rotate()
    {
        Vector3 rotationEuler = new Vector3();
        while (isLoading)
        {
            rotationEuler += Vector3.forward * 30 * Time.deltaTime;
            roundLogo.transform.Rotate(rotationEuler);
            yield return null;
        }
    }
}
