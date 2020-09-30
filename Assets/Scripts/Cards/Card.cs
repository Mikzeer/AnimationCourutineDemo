using System;
using System.Collections.Generic;
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
        private bool isChainable;
        public bool IsChainable { get { return isChainable; } protected set { isChainable = value; } }
        private CARDTYPE cardType;
        public CARDTYPE CardType { get { return cardType; } protected set { cardType = value; } }
        private ACTIVATIONTYPE activationType;
        public ACTIVATIONTYPE ActivationType { get { return activationType; } protected set { activationType = value; } }
        private List<CARDTARGETTYPE> posibleTargets;
        public List<CARDTARGETTYPE> PosibleTargets { get { return posibleTargets; } protected set { posibleTargets = value; } }

        // ACTION MODIFIERS
        // STATS MODIFIERS
        // ACTIVATION CONDITION
        // PERMANENCIA DE EFECTO

        private CardScriptableObject _cardSO;
        public CardScriptableObject CardSO { get { return _cardSO; } protected set { _cardSO = value; } }

        public Card(int ID, Player ownerPlayer, CardScriptableObject CardSO)
        {
            this.ID = ID;
            this.ownerPlayer = ownerPlayer;
            this.CardSO = CardSO;

            OnDrop += OnDropCard;

            isChainable = CardSO.IsChainable;
            cardType = CardSO.CardType;
            activationType = CardSO.ActivationType;
            posibleTargets = CardSO.PosibleTargets;           
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