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

        private Action<CardData, RectTransform> onTryUseCard; // ON TRY USE CARD DESDE EL CARD CONTROLLER
        private Action<CardData, RectTransform, Vector2> onCardInformationShow;// ESTO LO HACEMOS PARA SETEAR LA INFORMACION Y EL LUGAR DONDE DEBERIA MOSTRARSE DE LA CARD
        private Action onCardInfoPanelClose; // ESTO LO HACEMOS PARA PRENDER O APAGAR EL INFO PANEL NADA MAS.....

        Tween scaleTween;
        Tween positionTween;

        public void SetCardData(CardData cardData, CardController cardController, CardManagerUI cardManagerUI, RectTransform playerHandTransform)
        {
            this.cardData = cardData;
            onTryUseCard += cardController.OnTryUseCard;
            onCardInfoPanelClose += cardManagerUI.OnCardDescriptionPanelClose;
            onCardInformationShow = cardManagerUI.OnCardInformationRequired;
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
            //Debug.Log("ENTER "); 
            startAnchoredPosition = uiRectTansform.anchoredPosition;
            startSiblingIndex = uiRectTansform.GetSiblingIndex();
            //
            //cardFrontStartPosition = cardFrontRect.anchoredPosition;
            cardFrontStartPosition = new Vector3(0, 0, 0);
            Vector3 newcardFrontStartPosition = cardFrontStartPosition + new Vector3(0, 50, 0);
            positionTween = cardFrontRect.DOAnchorPos(newcardFrontStartPosition, 0.5f);
            scaleTween = uiRectTansform.DOScale(initialScale * scaleFactor, 0.5f); // SetEase(ease)
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
            //int screenWidth = Screen.width;
            //int screenHeight = Screen.height;
            //float halfScreenWidth = screenWidth / 2 / canvas.scaleFactor;
            //float halfScreenHeight = screenHeight / 2 / canvas.scaleFactor;
            //float finalPositionX = halfScreenWidth - (-1 * parentRectTransform.localPosition.x) - (-1 * uiRectTansform.localPosition.x);
            //float finalPositionY = halfScreenHeight - (-1 * parentRectTransform.localPosition.y) - (-1 * uiRectTansform.localPosition.y);
            //Vector2 finalPositionInfoPanel = new Vector2(finalPositionX + cardHalfSizeX, finalPositionY + cardHalfSizeY);

            // parentRectTransform == cardHolder
            float anchorMinX = parentRectTransform.anchorMin.x;
            float anchorMaxX = parentRectTransform.anchorMax.x;
            float anchorMinY = parentRectTransform.anchorMin.y;
            float anchorMaxY = parentRectTransform.anchorMax.y;

            // OBTENEMOS EL TAMANO ACTUAL DEL RECT DEL CANVAS
            float CanvasWidth = canvasTransform.rect.width;
            float CanvasHeight = canvasTransform.rect.height;

            float mostLeftPosition = anchorMinX * CanvasWidth;
            float mostRightPosition = anchorMaxX * CanvasWidth;
            float totalWidthRect = mostRightPosition - mostLeftPosition;
            float finalPositionXOfCardHolder = totalWidthRect / 2 + mostLeftPosition;

            float mostDownPosition = anchorMinY * CanvasHeight;
            float mostTopPosition = anchorMaxY * CanvasHeight;
            float totalHeightRect = mostTopPosition - mostDownPosition;
            float finalPositionYOfCardHolder = totalHeightRect / 2 + mostDownPosition;
            //Debug.Log("finalPositionOfCardHolder: " + finalPositionXOfCardHolder + "/" + finalPositionYOfCardHolder);

            Vector2 cardPrefabAnchorInLayout = new Vector2(mostLeftPosition, mostTopPosition);
            //Debug.Log("cardPrefabAnchorInLayout" + cardPrefabAnchorInLayout);

            float finalCardXPosition = cardPrefabAnchorInLayout.x + uiRectTansform.anchoredPosition.x + cardFrontRect.anchoredPosition.x;
            float finalCardYPosition = cardPrefabAnchorInLayout.y + uiRectTansform.anchoredPosition.y + cardFrontRect.anchoredPosition.y;
            Vector2 finalCardPosition = new Vector2(finalCardXPosition, finalCardYPosition);
            //Debug.Log("finalCardPosition" + finalCardPosition);

            onCardInformationShow?.Invoke(cardData, cardFrontRect.GetChild(0).GetComponent<RectTransform>(), finalCardPosition);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            positionTween.Kill();
            scaleTween.Kill();
            cardFrontRect.anchoredPosition = cardFrontStartPosition;
            uiRectTansform.localScale = initialScale;
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
                //Debug.Log("Hit " + result.gameObject.name);
                if (onTryUseCard != null)
                {
                    onTryUseCard?.Invoke(cardData, uiRectTansform);
                    parentRectTransform.GetChild(startSiblingIndex).SetAsLastSibling();
                }
                else
                {
                    //Debug.Log("CARD NOT CREATED BY CODE");
                }
                return;
            }
        }

        protected override void OnFaliedDrop()
        {
            //Debug.Log("NO CHOCO CONTRA NINGUNA UI");
            //Debug.Log("NO CHOCO CONTRA NINGUNA UI cardFrontStartPosition " + cardFrontStartPosition);
            cardFrontRect.anchoredPosition = cardFrontStartPosition;
            uiRectTansform.localScale = initialScale;
            uiRectTansform.SetParent(parentRectTransform);
            //canvasGroup.blocksRaycasts = true;
            if (holderAux != null)
            {
                uiRectTansform.SetSiblingIndex(holderAux.transform.GetSiblingIndex());
                Destroy(holderAux);
            }
            else
            {
                uiRectTansform.SetSiblingIndex(startSiblingIndex);
            }

            // SI LO PONGO ACA, NO SE EFECTUA EL ON POINTER EXIT... QUE SERIA LO IDEAL
            canvasGroup.blocksRaycasts = true;
            isPointerOverCard = false;
            onCardInfoPanelClose?.Invoke();
        }

        protected override void OnSuccesfulPointerExit()
        {
            isPointerOverCard = false;
            onCardInfoPanelClose?.Invoke();
            //Debug.Log("POINTER EXIT");
            cardFrontRect.DOAnchorPos(cardFrontStartPosition, 0.5f);
            uiRectTansform.DOScale(initialScale, 0.5f); // SetEase(ease)
        }
    }
}