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

        public void SetCardPackOpeningArea(PackOpeningArea packOpeningArea)
        {
            this.packOpeningArea = packOpeningArea;
        }

        public void AllowToOpenThisPack()
        {
            allowedToOpen = true;
            packOpeningArea.AllowedToDragAPack = false;
            // Disable back button so that player can not exit the pack opening screen while he has not opened a pack
            packOpeningArea.BackButton.interactable = false;
            //isSomethingDraggin = true;
        }

        protected override void InitializeSimpleUI()
        {
            base.InitializeSimpleUI();
            canvasGroup = GetComponent<CanvasGroup>();
        }

        protected override void OnSuccesfulPointerEnter()
        {
            if (allowedToOpen)
            {
                GlowImage.gameObject.SetActive(true);
                GlowImage.DOColor(GlowColor, 0.5f);
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
        }

        protected override void OnSuccesfulBeginDrag(PointerEventData eventData)
        {
            if (packOpeningArea.AllowedToDragAPack == false)
            {
                eventData.pointerDrag = null;
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
                packOpeningArea.OnPackCardOpen(uiRectTansform);
            }
        }

        protected override bool IsSuccesfulDropArea(RaycastResult result)
        {
            if (result.gameObject.CompareTag("OpenPackArea"))
            {
                Debug.Log("Hit " + result.gameObject.name);
                return true;
            }

            return false;
        }

        protected override void OnFaliedDrop()
        {
            Debug.Log("startAnchoredPosition " + startAnchoredPosition);
            uiRectTansform.SetParent(parentRectTransform);
            transform.DOLocalMove(startAnchoredPosition, 1f).OnComplete(() =>
            {
                packOpeningArea.AllowedToDragAPack = true;
                
                uiRectTansform.SetSiblingIndex(startSiblingIndex);
            });
        }

        protected override void OnSuccesfulDrop(RaycastResult result)
        {
            // snap the pack to the center of the pack opening area
            uiRectTansform.SetParent(packOpeningArea.packOpeningParent.transform);
            uiRectTansform.DOLocalMove(packOpeningArea.packOpeningParent.transform.position, 0.5f).OnComplete(() =>
            {
                // enable opening on click
                AllowToOpenThisPack();
            });
        }

    }

}
