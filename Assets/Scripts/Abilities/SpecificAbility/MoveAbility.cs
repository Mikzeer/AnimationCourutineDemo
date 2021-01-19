using UnityEngine;

namespace PositionerDemo
{
    public class MoveAbility : Ability<MoveAbilityEventInfo>
    {
        private const ABILITYTYPE TYPEABILITY = ABILITYTYPE.MOVE;
        private const int ACTIONPOINTSREQUIRED = 1;

        public MoveAbility(IOcuppy performerIOcuppy) : base(performerIOcuppy, ACTIONPOINTSREQUIRED, TYPEABILITY)
        {
            actionStatus = ABILITYEXECUTIONSTATUS.WAIT;
        }

        public override bool CanIExecute()
        {
            // QUE LA ACCION NO ESTE CANCELADA
            if (actionStatus == ABILITYEXECUTIONSTATUS.CANCELED) return false;
            // 1- TENER LOS AP NECESARIOS
            if (performerIOcuppy.GetCurrentActionPoints() < GetActionPointsRequiredToUseAbility())
            {
                Debug.Log("Move Ability: Not Enough Action Points");
                return false;
            }

            // 2 - QUE A DONDE ME QUIERA MOVER NO ESTE OCUPADO
            if (actionInfo.endPosition.IsOccupied())
            {
                return false;
            }

            return true;
        }

        public override void SetRequireGameData(MoveAbilityEventInfo gameData)
        {
            actionInfo = new MoveAbilityEventInfo(gameData.moveOccupy, gameData.fromTile, gameData.endPosition);
        }
    }
}
