using PositionerDemo;
using UnityEngine;

public class DeckBuilderCreationUI : MonoBehaviour
{
    [SerializeField] private GameObject deckPanel;
    [SerializeField] private GameObject cardInDeckPanel;
    [SerializeField] private GameObject backPanel;
    [SerializeField] private DeckStatusVisualManager deckStatusVisualManager;

    public void SetActiveStatusPanel()
    {
        deckPanel.SetActive(false);
        cardInDeckPanel.SetActive(true);
        if (deckStatusVisualManager.isInitialized == false)
        {
            deckStatusVisualManager.CreateSlotUI(CardPropertiesDatabase.GetAmountPerCardPerLevelPerDeck());
            deckStatusVisualManager.SetMaxDeckAmount(CardPropertiesDatabase.maxAmountOfCardsPerDeck);
        }
        deckStatusVisualManager.SetActiveStatusPanel(true);
        backPanel.SetActive(false);
    }

    public void Clear()
    {
        deckPanel.SetActive(true);
        cardInDeckPanel.SetActive(false);
        deckStatusVisualManager.ResetSlotData();
        deckStatusVisualManager.SetActiveStatusPanel(false);
        backPanel.SetActive(true);
    }

}
