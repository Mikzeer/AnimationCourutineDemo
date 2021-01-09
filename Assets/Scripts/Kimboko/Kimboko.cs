using System;
using UnityEngine;

namespace PositionerDemo
{
    public abstract class Kimboko : Occupier
    {
        #region VARIABLES
        protected Animator kimbokoAnimator;
        protected Transform kimbokoTransform;
        public UNITTYPE UnitType { get; protected set; }
        public MOVEDIRECTIONTYPE MoveDirectionerType { get; protected set; }
        #endregion

        public Kimboko(int ID, Player ownerPlayer, UNITTYPE UnitType, MOVEDIRECTIONTYPE MoveDirectionerType)
        {
            this.ID = ID;
            OwnerPlayerID = ownerPlayer.ID;
            CardTargetType = CARDTARGETTYPE.UNIT;
            OccupierType = OCUPPIERTYPE.UNIT;

            this.UnitType = UnitType;
            this.MoveDirectionerType = MoveDirectionerType;
            TurnManager.OnUnitResetActionPoints += ResetActionPoints;
        }

        public override void OnSelect(bool isSelected, int playerID)
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
            MonoBehaviour.Destroy(kimbokoTransform.gameObject);
        }

    }
}