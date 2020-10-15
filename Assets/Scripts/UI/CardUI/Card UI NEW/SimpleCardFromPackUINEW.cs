using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

namespace PositionerDemo
{
    public class SimpleCardFromPackUINEW : SimpleClickeableUI
    {
        public Image GlowImage;
        private Color32 GlowColor;
        PackOpeningArea packOpeningArea;
        private float InitialScale;
        private float scaleFactor = 1.1f;
        private bool turnedOver = false;

        public void SetSimpleCardFromPackUI(Color32 GlowColor, PackOpeningArea packOpeningArea)
        {
            this.GlowColor = GlowColor;
            this.packOpeningArea = packOpeningArea;
        }

        protected override void InitializeSimpleUI()
        {
            InitialScale = uiRectTansform.localScale.x;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (turnedOver)
                return;

            turnedOver = true;
            // turn the card over
            transform.DORotate(Vector3.zero, 0.5f);
            // add this card to collection as unlocked
            packOpeningArea.NumberOfCardsOpenedFromPack++;
        }

        protected override void OnSuccesfulPointerEnter()
        {
            uiRectTansform.DOScale(InitialScale * scaleFactor, 0.5f);
            GlowImage.DOColor(GlowColor, 0.5f);
        }

        protected override void OnSuccesfulPointerExit()
        {
            uiRectTansform.DOScale(InitialScale, 0.5f);
            GlowImage.DOColor(Color.clear, 0.5f);
        }
    }

}
