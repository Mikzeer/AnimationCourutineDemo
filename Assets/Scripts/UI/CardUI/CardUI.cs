using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MikzeerGame
{
    public class CardUI : MonoBehaviour, IPointerDownHandler, IDragHandler,IBeginDragHandler, IPointerEnterHandler, IPointerExitHandler, IEndDragHandler, IInitializePotentialDragHandler
    {
        private Canvas canvas;
        private RectTransform cardRectTansform;
        private Transform playerHandTransform;
        private GraphicRaycaster m_Raycaster;
        private CanvasGroup canvasGroup;
        //int siblingIndex;
        bool isDragin = false;
        public static bool isACardDraging = false;
        int indexBeforeEnter = 0;
        bool isPointerOverCard = false;
        float timeToReach = 1.2f;
        float actualTimeOver = 0f;
        bool isComingFromDrag = false;

        private Action<CardUI> onCardUse;
        private Action<string, Vector2> onCardInfo;
        private Action<bool> onCardEnter;
        private Action<int> onCardDrop;
        public int ID;

        public void Awake()
        {
            cardRectTansform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            if (transform.parent != null)
            {
                playerHandTransform = transform.parent.GetComponent<Transform>();
            }
            m_Raycaster = FindObjectOfType<GraphicRaycaster>();
            // Dos maneras de buscar el Canvas, la segunda es mejor cuando se tiene diferentes Canvas
            //canvas = transform.root.GetComponent<Canvas>();
            if (canvas == null)
            {
                Transform trsCanvas = transform.parent;
                while (trsCanvas != null)
                {
                    canvas = trsCanvas.GetComponent<Canvas>();
                    if (canvas != null)
                    {
                        break;
                    }

                    trsCanvas = trsCanvas.parent;
                }
            }
        }

        public void SetRealCardDrop(Action<int> onCardDrop, int ID)
        {
            this.onCardDrop = onCardDrop;
            this.ID = ID;
        }

        public void SetPlayerHandTransform(Transform playerHandTransform, Action<CardUI> OnCardUse, Action<string, Vector2> onCardInfo, Action<bool> onCardEnter)
        {
            this.playerHandTransform = playerHandTransform;
            onCardUse = OnCardUse;
            this.onCardInfo = onCardInfo;
            this.onCardEnter = onCardEnter;
        }

        public void CanvasGroupRaycast(bool block)
        {
            canvasGroup.blocksRaycasts = block;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (isACardDraging) return;

            if (isDragin == false)
            {
                isPointerOverCard = true;
                StartCoroutine(mouseOverToShowInfoPanel(cardRectTansform.position));
                indexBeforeEnter = cardRectTansform.GetSiblingIndex();
                playerHandTransform.GetChild(playerHandTransform.childCount - 1).SetSiblingIndex(indexBeforeEnter);
                cardRectTansform.SetParent(playerHandTransform.parent.parent.transform);
                Vector2 offset = new Vector2(80,0);
                cardRectTansform.anchoredPosition += offset;
            }
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

            if (isDragin)
            {
                yield break;
            }

            actualTimeOver = 0;
            // aca tenemos que hacer el InfoPanel haga el SetText(hola soy la cartita)
            onCardEnter?.Invoke(true);
            Vector2 cardRectInScreenPosition = Camera.main.WorldToScreenPoint(cardRectTansform.position);
            cardRectInScreenPosition += new Vector2(cardRectTansform.sizeDelta.x / 2 * canvas.scaleFactor, 0);
            onCardInfo?.Invoke("SOY LA CARD, mi nombre es " + name, cardRectInScreenPosition / canvas.scaleFactor);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            //siblingIndex = cardRectTansform.GetSiblingIndex();
            //siblingIndex = indexBeforeEnter;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (isACardDraging)
            {
                return;
            }

            cardRectTansform.SetParent(canvas.transform);
            cardRectTansform.SetAsLastSibling();
            //canvasGroup.blocksRaycasts = false;
            onCardEnter?.Invoke(false);
            isACardDraging = true;
            isDragin = true;
        }

        public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            eventData.useDragThreshold = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            //Debug.Log("OnDrag");
            // eventData.delta = the amount the mouse move since the last frame
            cardRectTansform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = false;

            isACardDraging = false;

            List<RaycastResult> results = new List<RaycastResult>();       
            m_Raycaster.Raycast(eventData, results);

            if (results.Count == 0)
            {
                Debug.Log("NO CHOCO CONTRA NINGUNA UI");
                cardRectTansform.SetParent(playerHandTransform);
                playerHandTransform.GetChild(indexBeforeEnter).SetAsLastSibling();
                cardRectTansform.SetSiblingIndex(indexBeforeEnter);
                canvasGroup.blocksRaycasts = true;
                isDragin = false;
                return;
            }
            else
            {
                foreach (RaycastResult result in results)
                {
                    if (result.gameObject.CompareTag("DropeableArea"))
                    {
                        Debug.Log("Hit " + result.gameObject.name);

                        if (onCardUse != null)
                        {
                            isComingFromDrag = true;
                            onCardUse.Invoke(this);
                            onCardDrop?.Invoke(ID);
                            playerHandTransform.GetChild(indexBeforeEnter).SetAsLastSibling();
                        }
                        else
                        {
                            Debug.Log("CARD NOT CREATED BY CODE");
                        }
                        isDragin = false;
                        return;
                    }

                    if (result.gameObject.CompareTag("Card"))
                    {
                        Debug.Log("CARD");
                        int newSiblingIndex = result.gameObject.transform.GetSiblingIndex();
                        playerHandTransform.GetChild(indexBeforeEnter).SetAsLastSibling();
                        cardRectTansform.SetParent(playerHandTransform);
                        cardRectTansform.SetSiblingIndex(newSiblingIndex);
                        canvasGroup.blocksRaycasts = true;
                        isDragin = false;
                        isComingFromDrag = true;
                        return;
                    }
                }
            }

            cardRectTansform.SetParent(playerHandTransform);
            playerHandTransform.GetChild(indexBeforeEnter).SetAsLastSibling();
            cardRectTansform.SetSiblingIndex(indexBeforeEnter);
            canvasGroup.blocksRaycasts = true;
            isDragin = false;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (isACardDraging)
            {
                return;
            }
            //Debug.Log("OnPointerExit, VOLVER A LA CARTA SU TAMANO Y POSICION ORIGINAL");
            onCardEnter?.Invoke(false);

            if (isComingFromDrag)
            {
                isComingFromDrag = false;
                isPointerOverCard = false;
                return;
            }

            if (isDragin == false)
            {
                //Debug.Log("ISDRAGIN IS FALSE");
                isPointerOverCard = false;
                playerHandTransform.GetChild(indexBeforeEnter).SetAsLastSibling();
                cardRectTansform.SetParent(playerHandTransform);
                cardRectTansform.SetSiblingIndex(indexBeforeEnter);
            }
        }

    }
}


