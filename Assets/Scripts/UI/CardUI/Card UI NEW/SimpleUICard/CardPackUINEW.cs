using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

namespace PositionerDemo
{
    public class CardPackUINEW : SimpleDragDropUI
    {
        PackOpeningArea packOpeningArea;
        private bool allowedToOpen = false;
        public Image GlowImage;
        public Color32 GlowColor;
        private Vector3 initialScale;
        private float scaleFactor = 1.1f;

        public void SetCardPackOpeningArea(PackOpeningArea packOpeningArea)
        {
            this.packOpeningArea = packOpeningArea;
        }

        protected override void InitializeSimpleUI()
        {
            base.InitializeSimpleUI();
            canvasGroup = GetComponent<CanvasGroup>();
            initialScale = uiRectTansform.localScale;
        }

        protected override void OnSuccesfulPointerEnter()
        {
            if (allowedToOpen)
            {
                GlowImage.gameObject.SetActive(true);
                GlowImage.DOColor(GlowColor, 0.5f);
            }
            else
            {
                uiRectTansform.DOScale(initialScale * scaleFactor, 0.5f); // SetEase(ease)
            }
        }

        protected override void OnSuccesfulPointerExit()
        {
            // turn the glow Off
            if (allowedToOpen)
            {
                GlowImage.gameObject.SetActive(false);
                GlowImage.DOColor(Color.clear, 0.5f);
            }
            else
            {
                uiRectTansform.DOScale(initialScale, 0.5f); // SetEase(ease)
            }
        }

        protected override void OnSuccesfulBeginDrag(PointerEventData eventData)
        {
            if (packOpeningArea.AllowedToDragAPack == false)
            {
                eventData.pointerDrag = null;
                isSomethingDraggin = false;
                return;
            }
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);

            if (allowedToOpen)
            {
                // prevent from opening again
                GlowImage.gameObject.SetActive(false);
                allowedToOpen = false;
                packOpeningArea.OnCardPackOpen(uiRectTansform);
            }
        }

        protected override bool IsSuccesfulDropArea(RaycastResult result)
        {
            if (result.gameObject.CompareTag("OpenPackArea"))
            {
                //Debug.Log("Hit " + result.gameObject.name);
                return true;
            }

            return false;
        }

        protected override void OnFaliedDrop()
        {
            isSomethingDraggin = true;
            //Debug.Log("startAnchoredPosition " + startAnchoredPosition);
            uiRectTansform.SetParent(parentRectTransform);
            transform.DOLocalMove(startAnchoredPosition, 1f).OnComplete(() =>
            {
                packOpeningArea.AllowedToDragAPack = true;
                
                uiRectTansform.SetSiblingIndex(startSiblingIndex);
                isSomethingDraggin = false;
            });
        }

        protected override void OnSuccesfulDrop(RaycastResult result)
        {
            //Debug.Log("OnSuccesfulDrop ");
            isSomethingDraggin = true;
            packOpeningArea.AllowedToDragAPack = false;
            // Disable back button so that player can not exit the pack opening screen while he has not opened a pack
            packOpeningArea.BackButton.interactable = false;

            // snap the pack to the center of the pack opening area
            uiRectTansform.SetParent(packOpeningArea.packOpeningParent.transform);
            uiRectTansform.DOLocalMove(packOpeningArea.packOpeningParent.transform.position, 0.5f).OnComplete(() =>
            {
                // enable opening on click
                allowedToOpen = true;
                isSomethingDraggin = false;
            });
        }

    }

}
