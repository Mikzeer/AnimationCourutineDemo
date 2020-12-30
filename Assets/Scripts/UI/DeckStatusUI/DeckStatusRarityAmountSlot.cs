using PositionerDemo;
using UnityEngine;
using UnityEngine.UI;

public class DeckStatusRarityAmountSlot : MonoBehaviour
{
    [SerializeField] private Text txtActualAmount;
    [SerializeField] private Text txtMaxAmount;
    [SerializeField] private Image fillImage;
    int actualAmount = 0;
    int maxAmount = 0;

    public void UpdateActualAmount(int actualAmount)
    {
        txtActualAmount.text = actualAmount.ToString();
        this.actualAmount = actualAmount;
        UpdateFillImage();
    }

    public void SetMaxAmountAndRarity(int maxAmount, CardRarity rarity)
    {
        SetFillColorByRarity(rarity);
        txtMaxAmount.text = maxAmount.ToString();
        this.maxAmount = maxAmount;
        UpdateFillImage();
    }

    public void SetMaxAmount(int maxAmount)
    {
        SetFillColorByRarity(CardRarity.NONE);
        txtMaxAmount.text = maxAmount.ToString();
        this.maxAmount = maxAmount;
        txtActualAmount.text = actualAmount.ToString();
        UpdateFillImage();
    }

    public void UpdateFillImage()
    {
        float fillPerc = actualAmount * 100 / maxAmount;
        fillImage.fillAmount = fillPerc / 100;
    }

    public void SetFillColorByRarity(CardRarity rarity)
    {
        Color fillColor = Color.white;
        switch (rarity)
        {
            case CardRarity.BASIC:
                fillColor = Color.yellow;
                break;
            case CardRarity.COMMON:
                fillColor = Color.green;
                break;
            case CardRarity.RARE:
                fillColor = Color.blue;
                break;
            case CardRarity.EPIC:
                fillColor = Color.red;
                break;
            case CardRarity.LEGENDARY:
                fillColor = Color.magenta;
                break;
            default:
                fillColor = Color.black;
                break;
        }
        fillImage.color = fillColor;
    }
}