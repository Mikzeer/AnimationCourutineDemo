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
        int siblingIndex;
        bool isDragin = false;
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

        int indexBeforeEnter = 0;
        bool isPointerOverCard = false;
        float timeToReach = 1.2f;
        float actualTimeOver = 0f;
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (DragUIController.isDragin)
            {
                return;
            }

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

        public void OnPointerDown(PointerEventData eventData)
        {
            //Debug.Log("OnPointerDown ");
            //siblingIndex = cardRectTansform.GetSiblingIndex();
            siblingIndex = indexBeforeEnter;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (DragUIController.isDragin)
            {
                return;
            }

            cardRectTansform.SetParent(canvas.transform);
            cardRectTansform.SetAsLastSibling();
            //canvasGroup.blocksRaycasts = false;
            onCardEnter?.Invoke(false);

            DragUIController.isDragin = true;
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
            DragUIController.isDragin = false;
            List<RaycastResult> results = new List<RaycastResult>(); //Create a list of Raycast Results           
            m_Raycaster.Raycast(eventData, results); //Raycast using the Graphics Raycaster and mouse click position

            if (results.Count == 0)
            {
                // ESTO SIGNIFICA QUE NO CHOCO CONTRA NINGUN UI
                Debug.Log("NO CHOCO CONTRA NINGUNA UI");

                cardRectTansform.SetParent(playerHandTransform);
                //esto agregue
                playerHandTransform.GetChild(indexBeforeEnter).SetAsLastSibling();

                //cardRectTansform.SetSiblingIndex(playerHandTransform.childCount - 2);
                cardRectTansform.SetSiblingIndex(indexBeforeEnter);
                canvasGroup.blocksRaycasts = true;
                isDragin = false;
                return;
            }
            else
            {
                //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
                foreach (RaycastResult result in results)
                {
                    if (result.gameObject.CompareTag("DropeableArea"))
                    {
                        Debug.Log("Hit " + result.gameObject.name);

                        if (onCardUse != null)
                        {
                            isComingFromDrag = true;
                            onCardUse.Invoke(this);
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
            //esto agregue
            playerHandTransform.GetChild(indexBeforeEnter).SetAsLastSibling();
            cardRectTansform.SetSiblingIndex(indexBeforeEnter);
            canvasGroup.blocksRaycasts = true;
            isDragin = false;
        }


        bool isComingFromDrag = false;

        public void OnPointerExit(PointerEventData eventData)
        {
            if (DragUIController.isDragin)
            {
                return;
            }

            Debug.Log("OnPointerExit, VOLVER A LA CARTA SU TAMANO Y POSICION ORIGINAL");
            onCardEnter?.Invoke(false);

            if (isComingFromDrag)
            {
                isComingFromDrag = false;
                isPointerOverCard = false;
                return;
            }

            if (isDragin == false)
            {
                Debug.Log("ISDRAGIN IS FALSE");
                isPointerOverCard = false;
                playerHandTransform.GetChild(indexBeforeEnter).SetAsLastSibling();
                cardRectTansform.SetParent(playerHandTransform);
                cardRectTansform.SetSiblingIndex(indexBeforeEnter);
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
            Vector2 cardRectInScreenPosition = Camera.main.WorldToScreenPoint(cardPosition);
            //Vector2 cardRectInScreenPosition = Camera.main.WorldToScreenPoint(cardPosition);
            //cardRectInScreenPosition += new Vector2(65, 0);
            Debug.Log("cardRectTansform.sizeDelta" + cardRectTansform.sizeDelta);
            cardRectInScreenPosition *= new Vector2(1.5f, 1);
            onCardInfo?.Invoke("SOY LA CARD, mi nombre es " + name, cardRectInScreenPosition / canvas.scaleFactor);
        }

        private Action<CardUI> onCardUse;
        private Action<string, Vector2> onCardInfo;
        private Action<bool> onCardEnter;

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

    }

    public static class DragUIController
    {
        public static bool isDragin = false;
    }
}
