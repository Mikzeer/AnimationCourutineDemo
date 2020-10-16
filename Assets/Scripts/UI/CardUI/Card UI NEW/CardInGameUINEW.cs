using System;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace PositionerDemo
{
    public class CardInGameUINEW : SimpleDragDropUI
    {
        bool isPointerOverCard = false;
        float timeToReach = 1.2f;
        float actualTimeOver = 0f;
        bool isComingFromDrag = false;

        private Action<CardInGameUINEW> onCardUse;
        private Action<string, Vector2> onCardInfo;
        private Action<bool> onCardEnter;
        private Action<int> onCardDrop;
        public int ID;

        public void SetRealCardDrop(Action<int> onCardDrop, int ID)
        {
            this.onCardDrop = onCardDrop;
            this.ID = ID;
        }

        public void SetPlayerHandTransform(RectTransform playerHandTransform, Action<CardInGameUINEW> OnCardUse, Action<string, Vector2> onCardInfo, Action<bool> onCardEnter)
        {
            this.parentRectTransform = playerHandTransform;
            onCardUse = OnCardUse;
            this.onCardInfo = onCardInfo;
            this.onCardEnter = onCardEnter;
        }

        protected override void InitializeSimpleUI()
        {
            base.InitializeSimpleUI();
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void CanvasGroupRaycast(bool block)
        {
            canvasGroup.blocksRaycasts = block;
        }

        protected override void OnSuccesfulPointerEnter()
        {
            StartCoroutine(mouseOverToShowInfoPanel(uiRectTansform.position));
            parentRectTransform.GetChild(parentRectTransform.childCount - 1).SetSiblingIndex(startSiblingIndex);
            uiRectTansform.SetParent(parentRectTransform.parent.parent.transform);
            Vector2 offset = new Vector2(80, 0);
            uiRectTansform.anchoredPosition += offset;
            isPointerOverCard = true;
        }

        IEnumerator mouseOverToShowInfoPanel(Vector3 cardPosition)
        {
            actualTimeOver = 0;

            while (actualTimeOver < timeToReach)
            {
                if (isPointerOverCard == false)
                {
                    yield break;
                }
                actualTimeOver += Time.deltaTime;
                yield return null;
            }

            if (isSomethingDraggin)
            {
                yield break;
            }

            actualTimeOver = 0;
            // aca tenemos que hacer el InfoPanel haga el SetText(hola soy la cartita)
            onCardEnter?.Invoke(true);
            Vector2 cardRectInScreenPosition = Camera.main.WorldToScreenPoint(uiRectTansform.position);
            cardRectInScreenPosition += new Vector2(uiRectTansform.sizeDelta.x / 2 * canvas.scaleFactor, 0);
            onCardInfo?.Invoke("CARD NAME: " + name, cardRectInScreenPosition / canvas.scaleFactor);
        }

        protected override void OnSuccesfulBeginDrag(PointerEventData eventData)
        {
            onCardEnter?.Invoke(false);
            isPointerOverCard = false;
        }

        protected override bool IsSuccesfulDropArea(RaycastResult result)
        {
            if (result.gameObject.CompareTag("DropeableArea") || result.gameObject.CompareTag("Card"))
            {
                // ACA TENGO QUE CHEQUEAR SI LA CARD ES MIA PARA QUE SEA CORRECTO, YA QUE SI ES DEL JUGADOR CONTRARIO NO CUENTA
                return true;
            }

            return false;
        }

        protected override void OnSuccesfulDrop(RaycastResult result)
        {
            isComingFromDrag = true;
            if (result.gameObject.CompareTag("DropeableArea"))
            {
                Debug.Log("Hit " + result.gameObject.name);
                if (onCardUse != null)
                {
                    onCardUse.Invoke(this);
                    onCardDrop?.Invoke(ID);
                    parentRectTransform.GetChild(startSiblingIndex).SetAsLastSibling();
                }
                else
                {
                    Debug.Log("CARD NOT CREATED BY CODE");
                }
                return;
            }

            if (result.gameObject.CompareTag("Card"))
            {
                Debug.Log("CARD");
                int newSiblingIndex = result.gameObject.transform.GetSiblingIndex();
                parentRectTransform.GetChild(startSiblingIndex).SetAsLastSibling();
                uiRectTansform.SetParent(parentRectTransform);
                uiRectTansform.SetSiblingIndex(newSiblingIndex);
                canvasGroup.blocksRaycasts = true;
                return;
            }
        }

        protected override void OnFaliedDrop()
        {
            isComingFromDrag = true;
            Debug.Log("NO CHOCO CONTRA NINGUNA UI");
            uiRectTansform.SetParent(parentRectTransform);
            parentRectTransform.GetChild(startSiblingIndex).SetAsLastSibling();
            uiRectTansform.SetSiblingIndex(startSiblingIndex);
            canvasGroup.blocksRaycasts = true;
        }

        protected override void OnSuccesfulPointerExit()
        {
            isPointerOverCard = false;
            onCardEnter?.Invoke(false);

            if (isComingFromDrag)
            {
                isComingFromDrag = false;
                return;
            }

            parentRectTransform.GetChild(startSiblingIndex).SetAsLastSibling();
            uiRectTansform.SetParent(parentRectTransform);
            uiRectTansform.SetSiblingIndex(startSiblingIndex);
        }

    }

}
