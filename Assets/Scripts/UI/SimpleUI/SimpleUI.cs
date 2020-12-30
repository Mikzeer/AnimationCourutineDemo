using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PositionerDemo
{
    public abstract class SimpleUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        #region VARIABLES

        public static bool isSomethingDraggin = false; // NUNCA SE VAN A PODER DRAGEAR DOS CARDS A LA VEZ
        protected RectTransform uiRectTansform; // el Rect de esa UI que queremos DRAG/DROP
        protected RectTransform parentRectTransform; // el parent donde ese UI se va a anclar
        protected Canvas canvas; // Este canvas sirva para llevar al frente de todo siempre lo que drageamos
        protected CanvasScaler canvasScaler;
        #endregion

        #region UNITY METHODS

        public void Awake()
        {
            uiRectTansform = GetComponent<RectTransform>();
            if (transform.parent != null)
            {
                parentRectTransform = transform.parent.GetComponent<RectTransform>();
            }
            // Dos maneras de buscar el Canvas, la segunda es mejor cuando se tiene diferentes Canvas
            if (canvas == null)
            {
                //canvas = transform.root.GetComponent<Canvas>();
                Transform trsCanvas = transform.parent;
                while (trsCanvas != null)
                {
                    canvas = trsCanvas.GetComponent<Canvas>();
                    if (canvas != null)
                    {
                        canvasScaler = canvas.GetComponent<CanvasScaler>();
                        break;
                    }
                    trsCanvas = trsCanvas.parent;
                }
            }
            InitializeSimpleUI();
        }

        #endregion

        #region METHODS

        protected virtual void InitializeSimpleUI()
        {

        }

        protected virtual void OnSuccesfulPointerEnter()
        {

        }

        protected virtual void OnSuccesfulPointerExit()
        {

        }

        #endregion

        #region POINTER METHODS

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (isSomethingDraggin) return;

            OnSuccesfulPointerEnter();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (isSomethingDraggin) return;

            OnSuccesfulPointerExit();
        }

        #endregion
    }

}
