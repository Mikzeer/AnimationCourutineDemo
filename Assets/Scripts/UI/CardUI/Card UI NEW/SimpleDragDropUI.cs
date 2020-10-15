using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PositionerDemo
{
    public abstract class SimpleDragDropUI : SimpleClickeableUI, IDragHandler, IBeginDragHandler, IEndDragHandler, IInitializePotentialDragHandler
    {
        #region VARIABLES
        protected CanvasGroup canvasGroup;
        protected GraphicRaycaster m_Raycaster;
        protected List<RaycastResult> results = new List<RaycastResult>(); // lista de resultados con los que chocamos luego de dropear la card
        protected int startSiblingIndex = 0; // el sibling index antes de comenzar a arrastrar la card
        protected Vector2 startAnchoredPosition; // la posicion en la cual estaba anclada la card antes de comenzar a dragearla

        bool succesfullDrop = false; // Esto lo podriamos llegar a utilizar para una card que queremos que despues del drop no sea utilizable
        
        #endregion

        #region METHODS

        protected override void InitializeSimpleUI()
        {
            m_Raycaster = FindObjectOfType<GraphicRaycaster>();
        }

        protected virtual void OnSuccesfulBeginDrag()
        {
        }

        protected virtual void OnSuccesfulDrop(RaycastResult result)
        {

        }

        protected virtual bool IsSuccesfulDropArea(RaycastResult result)
        {
            return true;
        }

        protected virtual void OnFaliedDrop()
        {
            uiRectTansform.SetParent(parentRectTransform);
            uiRectTansform.SetSiblingIndex(startSiblingIndex);
            uiRectTansform.anchoredPosition = startAnchoredPosition;
        }

        #endregion

        #region POINTER METHODS

        public override void OnPointerDown(PointerEventData eventData)
        {
            startSiblingIndex = uiRectTansform.GetSiblingIndex();
            startAnchoredPosition = uiRectTansform.anchoredPosition;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (isSomethingDraggin) return;
            uiRectTansform.SetParent(canvas.transform);
            uiRectTansform.SetAsLastSibling();
            isSomethingDraggin = true;

            OnSuccesfulBeginDrag();
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
                OnFaliedDrop();
                return;
            }
            else
            {
                foreach (RaycastResult result in results)
                {
                    if (IsSuccesfulDropArea(result))
                    {
                        succesfullDrop = true;
                        OnSuccesfulDrop(result);
                        return;
                    }
                }
            }
            // Si aun habiendo chocado con una UI, ninguna es la buscada, entonces es un fail drop
            OnFaliedDrop();
        }

        #endregion
    }

}
