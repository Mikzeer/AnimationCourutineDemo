﻿using UnityEngine;

namespace PositionerDemo
{
    public abstract class Kimboko : Occupier
    {
        #region VARIABLES
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
        }

        public override void OnSelect(bool isSelected, int playerID)
        {
            Debug.Log("KIMBOKO SELECTED");
        }

    }
}