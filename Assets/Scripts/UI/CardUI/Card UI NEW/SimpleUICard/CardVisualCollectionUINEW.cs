using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PositionerDemo
{
    public class CardVisualCollectionUINEW : SimpleClickeableUI
    {
        protected CardSlotUI cardSlotUI;

        public void SetSlot(CardSlotUI cardSlotUI)
        {
            this.cardSlotUI = cardSlotUI;
        }

        protected override void InitializeSimpleUI()
        {
            base.InitializeSimpleUI();
        }

        protected override void OnSuccesfulPointerEnter()
        {
            cardSlotUI.ShowLibraryAmount();
        }

        protected override void OnSuccesfulPointerExit()
        {
            cardSlotUI.HideLibraryAmount();
        }
    }
}
