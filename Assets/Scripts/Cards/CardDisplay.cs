using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MikzeerGame
{
    public class CardDisplay : MonoBehaviour
    {
        public Text txtTitle;
        public Text txtDescription;
        public GameObject chainGo;
        public GameObject AutoGo;
        Animator cardAnimator;

        public Sprite miniatureSprite;

        private void Awake()
        {
            cardAnimator = GetComponent<Animator>();
        }

        //public void Initialized(Card card)
        //{
        //    card.SetAnimator(cardAnimator);
        //    card.cardPrefab = this;
        //    this.name = "Card: " + card.Name;

        //    SetDisplay(card.Name, card.Description, card.MiniatureImage, false, false);
        //}

        private void SetDisplay(string title, string description, Sprite miniatureSprite, bool chain, bool auto)
        {
            this.miniatureSprite = miniatureSprite;

            txtTitle.text = title;
            txtDescription.text = description;
            chainGo.SetActive(chain);
            AutoGo.SetActive(auto);
        }

    }
}

