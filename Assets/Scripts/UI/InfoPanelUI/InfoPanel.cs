using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class InfoPanel : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI infoTextMesh = default;
    [SerializeField]private RectTransform infoRect = default;

    RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();     
    }

    public void SetText(string infoText, Vector2 position)
    {
        SetActive(true);
        if (rectTransform == null)
        {
            return;
        }

        infoTextMesh.SetText(infoText);
        infoTextMesh.ForceMeshUpdate();

        Vector2 textSize = infoTextMesh.GetRenderedValues(false);
        Vector2 paddingSize = new Vector2(8, 8);
        infoRect.sizeDelta = textSize + paddingSize;
        rectTransform.anchoredPosition = position;
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}