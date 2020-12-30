using UnityEngine.UI;
using UnityEngine;

public class KimbokoInfoPanel : MonoBehaviour
{
    [SerializeField] private Text nameText;
    RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();    
    }

    public void SetText(bool isActive, string infoText = null)
    {
        if (isActive == false)
        {
            gameObject.SetActive(isActive);
            return;
        }

        gameObject.SetActive(isActive);
        nameText.text = infoText;
    }

}
