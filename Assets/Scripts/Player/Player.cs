using System.Collections.Generic;
using UnityEngine;

namespace PositionerDemo
{
    public class Player : IOcuppy
    {
        private Animator playerAnimator;
        private Transform playerTransform;

        public int PosX { get; private set; }
        public int PosY { get; private set; }

        private int _playerID;
        public int PlayerID { get => _playerID; private set => _playerID = value; }

        private OCUPPIERTYPE _occupierType;
        public OCUPPIERTYPE OccupierType { get => _occupierType; protected set => _occupierType = value; }

        private CARDTARGETTYPE _cardTargetType;
        public CARDTARGETTYPE CardTargetType { get => _cardTargetType; protected set => _cardTargetType = value; }

        public PLAYERTYPE playerType { get; set; }

        private List<Kimboko.Kimboko> kimbokoUnits = new List<Kimboko.Kimboko>();

        private int _actionPoints;
        public int ActionPoints { get { return _actionPoints; } private set { _actionPoints = value; } }

        private Stack<Card> _deck;
        public Stack<Card> Deck { get { return _deck; } set { _deck = value; } }
        private List<Card> _playersHands;
        public List<Card> PlayersHands { get { return _playersHands; } private set { _playersHands = value; } }
        private List<Card> _graveyard;
        public List<Card> Graveyard { get { return _graveyard; } private set { _graveyard = value; } }

        public Player(int PlayerID, PLAYERTYPE playerType, Stack<Card> Deck)
        {
            this.Deck = Deck;
            this.PlayerID = PlayerID;
            this.playerType = playerType;
            OccupierType = OCUPPIERTYPE.PLAYER;
            CardTargetType = CARDTARGETTYPE.BASENEXO;
            PlayersHands = new List<Card>();
            Graveyard = new List<Card>();
        }

        public void OnSelect(bool isSelected, int playerID)
        {
            Debug.Log("PLAYER SELECTED");
        }

        public void SetGameObject(GameObject playerPrefab)
        {
            playerAnimator = playerPrefab.GetComponent<Animator>();
            playerTransform = playerPrefab.transform;
        }

        public Transform GetTransform()
        {
            return playerTransform;
        }

        public Animator GetAnimator()
        {
            return playerAnimator;
        }

        public void AddUnit(Kimboko.Kimboko kimbokoUnit)
        {
            kimbokoUnits.Add(kimbokoUnit);
        }

        public void RemoveUnit(Kimboko.Kimboko kimbokoUnit)
        {
            kimbokoUnits.Remove(kimbokoUnit);
        }

        public void SetActionPoints(int apPoints)
        {
            ActionPoints = apPoints;
        }

        public void RestActionPoints(int apPoints)
        {
            ActionPoints -= apPoints;
        }

    }
}
