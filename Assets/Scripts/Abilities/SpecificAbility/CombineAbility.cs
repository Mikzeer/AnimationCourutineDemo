using UnityEngine;

namespace PositionerDemo
{
    public class CombineAbility : Ability<CombineAbilityEventInfo>
    {
        private const ABILITYTYPE TYPEABILITY = ABILITYTYPE.COMBINE;
        private const int ACTIONPOINTSREQUIRED = 2;

        public CombineAbility(IOcuppy performerIOcuppy) : base(performerIOcuppy, ACTIONPOINTSREQUIRED, TYPEABILITY)
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
                Debug.Log("Spawn Ability: Not Enough Action Points");
                return false;
            }
            // 2- VER SI LOS PUEDO COMBINAR
            if (CombineKimbokoRules.CanICombine(actionInfo.combiner, actionInfo.kimbokoToCombine) == false)
            {
                Debug.Log("You cant combine this two kimbokos");
                return false;
            }

            // 3- ME TENGO QUE FIJAR QUE NO HAYA ESTE DEFENDIENDO... AUNQUE ESO AUTOMATICAMENTE DEBERIA PONERME EN 0 LOS AC
            // 3- ACA DEBO CHEQUEAR LAS REGLAS DE COMBINACION, NO SOLO SI LOS PJ SON APTOS, SINO TAMBIEN SI MI PJ PUEDE EJECUTARLA

            return true;
        }

        public override void SetRequireGameData(CombineAbilityEventInfo gameData)
        {
            actionInfo = new CombineAbilityEventInfo(gameData.combiner, gameData.kimbokoToCombine, gameData.player, gameData.IndexID);
        }
    }
}
