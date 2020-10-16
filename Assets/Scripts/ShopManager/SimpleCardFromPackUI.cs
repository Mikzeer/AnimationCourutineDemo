using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class SimpleCardFromPackUI : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform cardRectTansform;
    public Image GlowImage;
    private Color32 GlowColor;
    PackOpeningArea packOpeningArea;
    private float InitialScale;
    private float scaleFactor = 1.1f;
    private bool turnedOver = false;

    public void Awake()
    {
        cardRectTansform = GetComponent<RectTransform>();
        InitialScale = cardRectTansform.localScale.x;
    }

    public void SetSimpleCardFromPackUI(Color32 GlowColor, PackOpeningArea packOpeningArea)
    {
        this.GlowColor = GlowColor;
        this.packOpeningArea = packOpeningArea;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        cardRectTansform.DOScale(InitialScale * scaleFactor, 0.5f);
        GlowImage.gameObject.SetActive(true);
        GlowImage.DOColor(GlowColor, 0.5f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (turnedOver)
            return;

        turnedOver = true;
        // turn the card over
        transform.DORotate(Vector3.zero, 0.5f);
        // add this card to collection as unlocked
        packOpeningArea.NumberOfCardsOpenedFromPack++;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        cardRectTansform.DOScale(InitialScale, 0.5f);
        GlowImage.gameObject.SetActive(false);
        GlowImage.DOColor(Color.clear, 0.5f);
    }

}