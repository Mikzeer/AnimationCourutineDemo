using UnityEngine;
using UnityEngine.UI;

public class CardSlotUI : MonoBehaviour
{
    public GameObject libraryTextParent;
    public CardSlot cardSlot;
    [SerializeField] private Text libraryAmount;
    [SerializeField] private RectTransform cardSlotAmountParent;
    [SerializeField] private GameObject cardSlotAmountPrefab;
    [SerializeField] private GameObject notAvailablePanel;
    RectTransform cardSlotRect;
    CardAmountSlot[] amountSlots;
    MikzeerGame.CardDisplay cardDisplay;

    private void Awake()
    {
        cardSlotRect = GetComponent<RectTransform>();
    }

    public void InitializeCardSlot(CardSlot cardSlot)
    {
        this.cardSlot = cardSlot;
        libraryAmount.text = cardSlot.userCollectionAmount.ToString();
        HideLibraryAmount();
        cardSlot.OnCardInfoChange += ChangeSlotData;
        cardSlotRect = GetComponent<RectTransform>();

        InitializeSlotAmount(cardSlot);
    }

    private void InitializeSlotAmount(CardSlot cardSlot)
    {
        amountSlots = new CardAmountSlot[cardSlot.maxAmountPerDeck];
        for (int i = 1; i <= cardSlot.maxAmountPerDeck; i++)
        {
            GameObject cardSlotAmountUIPrefab = Instantiate(cardSlotAmountPrefab, cardSlotAmountParent);
            CardAmountSlot cAmountAux = cardSlotAmountUIPrefab.GetComponent<CardAmountSlot>();
            if (cardSlot.userCollectionAmount >= i)
            {
                cAmountAux.TurnOnHasOne();
            }

            if (cardSlot.deckAmount >= i)
            {
                cAmountAux.TurnOnHasInDeck();
            }

            amountSlots[i - 1] = cAmountAux;
        }
    }

    private void RefreshSlotAmount(CardSlot cardSlot)
    {
        for (int i = 0; i < amountSlots.Length; i++)
        {
            if (cardSlot.userCollectionAmount >= i)
            {
                amountSlots[i].TurnOnHasOne();
            }

            if (cardSlot.deckAmount >= i + 1)
            {
                amountSlots[i].TurnOnHasInDeck();
            }
            else
            {
                amountSlots[i].TurnOffHasInDeck();
            }
        }
    }

    public void ChangeSlotData(CardSlot cardSlot)
    {
        libraryAmount.text = cardSlot.userCollectionAmount.ToString();
        RefreshSlotAmount(cardSlot);
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public RectTransform GetCardSlotTransform()
    {
        return cardSlotRect;
    }

    public void DestroyCardSlotUI(CardSlot cardSlot)
    {
        cardSlot.OnCardInfoChange -= ChangeSlotData;
        Destroy(cardDisplay.gameObject);
        Destroy(gameObject);
    }

    public void SetCardDisplay(MikzeerGame.CardDisplay cardDisplay)
    {
        this.cardDisplay = cardDisplay;
    }

    public void SetIsAvailable(bool isAvailable)
    {
        if (isAvailable == true)
        {
            notAvailablePanel.SetActive(false);
        }
        else
        {
            notAvailablePanel.SetActive(true);
        }
    }

    public void ShowLibraryAmount()
    {
        libraryTextParent.SetActive(true);
    }

    public void HideLibraryAmount()
    {
        libraryTextParent.SetActive(false);
    }
}