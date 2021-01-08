using System;
using UnityEngine;

namespace PositionerDemo
{
    public class TakeCardAbility : GenericAbilityAction<TakeCardAbilityEventInfo>
    {
        private const int TAKECARDABILITYID = 1;
        private const ABILITYTYPE TYPEABILITY = ABILITYTYPE.TAKEACARD;
        private const int ACTIONPOINTSREQUIRED = 1;
        private Player player;

        public static Action<TakeCardAbilityEventInfo> OnActionStartExecute { get; set; }
        public static Action<TakeCardAbilityEventInfo> OnActionEndExecute { get; set; }

        public TakeCardAbilityEventInfo takeCardAbilityInfo;

        public TakeCardAbility(IOcuppy performerIOcuppy) : base(TAKECARDABILITYID, performerIOcuppy, ACTIONPOINTSREQUIRED, TYPEABILITY)
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

        public override bool OnTryEnter()
        {
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
            return true;
        }

        public override void OnResetActionExecution()
        {
            if (GameCreator.Instance.turnManager.GetActualPlayerTurn() == player)
            {
                //Debug.Log("Reseteaer la Take Card Hability Player:" + player.PlayerID);
                actionStatus = ABILITYEXECUTIONSTATUS.WAIT;
            }
        }

        public override bool OnTryExecute()
        {
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

        public override void OnStartExecute()
        {
            //OnActionStarExecute?.Invoke(actor, ActionEventInformation);
        }

        public override void Execute()
        {
            if (actionStatus == ABILITYEXECUTIONSTATUS.CANCELED)
            {
                Debug.Log("TakeCardAbility CANCEL");
                return;
            }

            if (OnTryExecute() == false)
            {
                actionStatus = ABILITYEXECUTIONSTATUS.CANCELED;
                return;
            }

            actionStatus = ABILITYEXECUTIONSTATUS.STARTED;
        }

        public override void OnEndExecute()
        {
            OnActionEndExecute?.Invoke(takeCardAbilityInfo);
        }
    }
}
