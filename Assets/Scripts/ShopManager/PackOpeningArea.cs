using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PositionerDemo;
using DG.Tweening;
using System.Threading.Tasks;
using System.Linq;

public class PackOpeningArea : MonoBehaviour
{
    #region VARIABLES

    public bool AllowedToDragAPack { get; set; }
    public Transform packOpeningParent;
    public GameObject cardPrefab;
    public Button DoneButton;
    public Button BackButton;
    [Header("Probabilities")]
    [Range(0, 1)]
    public float LegendaryProbability;
    [Range(0, 1)]
    public float EpicProbability;
    [Range(0, 1)]
    public float RareProbability;
    // these are the glow colors that will show while opening cards or you can use colors from  RarityColors
    [Header("Colors")]
    public Color32 LegendaryColor;
    public Color32 EpicColor;
    public Color32 RareColor;
    public Color32 CommonColor;

    public Dictionary<CardRarity, Color32> GlowColorsByRarity = new Dictionary<CardRarity, Color32>();

    public bool giveAtLeastOneRare = false;

    public RectTransform[] SlotsForCards;

    private List<GameObject> CardsFromPackCreated = new List<GameObject>();
    private int numOfCardsOpened = 0;
    public int NumberOfCardsOpenedFromPack
    {
        get { return numOfCardsOpened; }
        set
        {
            numOfCardsOpened = value;
            //Debug.Log("numOfCardsOpened " + numOfCardsOpened);
            //Debug.Log("SlotsForCards.Length " + SlotsForCards.Length);
            if (value == SlotsForCards.Length)
            {
                // activate the Done button
                DoneButton.gameObject.SetActive(true);
            }
        }
    }
    [SerializeField] private ShopManager shopManager = default;
    [SerializeField] private GameMenuManager gameMenuManager = default;
    #endregion

    private void Awake()
    {
        AllowedToDragAPack = true;

        GlowColorsByRarity.Add(CardRarity.COMMON, CommonColor);
        GlowColorsByRarity.Add(CardRarity.RARE, RareColor);
        GlowColorsByRarity.Add(CardRarity.EPIC, EpicColor);
        GlowColorsByRarity.Add(CardRarity.LEGENDARY, LegendaryColor);
    }

    private async Task<GameObject> CardFromPack(CardRarity rarity, Transform cardParent)
    {

        CardCollectionSearchFiltter cardCollectionSearchFiltter = new CardCollectionSearchFiltter();
        List<CardData> cDat = gameMenuManager.GetAllCardDataArray().ToList();
        List<CardData> CardsDataOfThisRarity = cardCollectionSearchFiltter.GetCardsDataWithCardRarity(cDat, rarity);

        if (CardsDataOfThisRarity.Count == 0)
        {
            //Debug.Log("NOT FOUND OF RARITY " + rarity);
            cDat = gameMenuManager.GetAllCardDataArray().ToList();
            CardsDataOfThisRarity = cardCollectionSearchFiltter.GetCardsDataWithCardRarity(cDat, CardRarity.COMMON);
            rarity = CardRarity.COMMON;
        }
        else
        {
            //Debug.Log("FOUND OF RARITY " + rarity);
        }

        CardData cardDataAux = CardsDataOfThisRarity[Random.Range(0, CardsDataOfThisRarity.Count)];

        gameMenuManager.AddCardToGameCollectionDictionary(cardDataAux);

        bool isLoaded = await gameMenuManager.AddNewCardToUserCollection(cardDataAux);

        GameObject card = Instantiate(cardPrefab, cardParent);
        card.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        card.GetComponentInChildren<SimpleCardFromPackUINEW>().SetSimpleCardFromPackUI(GlowColorsByRarity[rarity], this);
        MikzeerGame.CardDisplay cardDisplay = card.GetComponent<MikzeerGame.CardDisplay>();
        cardDisplay.SetDisplay(cardDataAux);
        return card;
    }

    public async void ShowPackOpening(Vector3 cardsInitialPosition)
    {
        // Allow To Drag Another Pack Only After DoneButton Is pressed

        // 1) Determine rarity of all cards
        CardRarity[] rarities = new CardRarity[SlotsForCards.Length];
        bool AtLeastOneRareGiven = false;
        for (int i = 0; i < rarities.Length; i++)
        {
            // determine rarity of this card
            float prob = Random.Range(0f, 1f);
            if (prob < LegendaryProbability)
            {
                rarities[i] = CardRarity.LEGENDARY;
                AtLeastOneRareGiven = true;
            }
            else if (prob < EpicProbability)
            {
                rarities[i] = CardRarity.EPIC;
                AtLeastOneRareGiven = true;
            }
            else if (prob < RareProbability)
            {
                rarities[i] = CardRarity.RARE;
                AtLeastOneRareGiven = true;
            }
            else
                rarities[i] = CardRarity.COMMON;
        }

        if (AtLeastOneRareGiven == false && giveAtLeastOneRare)
        {
            rarities[Random.Range(0, rarities.Length)] = CardRarity.RARE;
        }

        for (int i = 0; i < rarities.Length; i++)
        {
            GameObject card = await CardFromPack(rarities[i], SlotsForCards[i]);
            //card.GetComponentInChildren<SimpleCardFromPackUINEW>().SetSimpleCardFromPackUI(GlowColorsByRarity[rarities[i]], this);
            CardsFromPackCreated.Add(card);
            card.transform.localPosition = cardsInitialPosition;
            card.transform.DOLocalMove(SlotsForCards[i].position, 0.5f);
        }

        gameMenuManager.LoadUserCollectionFromFirebase();
    }

    public void Done()
    {
        AllowedToDragAPack = true;
        NumberOfCardsOpenedFromPack = 0;
        while (CardsFromPackCreated.Count > 0)
        {
            GameObject g = CardsFromPackCreated[0];
            CardsFromPackCreated.RemoveAt(0);
            Destroy(g);
        }
        BackButton.interactable = true;
        DoneButton.gameObject.SetActive(false);
    }

    public void OnCardPackOpen(RectTransform cardPackRect)
    {
        cardPackRect.SetParent(packOpeningParent);

        // Start a dotween sequence
        Sequence tweenSequence = DOTween.Sequence();
        // 1) raise the pack to opening position
        //tweenSequence.Append(cardPackRect.DOLocalMove(packOpeningParent.position, 0.5f));
        tweenSequence.Append(cardPackRect.DOShakeRotation(1f, 20f, 20));

        tweenSequence.OnComplete(() =>
        {
            ShowPackOpening(cardPackRect.position);

            if (shopManager.PacksCreated > 0)
            {
                gameMenuManager.RestOneOpenPackFromFirebase();
                shopManager.PacksCreated--;
            }               
            
            // 4) destroy this pack in the end of the sequence
            Destroy(cardPackRect.gameObject);
        });
    }

}
