using UnityEngine;
using UnityEngine.UI;
using PositionerDemo;

namespace MikzeerGame
{
    public class CardDisplay : MonoBehaviour
    {
        private int id;
        [SerializeField] private Image cardImage;
        [SerializeField] private Text txtCardName;
        [SerializeField] private Text txtCardDescription;
        [SerializeField] private Text txtCardLevel;
        [SerializeField] private Image cardSprite;
        [SerializeField] private GameObject chainGameObject;
        [SerializeField] private GameObject automaticGameObject;
        [SerializeField] private Text txtDarkPoints;
        
        //[SerializeField] private Sprite cardTypeSprite;
        [SerializeField] private Image cardMiniatureSprite;

        public void Initialized(Card card)
        {
            id = card.IDInGame;
            SetDisplay(card.CardData);
        }

        public void SetDisplay(CardData cardData)
        {
            txtCardName.text = cardData.CardName;
            txtCardDescription.text = cardData.Description;

            if (txtCardLevel != null)
            {
                txtCardLevel.text = cardData.CardRarity.ToString();
            }

            if (txtDarkPoints != null)
            {
                txtDarkPoints.text = cardData.DarkPoints.ToString();
            }


            cardSprite.sprite = cardData.CardImage;
            switch (cardData.CardType)
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

            chainGameObject.SetActive(cardData.IsChainable);
            switch (cardData.ActivationType)
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

