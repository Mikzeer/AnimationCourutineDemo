using UnityEngine;
using UnityEngine.UI;
using PositionerDemo;

namespace MikzeerGame
{
    public class CardDisplay : MonoBehaviour
    {
        private int id;
        [SerializeField] private Image cardImage = default;
        [SerializeField] private Text txtCardName = default;
        [SerializeField] private Text txtCardDescription = default;
        [SerializeField] private Text txtCardLevel = default;
        [SerializeField] private Image cardSprite = default;
        [SerializeField] private GameObject chainGameObject = default;
        [SerializeField] private GameObject automaticGameObject = default;
        [SerializeField] private Text txtDarkPoints = default;
        
        //[SerializeField] private Sprite cardTypeSprite;
        //[SerializeField] private Image cardMiniatureSprite = default;

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

