using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PositionerDemo
{
    public class DeckBuilderCardOnlyClickUINEW : CardVisualCollectionUINEW, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        CardData pCardData;
        public event Action<CardData, CardSlotUI> OnCardClick;
        Vector2 mousePos;
        bool isOver;
        float scrollAdTime = 0f;
        float scrollTriggerTime = 0.6f;
        bool scrollTrigger = false;
        private ScrollRect ScrollRectParent;
        private IEnumerator ScrollRectCoroutine;

        public void SetEvent(Action<CardData, CardSlotUI> OnCardClick, CardData pCardData, CardSlotUI cardSlotUI)
        {
            this.OnCardClick = OnCardClick;
            this.pCardData = pCardData;
            SetSlot(cardSlotUI);
        }

        protected override void InitializeSimpleUI()
        {
            base.InitializeSimpleUI();
        }

        public void SetScrollRect(ScrollRect ScrollRectParent)
        {
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
            base.OnSuccesfulPointerEnter();
            isOver = true;
        }

        protected override void OnSuccesfulPointerExit()
        {
            base.OnSuccesfulPointerExit();
            isOver = false;
            scrollAdTime = 0;
        }         

        public override void OnPointerDown(PointerEventData eventData)
        {
            isOver = true;
            mousePos = Input.mousePosition;
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
            OnCardClick?.Invoke(pCardData, cardSlotUI);
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
}