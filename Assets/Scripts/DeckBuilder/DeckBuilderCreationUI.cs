using PositionerDemo;
using UnityEngine;

public class DeckBuilderCreationUI : MonoBehaviour
{
    [SerializeField] private GameObject deckPanel = default;
    [SerializeField] private GameObject cardInDeckPanel = default;
    [SerializeField] private GameObject backPanel = default;
    [SerializeField] private DeckStatusVisualManager deckStatusVisualManager = default;

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
