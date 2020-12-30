using MikzeerGame;
using UnityEngine;
using UnityEngine.UI;

public class RibbonManagerUI : MonoBehaviour
{
    [SerializeField] private RectTransform rootRect;
    [SerializeField] private GameObject ribbonPrefab;
    [SerializeField] private RectTransform ribbonsParent;
    [SerializeField] private ScrollRect ribbonScrollRectParent;
    CardDisplay cardDisplay;

    public RiboonDisplay CreateNewCardRibbon()
    {
        GameObject cardRibbonPrefab = Instantiate(ribbonPrefab, ribbonsParent);
        RiboonDisplay ribbonDisplay = cardRibbonPrefab.GetComponent<RiboonDisplay>();
        ribbonDisplay.SetTransformsAndScrollRect(rootRect, ribbonScrollRectParent);
        return ribbonDisplay;
    }

    public void SetCardDisplay(CardDisplay cardDisplay)
    {
        this.cardDisplay = cardDisplay;
        cardDisplay.gameObject.SetActive(false);
    }

    public CardDisplay GetCardDisplay()
    {
        return cardDisplay;
    }
}
