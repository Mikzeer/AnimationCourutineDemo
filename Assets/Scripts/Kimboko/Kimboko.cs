using System.Collections.Generic;
using UnityEngine;


namespace PositionerDemo
{

    public abstract class Kimboko : IOcuppy
    {
        #region VARIABLES

        private int _id;
        public int ID { get => _id; protected set => _id = value; }
        public Player ownerPlayer { get; set; }

        protected Animator kimbokoAnimator;
        protected Transform kimbokoTransform;

        bool isAlly;
        public bool IsAlly { get => isAlly; protected set => isAlly = value; }

        Dictionary<int, Stat> stats;
        public Dictionary<int, Stat> Stats { get => stats; protected set => stats = value; }
        Dictionary<int, AbilityAction> abilities;
        public Dictionary<int, AbilityAction> Abilities { get => abilities; protected set => abilities = value; }
        private OCUPPIERTYPE _occupierType;
        public OCUPPIERTYPE OccupierType { get => _occupierType; protected set => _occupierType = value; }
        private CARDTARGETTYPE _cardTargetType;
        public CARDTARGETTYPE CardTargetType { get => _cardTargetType; protected set => _cardTargetType = value; }

        private UNITTYPE _unitType;
        public UNITTYPE UnitType { get => _unitType; protected set => _unitType = value; }
        private MOVEDIRECTIONTYPE _moveDirectionerType;
        public MOVEDIRECTIONTYPE MoveDirectionerType { get => _moveDirectionerType; protected set => _moveDirectionerType = value; }

        #endregion

        public Kimboko(int ID, Player ownerPlayer, UNITTYPE UnitType, MOVEDIRECTIONTYPE MoveDirectionerType)
        {
            this.ID = ID;
            this.ownerPlayer = ownerPlayer;
            CardTargetType = CARDTARGETTYPE.UNIT;
            OccupierType = OCUPPIERTYPE.UNIT;

            this.UnitType = UnitType;
            this.MoveDirectionerType = MoveDirectionerType;
            TurnManager.OnUnitResetActionPoints += ResetActionPoints;
        }

        public void OnSelect(bool isSelected, int playerID)
        {
            Debug.Log("KIMBOKO SELECTED");
        }

        public void SetGameObject(GameObject kimbokoPrefab)
        {
            kimbokoAnimator = kimbokoPrefab.GetComponent<Animator>();
            kimbokoTransform = kimbokoPrefab.transform;
        }

        public Transform GetTransform()
        {
            return kimbokoTransform;
        }

        public Animator GetAnimator()
        {
            return kimbokoAnimator;
        }

        public void DestroyPrefab()
        {
            ownerPlayer.RemoveUnit(this);
            MonoBehaviour.Destroy(kimbokoTransform.gameObject);
        }

        public int GetCurrentActionPoints()
        {
            int actionPoints = 0;

            if (stats.ContainsKey(4))
            {
                actionPoints = stats[4].ActualStatValue;
            }

            return actionPoints;
        }

        public void ResetActionPoints(int playerID, int amount)
        {
            if (ownerPlayer.PlayerID != playerID) return;

            if (Stats.ContainsKey(4))
            {
                int actionPointsReset = 2;
                StatModification statModification = new StatModification(this, Stats[4], actionPointsReset, STATMODIFIERTYPE.CHANGE);
                Stats[4].AddStatModifier(statModification);
                Stats[4].ApplyModifications();
            }
        }

        public IOcuppy GetOcuppy()
        {
            return this;
        }
    }

}
