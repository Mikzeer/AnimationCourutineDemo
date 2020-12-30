using MikzeerGame;
using PositionerDemo;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RiboonDisplay : SimpleClickeableUI, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private Text txtName;
    [SerializeField] private Text txtAmount;
    private IEnumerator actualCoroutine;
    public CardDisplay cardDisplay;
    RibbonData ribbonData;
    RectTransform cardRect;
    RectTransform rootRect;
    Vector2 mousePos;
    public event Action<RibbonData> OnRibbonClick;
    float adTime = 0f;
    float triggerTime = 0.5f;
    bool trigger = false;
    private IEnumerator ScrollRectCoroutine;
    private ScrollRect ScrollRectParent;
    bool isOver;
    float scrollAdTime = 0f;
    float scrollTriggerTime = 0.6f;
    bool scrollTrigger = false;
    
    public void SetData(RibbonData ribbonData, Action<RibbonData> OnRibbonClick, CardDisplay cardDisplay)
    {
        txtName.text = ribbonData.name;
        txtAmount.text = ribbonData.amount.ToString();
        this.ribbonData = ribbonData;
        this.OnRibbonClick = OnRibbonClick;
        this.cardDisplay = cardDisplay;
        cardRect = cardDisplay.GetComponent<RectTransform>();
    }

    public void SetTransformsAndScrollRect(RectTransform rootRect, ScrollRect ScrollRectParent)
    {
        this.rootRect = rootRect;
        this.ScrollRectParent = ScrollRectParent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (ScrollRectParent != null)
            ScrollRectParent.OnBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (ScrollRectParent != null)
            ScrollRectParent.OnDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (ScrollRectParent != null)
            ScrollRectParent.OnEndDrag(eventData);
    }

    protected override void OnSuccesfulPointerEnter()
    {
        trigger = true;
        adTime = 0;
        isOver = true;
        if (actualCoroutine != null) StopCoroutine(actualCoroutine);

        actualCoroutine = SetActiveCardDisplay();
        StartCoroutine(actualCoroutine);
    }

    protected override void OnSuccesfulPointerExit()
    {
        StopShowingDisplay();
        isOver = false;
        scrollAdTime = 0;
    }
   
    public override void OnPointerDown(PointerEventData eventData)
    {
        isOver = true;
        mousePos = Input.mousePosition;
        if (trigger)
        {
            StopShowingDisplay();
        }
        if (ScrollRectCoroutine != null) StopCoroutine(ScrollRectCoroutine);
        ScrollRectCoroutine = SetActiveTimeCounterToTriggerMouseInput();
        StartCoroutine(ScrollRectCoroutine);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (isOver == false)
        {
            //Debug.Log("EL MOUSE NO ESTA SOBRE LA CARD");
            return;
        }
        if (scrollAdTime >= scrollTriggerTime)
        {
            //Debug.Log("SE SUPERO EL TIEMPO DE APRETADO " + scrollAdTime);
            isOver = false;
            scrollAdTime = 0;
            return;
        }
        Vector2 newMousePos = Input.mousePosition;
        if (Vector2.Distance(newMousePos, mousePos) > 50f)
        {
            //Debug.Log("SE SUPERO LA DISTANCIA");
            scrollAdTime = 0;
            isOver = false;
            return;
        }

        //Debug.Log("TIEMPO DE APRETADO " + scrollAdTime);
        scrollAdTime = 0;
        isOver = false;
        OnRibbonClick?.Invoke(ribbonData);
    }

    public void AddAmount()
    {
        ribbonData.amount++;
        txtAmount.text = ribbonData.amount.ToString();
    }

    public void RestAmount()
    {
        ribbonData.amount--;
        txtAmount.text = ribbonData.amount.ToString();
    }

    public void SetAndShowDisplay()
    {
        cardDisplay.SetDisplay(ribbonData.cardData);
        if (cardRect != null)
        {
            float cardHalfSizeX = cardRect.rect.size.x / 2;
            float panelSizeX = rootRect.rect.size.x;
            float canvasResoX = canvasScaler.referenceResolution.x;
            float newPosX = canvasResoX - panelSizeX - (cardHalfSizeX * 1.3f);

            Vector2 mousePosition = Input.mousePosition / canvas.scaleFactor;
            Vector3 locPoint = new Vector3(newPosX, mousePosition.y, cardRect.localPosition.z);
            Vector3 clampedPostion = Helper.KeepRectInsideScreen(cardRect, locPoint, canvasScaler);
            cardRect.anchoredPosition = clampedPostion;
        }
        cardDisplay.gameObject.SetActive(true);
    }

    public void StopShowingDisplay()
    {
        if (actualCoroutine != null) StopCoroutine(actualCoroutine);

        cardDisplay.gameObject.SetActive(false);
        trigger = false;
        adTime = 0;
    }

    public IEnumerator SetActiveCardDisplay()
    {
        bool show = true;
        while (show)
        {
            adTime += Time.deltaTime;
            if (adTime >= triggerTime || trigger == false)
            {
                show = false;
                adTime = 0;
            }
            yield return null;
        }
        yield return null;

        if (trigger)
        {
            SetAndShowDisplay();
        }
    }

    public IEnumerator SetActiveTimeCounterToTriggerMouseInput()
    {
        bool show = true;
        while (show)
        {
            scrollAdTime += Time.deltaTime;
            if (scrollAdTime >= scrollTriggerTime || isOver == false)
            {
                show = false;
            }
            yield return null;
        }
        yield return null;
    }
}