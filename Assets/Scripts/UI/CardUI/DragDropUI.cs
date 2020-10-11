using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PositionerDemo
{
    public class DragDropUI : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IPointerEnterHandler, IPointerExitHandler, IEndDragHandler, IInitializePotentialDragHandler
    {

        #region VARIABLES

        public static bool isSomethingDraggin = false; // NUNCA SE VAN A PODER DRAGEAR DOS CARDS A LA VEZ
        private Canvas canvas; // el root Canvas con el que empieza
        private RectTransform uiRectTansform; // el Rect de esa UI que queremos DRAG/DROP
        private Transform parentRectTransform; // el parent donde ese UI se va a anclar
        private GraphicRaycaster m_Raycaster;
        List<RaycastResult> results = new List<RaycastResult>();
        private CanvasGroup canvasGroup;
        bool isDragin = false;
        int indexBeforeEnter = 0;
        bool isPointerOverCard = false;
        bool isComingFromDrag = false;

        Vector2 startAnchoredPosition;

        #endregion

        #region EVENTS

        public event Action OnCardDropOnDeck;

        #endregion

        #region UNITY METHODS

        public void Awake()
        {
            uiRectTansform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            if (transform.parent != null)
            {
                parentRectTransform = transform.parent.GetComponent<Transform>();
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

        #endregion

        #region POINTER METHODS

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (isSomethingDraggin) return;

            if (isDragin == false)
            {
                isPointerOverCard = true;
                indexBeforeEnter = uiRectTansform.GetSiblingIndex();
                startAnchoredPosition = uiRectTansform.anchoredPosition;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (isSomethingDraggin) return;

            uiRectTansform.SetParent(canvas.transform);
            uiRectTansform.SetAsLastSibling();
            isSomethingDraggin = true;
            isDragin = true;
        }

        public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            eventData.useDragThreshold = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            // eventData.delta = the amount the mouse move since the last frame
            uiRectTansform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = false;
            isSomethingDraggin = false;

            results.Clear();
            m_Raycaster.Raycast(eventData, results);

            if (results.Count == 0)
            {
                Debug.Log("NO CHOCO CONTRA NINGUNA UI");
                uiRectTansform.SetParent(parentRectTransform);
                uiRectTansform.SetSiblingIndex(indexBeforeEnter);
                canvasGroup.blocksRaycasts = true;

                uiRectTansform.anchoredPosition = startAnchoredPosition;
                return;
            }
            else
            {
                foreach (RaycastResult result in results)
                {
                    if (result.gameObject.CompareTag("DropeableArea"))
                    {
                        Debug.Log("Hit " + result.gameObject.name);
                        isComingFromDrag = true;
                        isDragin = false;
                        OnCardDropOnDeck?.Invoke();
                        return;
                    }
                    else if (result.gameObject.CompareTag("Card"))
                    {
                        Debug.Log("Hit " + result.gameObject.name);
                        uiRectTansform.SetParent(parentRectTransform);
                        uiRectTansform.SetSiblingIndex(indexBeforeEnter);
                        canvasGroup.blocksRaycasts = true;

                        uiRectTansform.anchoredPosition = startAnchoredPosition;
                        return;
                    }
                }
            }

            uiRectTansform.SetParent(parentRectTransform);
            uiRectTansform.SetSiblingIndex(indexBeforeEnter);
            isDragin = false;
            canvasGroup.blocksRaycasts = true;

        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (isSomethingDraggin)
            {
                return;
            }

            if (isComingFromDrag)
            {
                isComingFromDrag = false;
                isPointerOverCard = false;
                return;
            }

            if (isDragin == false)
            {
                isPointerOverCard = false;
                uiRectTansform.SetParent(parentRectTransform);
                uiRectTansform.SetSiblingIndex(indexBeforeEnter);
            }
        }

        #endregion

        int ID;

        public void SetEvent(Action OnCardDropOnDeck, int ID)
        {
            this.OnCardDropOnDeck = OnCardDropOnDeck;
            this.ID = ID;
        }

    }
}
