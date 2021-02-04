using System;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using DG.Tweening;

namespace PositionerDemo
{
    public class CardInGameUINEW : SimpleDragDropUI
    {
        [SerializeField] private GameObject placeholderPrefab = default;
        private GameObject holderAux;
        [SerializeField] private RectTransform cardFrontRect = default;
        bool isPointerOverCard = false;
        float timeToReach = 1.2f;
        float actualTimeOver = 0f;

        private Vector3 cardFrontStartPosition;
        private Vector3 initialScale;
        private float scaleFactor = 1.1f;
        CardData cardData;

        float cardHalfSizeX;
        float cardHalfSizeY;

        private Action<CardData> onTryUseCard; // ON TRY USE CARD DESDE EL CARD CONTROLLER
        private Action<CardData, Vector2> onCardInfoShow; // ESTO LO HACEMOS PARA SETEAR LA INFORMACION Y EL LUGAR DONDE DEBERIA MOSTRARSE DE LA CARD
        private Action onCardInfoPanelClose; // ESTO LO HACEMOS PARA PRENDER O APAGAR EL INFO PANEL NADA MAS.....

        public void SetCardData(CardData cardData, CardController cardController, CardManagerUI cardManagerUI, RectTransform playerHandTransform)
        {
            this.cardData = cardData;
            onTryUseCard += cardController.OnTryUseCard;
            onCardInfoPanelClose += cardManagerUI.OnCardDescriptionPanelClose;
            onCardInfoShow = cardManagerUI.OnCardDescriptionPanelRequired;
            parentRectTransform = playerHandTransform;
        }

        protected override void InitializeSimpleUI()
        {
            base.InitializeSimpleUI();
            initialScale = uiRectTansform.localScale;
            cardHalfSizeX = uiRectTansform.rect.size.x / 2;
            cardHalfSizeY = uiRectTansform.rect.size.y / 2;
            canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.blocksRaycasts = false;
        }

        public void CanvasGroupRaycast(bool block)
        {
            canvasGroup.blocksRaycasts = block;
        }

        protected override void OnSuccesfulPointerEnter()
        {
            isPointerOverCard = true;
            StartCoroutine(mouseOverToShowInfoPanel(uiRectTansform.position));
            // NEW
            startAnchoredPosition = uiRectTansform.anchoredPosition;
            startSiblingIndex = uiRectTansform.GetSiblingIndex();
            //
            cardFrontStartPosition = cardFrontRect.anchoredPosition;
            Vector3 newcardFrontStartPosition = cardFrontStartPosition + new Vector3(0, 50, 0);
            cardFrontRect.DOAnchorPos(newcardFrontStartPosition, 0.5f);
            uiRectTansform.DOScale(initialScale * scaleFactor, 0.5f); // SetEase(ease)
        }

        IEnumerator mouseOverToShowInfoPanel(Vector3 cardPosition)
        {
            actualTimeOver = 0;
            while (actualTimeOver < timeToReach)
            {
                if (isPointerOverCard == false) yield break;
                actualTimeOver += Time.deltaTime;
                yield return null;
            }
            if (isSomethingDraggin) yield break;
            actualTimeOver = 0;
            // aca tenemos que hacer el InfoPanel haga el SetText(hola soy la cartita)
            ////Vector2 cardRectInScreenPosition = Camera.main.WorldToScreenPoint(uiRectTansform.position);
            //Vector2 cardRectInScreenPosition = transform.position;
            ////cardRectInScreenPosition += new Vector2(uiRectTansform.sizeDelta.x / 2 * canvas.scaleFactor, 0);
            //cardRectInScreenPosition += new Vector2(uiRectTansform.sizeDelta.x / 2 * canvas.scaleFactor, uiRectTansform.sizeDelta.x / 2 * canvas.scaleFactor);
            //onCardInfoShow?.Invoke(cardData, cardRectInScreenPosition / canvas.scaleFactor);
            int screenWidth = Screen.width;
            int screenHeight = Screen.height;
            float halfScreenWidth = screenWidth / 2 / canvas.scaleFactor;
            float halfScreenHeight = screenHeight / 2 / canvas.scaleFactor;
            float finalPositionX = halfScreenWidth - (-1 * parentRectTransform.localPosition.x) - (-1 * uiRectTansform.localPosition.x);
            float finalPositionY = halfScreenHeight - (-1 * parentRectTransform.localPosition.y) - (-1 * uiRectTansform.localPosition.y);
            Vector2 finalPositionInfoPanel = new Vector2(finalPositionX + cardHalfSizeX, finalPositionY + cardHalfSizeY);



            onCardInfoShow?.Invoke(cardData, finalPositionInfoPanel);
            //Debug.Log("finalPositionX " + finalPositionX);
            //Debug.Log("finalPositionY " + finalPositionY);
            //Debug.Log("cardHalfSizeX " + cardHalfSizeX);
            //Debug.Log("cardHalfSizeY " + cardHalfSizeY);
            //Debug.Log("finalPositionInfoPanel " + finalPositionInfoPanel);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            uiRectTansform.DOScale(initialScale, 0.1f); // SetEase(ease)
            cardFrontRect.DOAnchorPos(cardFrontStartPosition, 0.1f);
        }

        protected override void OnSuccesfulBeginDrag(PointerEventData eventData)
        {
            onCardInfoPanelClose?.Invoke();
            isPointerOverCard = false;
            canvasGroup.blocksRaycasts = false;
            // INSTANCIAMOS EL PLACE HOLDER
            holderAux = Instantiate(placeholderPrefab);
            // LE SETEAMOS EL PARENT 
            holderAux.transform.SetParent(parentRectTransform);
            // LO PONEMOS EN EL MISMO SIBLING INDEX QUE ESTE 
            holderAux.transform.SetSiblingIndex(startSiblingIndex);
        }

        public override void OnDrag(PointerEventData eventData)
        {
            base.OnDrag(eventData);
            if (holderAux != null)
            {
                int newSiblignIndex = parentRectTransform.childCount;
                // RECORRO TODOS LOS CHILD, EN ESTE CASO LAS CARDS
                for (int i = 0; i < parentRectTransform.childCount; i++)
                {
                    // SI LA POSICION DE LA CARTA QUE ESTOY DRAGEANDO, ES MENOR A LA POSICION DE LA CARTA DE LA HAND
                    // SIEMPRE EMPEZAMOS DE 0 A N DENTRO DEL SIBLING INDEX DEL TRANSFORM
                    if (transform.position.x < parentRectTransform.GetChild(i).position.x)
                    {
                        // el nuevo sibling index pasa a ser el de la primera card encontrada dentro de la hand
                        newSiblignIndex = i;
                        if (holderAux.transform.GetSiblingIndex() < newSiblignIndex)
                        {
                            newSiblignIndex--;
                        }
                        break;
                    }
                }
                holderAux.transform.SetSiblingIndex(newSiblignIndex);
            }
        }

        protected override bool IsSuccesfulDropArea(RaycastResult result)
        {
            if (result.gameObject.CompareTag("DropeableArea"))
            {
                return true;
            }
            return false;
        }

        protected override void OnSuccesfulDrop(RaycastResult result)
        {
            if (result.gameObject.CompareTag("DropeableArea"))
            {
                Debug.Log("Hit " + result.gameObject.name);
                if (onTryUseCard != null)
                {
                    onTryUseCard?.Invoke(cardData);
                    parentRectTransform.GetChild(startSiblingIndex).SetAsLastSibling();
                }
                else
                {
                    Debug.Log("CARD NOT CREATED BY CODE");
                }
                return;
            }
        }

        protected override void OnFaliedDrop()
        {
            Debug.Log("NO CHOCO CONTRA NINGUNA UI");
            uiRectTansform.SetParent(parentRectTransform);
            canvasGroup.blocksRaycasts = true;
            if (holderAux != null)
            {
                uiRectTansform.SetSiblingIndex(holderAux.transform.GetSiblingIndex());
                Destroy(holderAux);
            }
            else
            {
                uiRectTansform.SetSiblingIndex(startSiblingIndex);
            }           
        }

        protected override void OnSuccesfulPointerExit()
        {
            isPointerOverCard = false;
            onCardInfoPanelClose?.Invoke();
            Debug.Log("POINTER EXIT");
            cardFrontRect.DOAnchorPos(cardFrontStartPosition, 0.5f);
            uiRectTansform.DOScale(initialScale, 0.5f); // SetEase(ease)
        }
    }
}