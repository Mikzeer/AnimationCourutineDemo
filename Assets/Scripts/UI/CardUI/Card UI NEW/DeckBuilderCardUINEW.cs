using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PositionerDemo
{
    public class DeckBuilderCardUINEW : SimpleDragDropUI
    {
        int ID;
        public event Action OnCardDropOnDeck;

        public void SetEvent(Action OnCardDropOnDeck, int ID)
        {
            this.OnCardDropOnDeck = OnCardDropOnDeck;
            this.ID = ID;
        }

        protected override void InitializeSimpleUI()
        {
            base.InitializeSimpleUI();
            canvasGroup = GetComponent<CanvasGroup>();
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
            Debug.Log("Hit " + result.gameObject.name);
            OnCardDropOnDeck?.Invoke();
        }

        protected override void OnFaliedDrop()
        {
            uiRectTansform.SetParent(parentRectTransform);
            uiRectTansform.SetSiblingIndex(startSiblingIndex);
            uiRectTansform.anchoredPosition = startAnchoredPosition;
            canvasGroup.blocksRaycasts = true;
        }
    }

}
