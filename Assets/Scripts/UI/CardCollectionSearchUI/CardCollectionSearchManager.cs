using PositionerDemo;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardCollectionSearchManager : MonoBehaviour
{
    [SerializeField] private List<CardRarityInteractuableImage> cardRarityInteactImage = default;
    [SerializeField] private List<CardActivationTypeInteractuableImage> cardActivationTypeInteractImage = default;
    [SerializeField] private List<CardTypeInteractuableImage> cardTypeInteractuableImage = default;
    [SerializeField] private CardPlayerDontOwnsInteractuableImage cardPlayerDontOwnsInteractuable = default;
    [SerializeField] private Toggle chainToggle = default;
    [SerializeField] private Toggle darkToggle = default;
    [SerializeField] private InputField tagInputFiled = default;
    [SerializeField] private Button clearBtn = default;
    [SerializeField] private CardCollectionVisualManager cardCollectionVisualManager = default;
    VisualFiltterUIHandler visualFiltterUIHandler;
    private void Start()
    {
        CreateInteractuableImages();
        visualFiltterUIHandler = new VisualFiltterUIHandler(cardCollectionVisualManager);
    }

    private void OnEnable()
    {
        chainToggle.onValueChanged.AddListener(OnIsChainableToggleChange);
        darkToggle.onValueChanged.AddListener(OnDarkPointsToggleChange);
        tagInputFiled.onEndEdit.AddListener(OnSearchByTagButtonPress);
        clearBtn.onClick.AddListener(OnClearSearchByTagButtonPress);
    }

    private void OnDisable()
    {
        chainToggle.onValueChanged.RemoveAllListeners();
        darkToggle.onValueChanged.RemoveAllListeners();
        tagInputFiled.onEndEdit.RemoveAllListeners();
        clearBtn.onClick.RemoveAllListeners();
    }

    public void CreateInteractuableImages()
    {
        // 5 de Lvl
        // 3 de actTYpe
        // 3 de Type
        cardPlayerDontOwnsInteractuable.SetCardPlayerDontOwnsInteractuableImage(this, true, "SHOW CARDS PLAYER DONT OWN");

        for (int i = 0; i < cardRarityInteactImage.Count; i++)
        {
            switch (i)
            {
                case 0:
                    cardRarityInteactImage[i].SetCardRarityInteractuableImage(this, CardRarity.BASIC, "1");
                    break;
                case 1:
                    cardRarityInteactImage[i].SetCardRarityInteractuableImage(this, CardRarity.COMMON, "2");
                    break;
                case 2:
                    cardRarityInteactImage[i].SetCardRarityInteractuableImage(this, CardRarity.RARE, "3");
                    break;
                case 3:
                    cardRarityInteactImage[i].SetCardRarityInteractuableImage(this, CardRarity.EPIC, "4");
                    break;
                case 4:
                    cardRarityInteactImage[i].SetCardRarityInteractuableImage(this, CardRarity.LEGENDARY, "5");
                    break;
                default:
                    cardRarityInteactImage[i].SetCardRarityInteractuableImage(this, CardRarity.BASIC, "1");
                    break;
            }
        }

        for (int i = 0; i < cardActivationTypeInteractImage.Count; i++)
        {
            switch (i)
            {
                case 0:
                    cardActivationTypeInteractImage[i].SetCardActivationTypeInteractuableImage(this, ACTIVATIONTYPE.HAND, "H");
                    break;
                case 1:
                    cardActivationTypeInteractImage[i].SetCardActivationTypeInteractuableImage(this, ACTIVATIONTYPE.AUTOMATIC, "A");
                    break;
                case 2:
                    cardActivationTypeInteractImage[i].SetCardActivationTypeInteractuableImage(this, ACTIVATIONTYPE.AUTOMATICNOTDESCARTABLE, "An");
                    break;
                default:
                    cardActivationTypeInteractImage[i].SetCardActivationTypeInteractuableImage(this, ACTIVATIONTYPE.HAND, "H");
                    break;
            }
        }

        for (int i = 0; i < cardTypeInteractuableImage.Count; i++)
        {
            switch (i)
            {
                case 0:
                    cardTypeInteractuableImage[i].SetCardTypeInteractuableImage(this, CARDTYPE.BUFF, "Bf");
                    break;
                case 1:
                    cardTypeInteractuableImage[i].SetCardTypeInteractuableImage(this, CARDTYPE.NERF, "Nf");
                    break;
                case 2:
                    cardTypeInteractuableImage[i].SetCardTypeInteractuableImage(this, CARDTYPE.NEUTRAL, "Nt");
                    break;
                default:
                    cardTypeInteractuableImage[i].SetCardTypeInteractuableImage(this, CARDTYPE.BUFF, "Bf");
                    break;
            }
        }
    }

    #region FILTTERS

    public void OnFiltterByRaraityButtonPress(CardRarity rarity)
    {
        visualFiltterUIHandler.OnFiltterByRaraityButtonPress(cardRarityInteactImage, rarity);
    }

    public void OnFiltterByActivationTypeButtonPress(ACTIVATIONTYPE actType)
    {
        visualFiltterUIHandler.OnFiltterByActivationTypeButtonPress(cardActivationTypeInteractImage, actType);
    }

    public void OnFiltterByTypeButtonPress(CARDTYPE cardType)
    {
        visualFiltterUIHandler.OnFiltterByTypeButtonPress(cardTypeInteractuableImage, cardType);
    }

    public void OnIsChainableToggleChange(bool isOn)
    {
        visualFiltterUIHandler.OnIsChainableToggleChange(isOn);
    }

    public void OnDarkPointsToggleChange(bool isOn)
    {
        visualFiltterUIHandler.OnDarkPointsToggleChange(isOn);
    }

    public void OnSearchByTagButtonPress(string tagToSearch)
    {
        visualFiltterUIHandler.OnSearchByTagButtonPress(tagToSearch);
    }

    public void OnClearSearchByTagButtonPress()
    {
        tagInputFiled.text = string.Empty;
        visualFiltterUIHandler.OnClearSearchByTagButtonPress();
    }

    public void OnOnlyUserOwnButtonPress(bool showOnlyOwned)
    {
        visualFiltterUIHandler.OnOnlyUserOwnButtonPress(showOnlyOwned);
    }

    public void ClearAllFiltters()
    {
        cardPlayerDontOwnsInteractuable.backgroundImage.color = cardPlayerDontOwnsInteractuable.UnactiveColor;
        cardPlayerDontOwnsInteractuable.IsPressed = false;
        tagInputFiled.text = string.Empty;
        chainToggle.isOn = false;
        darkToggle.isOn = false;
        foreach (CardRarityInteractuableImage i in cardRarityInteactImage)
        {
            i.backgroundImage.color = i.UnactiveColor;
            i.IsPressed = false;
        }
        foreach (CardActivationTypeInteractuableImage i in cardActivationTypeInteractImage)
        {
            i.backgroundImage.color = i.UnactiveColor;
            i.IsPressed = false;
        }
        foreach (CardTypeInteractuableImage i in cardTypeInteractuableImage)
        {
            i.backgroundImage.color = i.UnactiveColor;
            i.IsPressed = false;
        }
        visualFiltterUIHandler.ClearAllFiltters();
    }

    #endregion
}