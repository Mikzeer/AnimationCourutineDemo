using System;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using DG.Tweening;

namespace PositionerDemo
{
    public class CardInGameUINEW : SimpleDragDropUI
    {
        [SerializeField] private GameObject placeholderPrefab;
        private GameObject holderAux;
        bool isPointerOverCard = false;
        float timeToReach = 1.2f;
        float actualTimeOver = 0f;
        private Action<CardInGameUINEW> onCardUse;
        private Action<string, Vector2> onCardInfo;
        private Action<bool> onCardEnter;
        private Action<int> onCardDrop;
        public int ID;

        private Vector3 initialScale;
        private float scaleFactor = 1.1f;

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
            initialScale = uiRectTansform.localScale;
            canvasGroup = GetComponent<CanvasGroup>();
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

            //parentRectTransform.GetChild(parentRectTransform.childCount - 1).SetSiblingIndex(startSiblingIndex);
            ////uiRectTansform.SetParent(parentRectTransform.parent.parent.transform);
            //uiRectTansform.SetParent(canvas.transform);

            //Vector2 localPoint;
            //RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, uiRectTansform.anchoredPosition, Camera.main, out localPoint);
            //Debug.Log("localPoint " + localPoint);

            uiRectTansform.DOScale(initialScale * scaleFactor, 0.5f); // SetEase(ease)
            //uiRectTansform.DOAnchorPos(startAnchoredPosition * new Vector2(0, 3), 0.5f);
            //Vector2 offset = new Vector2(80, 0);
            //uiRectTansform.anchoredPosition += offset;


            //float cardHalfSizeY = uiRectTansform.rect.size.y / 2;
            //float panelSizeY = parentRectTransform.rect.size.y;
            //float canvasResoY = canvasScaler.referenceResolution.y;
            //float newPosY = canvasResoY - panelSizeY - (cardHalfSizeY);

            //Vector2 mousePosition = Input.mousePosition / canvas.scaleFactor;
            //Vector3 locPoint = new Vector3(mousePosition.x, cardHalfSizeY, uiRectTansform.localPosition.z);
            //Vector3 clampedPostion = Helper.KeepRectInsideScreen(uiRectTansform, locPoint, canvasScaler);
            //uiRectTansform.anchoredPosition = clampedPostion;
        }

        IEnumerator mouseOverToShowInfoPanel(Vector3 cardPosition)
        {
            Debug.Log("inicio coroutine");
            actualTimeOver = 0;

            while (actualTimeOver < timeToReach)
            {
                if (isPointerOverCard == false)
                {
                    Debug.Log("pointer not over a card");
                    yield break;
                }
                actualTimeOver += Time.deltaTime;
                yield return null;
            }

            if (isSomethingDraggin)
            {
                Debug.Log("something draging");
                yield break;
            }

            Debug.Log("llego a la coroutine del show panel info");

            actualTimeOver = 0;
            // aca tenemos que hacer el InfoPanel haga el SetText(hola soy la cartita)
            onCardEnter?.Invoke(true);
            Vector2 cardRectInScreenPosition = Camera.main.WorldToScreenPoint(uiRectTansform.position);
            cardRectInScreenPosition += new Vector2(uiRectTansform.sizeDelta.x / 2 * canvas.scaleFactor, 0);
            onCardInfo?.Invoke("CARD NAME: " + name, cardRectInScreenPosition / canvas.scaleFactor);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            uiRectTansform.DOScale(initialScale, 0.5f); // SetEase(ease)
        }

        protected override void OnSuccesfulBeginDrag(PointerEventData eventData)
        {
            onCardEnter?.Invoke(false);
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
                if (onCardUse != null)
                {
                    onCardUse?.Invoke(this);
                    onCardDrop?.Invoke(ID);
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
            onCardEnter?.Invoke(false);

            Debug.Log("POINTER EXIT");
            uiRectTansform.DOScale(initialScale, 0.5f); // SetEase(ease)
            //uiRectTansform.DOAnchorPos(startAnchoredPosition, 0.5f);
        }

    }
}