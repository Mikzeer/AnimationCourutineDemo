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

        public Player(int PlayerID, PLAYERTYPE playerType)
        {
            this.PlayerID = PlayerID;
            this.playerType = playerType;
            OccupierType = OCUPPIERTYPE.PLAYER;
            CardTargetType = CARDTARGETTYPE.BASENEXO;
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
