using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

namespace PositionerDemo
{
    public class SimpleCardFromPackUINEW : SimpleClickeableUI
    {
        public Image GlowImage;
        public GameObject cardFront;
        public GameObject cardBack;

        private Color32 GlowColor;
        PackOpeningArea packOpeningArea;
        private Vector3 initialScale;
        private float scaleFactor = 1.1f;
        private bool turnedOver = false;
          
        public void SetSimpleCardFromPackUI(Color32 GlowColor, PackOpeningArea packOpeningArea)
        {
            //Debug.Log("POINTER DOWN");
            this.GlowColor = GlowColor;
            this.packOpeningArea = packOpeningArea;
        }

        protected override void InitializeSimpleUI()
        {
            initialScale = uiRectTansform.localScale;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (turnedOver)
                return;

            turnedOver = true;
            GlowImage.DOColor(Color.clear, 0.5f);
            // turn the card over
            transform.DORotate(new Vector3(0,90,0), 0.5f).OnComplete(() =>
            {
                cardBack.SetActive(false);
                cardFront.SetActive(true);
                Vector3 changeRot = new Vector3(0, -90, 0);
                cardFront.transform.rotation = Quaternion.Euler(changeRot);
                cardFront.transform.DORotate(new Vector3(0, 0, 0), 0.5f);
                // add this card to collection as unlocked
                packOpeningArea.NumberOfCardsOpenedFromPack++;
            });

        }

        protected override void OnSuccesfulPointerEnter()
        {
            uiRectTansform.DOScale(initialScale * scaleFactor, 0.5f); // SetEase(ease)
            GlowImage.gameObject.SetActive(true);
            GlowImage.DOColor(GlowColor, 0.5f);
        }

        protected override void OnSuccesfulPointerExit()
        {
            uiRectTansform.DOScale(initialScale, 0.5f); // SetEase(ease)
            GlowImage.gameObject.SetActive(false);
            GlowImage.DOColor(Color.clear, 0.5f);
        }
    }

}
