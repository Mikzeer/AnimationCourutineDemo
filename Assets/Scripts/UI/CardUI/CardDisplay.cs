using UnityEngine;
using UnityEngine.UI;
using PositionerDemo;

namespace MikzeerGame
{
    public class CardDisplay : MonoBehaviour
    {
        private int id;
        [SerializeField] private Image cardImage;
        //[SerializeField] private Sprite cardTypeSprite;
        [SerializeField] private Text txtCardName;
        [SerializeField] private Text txtCardDescription;
        [SerializeField] private Text txtCardLevel;
        [SerializeField] private Image cardMiniatureSprite;
        [SerializeField] private Image cardSprite;
        [SerializeField] private GameObject chainGameObject;
        [SerializeField] private GameObject automaticGameObject;
        [SerializeField] private Text txtDarkPoints;

        public void Initialized(Card card)
        {
            id = card.ID;
            SetDisplay(card.CardSO);
        }

        public void SetDisplay(CardScriptableObject cardSO)
        {           
            txtCardName.text = cardSO.CardName;
            txtCardDescription.text = cardSO.Description;

            if (txtCardLevel != null)
            {
                txtCardLevel.text = cardSO.Level.ToString();
            }

            if (txtDarkPoints != null)
            {
                txtDarkPoints.text = cardSO.DarkPoints.ToString();
            }

            if (cardMiniatureSprite != null)
            {
                cardMiniatureSprite.sprite = cardSO.MiniatureImage;
            }

            cardSprite.sprite = cardSO.MiniatureImage;
            switch (cardSO.CardType)
            {
                case CARDTYPE.BUFF:
                    cardImage.color = Color.green;
                    break;
                case CARDTYPE.NERF:
                    cardImage.color = Color.red;
                    break;
                case CARDTYPE.NEUTRAL:
                    break;
                default:
                    break;
            }

            chainGameObject.SetActive(cardSO.IsChainable);
            switch (cardSO.ActivationType)
            {
                case ACTIVATIONTYPE.HAND:
                    automaticGameObject.SetActive(false);
                    break;
                case ACTIVATIONTYPE.AUTOMATIC:
                    automaticGameObject.SetActive(true);
                    break;
                default:
                    break;
            }
        }

    }
}

