using PositionerDemo;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckStatusVisualManager : MonoBehaviour
{
    [SerializeField] private DeckStatusRarityAmountSlot deckAmount = default;
    [SerializeField] private GameObject deckStatusSlotPrefab = default;
    [SerializeField] public RectTransform statusSlotPanelParent = default;
    [SerializeField] public GameObject statusCanvas = default;
    private DeckStatusRarityAmountVisualManager deckStatusSSlotslManager;
    public bool isInitialized = false;
    private void Awake()
    {
        DeckBuilderManager.OnDeckChange += DeckCreator_OnDeckChange;
    }

    private void Start()
    {
        //DeckCreator.OnDeckChange += DeckCreator_OnDeckChange;
        //DeckBuilderManager.OnDeckChange += DeckCreator_OnDeckChange;
        deckStatusSSlotslManager = new DeckStatusRarityAmountVisualManager();
    }
    
    private void DeckCreator_OnDeckChange(Deck pDeck)
    {
        deckAmount.UpdateActualAmount(pDeck.totalCards);
        deckStatusSSlotslManager.RefreshRarityAmount(pDeck.amountPerRarity);
    }

    public void CreateSlotUI(Dictionary<CardRarity, int> amountPerRarity)
    {
        foreach (KeyValuePair<CardRarity, int> rar in amountPerRarity)
        {
            deckStatusSSlotslManager.AddRarity(rar.Key, CreateNewStatusSlot(), rar.Value);
        }

        isInitialized = true;
    }

    public void SetMaxDeckAmount(int maxAmount)
    {
        deckAmount.SetMaxAmount(maxAmount);
    }

    private DeckStatusRarityAmountSlot CreateNewStatusSlot()
    {
        GameObject statusSlotPrefab = Instantiate(deckStatusSlotPrefab, statusSlotPanelParent);
        DeckStatusRarityAmountSlot deckStatusSlot = statusSlotPrefab.GetComponent<DeckStatusRarityAmountSlot>();
        return deckStatusSlot;
    }

    public void SetActiveStatusPanel(bool isActive)
    {
        statusCanvas.SetActive(isActive);
    }

    public void ResetSlotData()
    {
        deckStatusSSlotslManager.ClearAllActualAmount();
        deckAmount.UpdateActualAmount(0);
    }
}
