using System;
using UnityEngine;

namespace PositionerDemo
{
    public abstract class Card
    {
        public Player ownerPlayer { get; protected set; }
        protected Animator cardAnimator;
        protected Transform cardTransform;
        protected RectTransform cardRectTransform;
        private int _id;
        public int ID { get { return _id; } protected set { _id = value; } }
        public Action OnDrop;

        // ACTION MODIFIERS
        // STATS MODIFIERS
        // ACTIVATION CONDITION
        // PERMANENCIA DE EFECTO

        public Card(int ID, Player ownerPlayer)
        {
            this.ID = ID;
            this.ownerPlayer = ownerPlayer;
            OnDrop += OnDropCard;
        }

        public virtual bool DoIHaveTarget()
        {
            // DEBERIA TENER UN METODO PARA VER SI SE PUEDE USAR O NO LA CARD, ASI CUANDO EL JUGADOR LA SUELTA EN EL CAMPO 
            // ESTA VERIFICA SI TIENE ALGUN TARGET, Y DE SER ASI NOS DEJA ELEGIR EL TARGET O SE EJECUTA AUTOMATICAMENTE SI NO REQUIERE TARGET
            return true;
        }

        public virtual void ApllyCard()
        {
            // para apply la card ya deberiamos haber tenido un target
        }

        public virtual void OnDropCard()
        {
            Debug.Log("I have been droped ");
        }

        public void SetGameObject(GameObject cardPrefab)
        {
            cardAnimator = cardPrefab.GetComponent<Animator>();
            cardRectTransform = cardPrefab.GetComponent<RectTransform>();
            cardTransform = cardPrefab.transform;
        }

        public Transform GetTransform()
        {
            return cardTransform;
        }

        public RectTransform GetRectTransform()
        {
            return cardRectTransform;
        }

        public Animator GetAnimator()
        {
            return cardAnimator;
        }

    }
}