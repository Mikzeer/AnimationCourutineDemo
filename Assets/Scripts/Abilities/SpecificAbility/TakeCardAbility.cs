using System;
using UnityEngine;

namespace PositionerDemo
{
    public class TakeCardAbility : Ability<TakeCardAbilityEventInfo>
    {
        private const ABILITYTYPE TYPEABILITY = ABILITYTYPE.TAKEACARD;
        private const int ACTIONPOINTSREQUIRED = 1;
        private Player player;

        public TakeCardAbility(IOcuppy performerIOcuppy) : base(performerIOcuppy, ACTIONPOINTSREQUIRED, TYPEABILITY)
        {
            actionStatus = ABILITYEXECUTIONSTATUS.WAIT;
            if (performerIOcuppy.OccupierType == OCUPPIERTYPE.PLAYER)
            {
                player = (Player)performerIOcuppy;
            }
        }

        public override void SetRequireGameData(TakeCardAbilityEventInfo gameData)
        {            
        }

        public override bool CanIExecute()
        {
            if (actionStatus == ABILITYEXECUTIONSTATUS.CANCELED) return false;

            // 1- TENER LOS AP NECESARIOS
            if (performerIOcuppy.GetCurrentActionPoints() < GetActionPointsRequiredToUseAbility())
            {
                return false;
            }

            // 2- TENER UN PLAYER Y QUE TENGA CARDS EN SU MAZO
            if (player != null)
            {
                if (player.Deck.Count <= 0)
                {
                    Debug.Log("TakeCardAbility: No Cards On Deck");
                    return false;
                }
            }

            // 6- SI ESTOY ONLINE TENGO QUE PREGUNTARLE AL SERVER SI ES UN MOVIMIENTO VALIDO
            // QUIEN QUIERE LEVANTAR CARD
            // SI EL PLAYER ES VALIDO Y ES SU TURNO
            // ENTONCES EL SERVER TE DICE SI, PODES LEVANTAR CARD
            return true;
        }
    }
}
