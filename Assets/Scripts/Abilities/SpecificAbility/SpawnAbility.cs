using System;
using UnityEngine;

namespace PositionerDemo
{
    public class SpawnAbility : AbilityAction
    {
        private const int SPAWNABILITYID = 0;
        private const ABILITYTYPE TYPEABILITY = ABILITYTYPE.SPAWN;
        private const int ACTIONPOINTSREQUIRED = 1;
        Tile selectedTile;        
        private Player player;

        public static Action<SpawnAbilityEventInfo> OnActionStartExecute { get; set; }
        public static Action<SpawnAbilityEventInfo> OnActionEndExecute { get; set; }

        public SpawnAbilityEventInfo spawnAbilityInfo;

        public SpawnAbility(IOcuppy performerIOcuppy) : base(SPAWNABILITYID, performerIOcuppy, ACTIONPOINTSREQUIRED, TYPEABILITY)
        {
            actionStatus = ABILITYEXECUTIONSTATUS.WAIT;
            if (performerIOcuppy.OccupierType == OCUPPIERTYPE.PLAYER)
            {
                player = (Player)performerIOcuppy;
            }
        }

        public void Set(Tile selectedTile)
        {
            this.selectedTile = selectedTile;
            spawnAbilityInfo = new SpawnAbilityEventInfo(player, UNITTYPE.X, selectedTile);
            //ChangeUnitClassAbilityModifier ab = new ChangeUnitClassAbilityModifier(player);
            //AddAbilityModifier(ab);
        }

        public override bool OnTryEnter()
        {
            // 1- TENER LOS AP NECESARIOS
            if (performerIOcuppy.GetCurrentActionPoints() < GetActionPointsRequiredToUseAbility())
            {
                return false;
            }
            
            // 2- QUE EXISTAN TILES VACIAS EN LA BASE POR AHORA
            return true;

            // 3- EN ESTE CASO NO VAMOS A CHEQUEAR SI YA SE EJECUTO, YA QUE PUEDE EJECUTARSE VARIAS VECES POR TURNO SOLO AL INICIO DE LA PARTIDA
            //    DESPUES SOLO UNA VEZ


            // QUIEN SE VA A ENCARGAR DE "" MARCAR LAS TILES DEL SPAWN COMO ""SPAWNEABLES"????
            // LA HABILIDAD O EL STATE?
        }

        public override void OnResetActionExecution()
        {
            if (GameCreator.Instance.turnManager.GetActualPlayerTurn() == player)
            {
                //Debug.Log("Es mi turno Player y voy a reseteaer la Spawn Hability " + player.PlayerID);
                actionStatus = ABILITYEXECUTIONSTATUS.WAIT;
            }
            //else
            //{
            //    Debug.Log("NO ES MI TURNO Player " + player.PlayerID);
            //    Debug.Log("NO ES MI TURNO Tengo Tantos Action Points " + player.GetCurrentActionPoints());
            //}
        }

        public override bool OnTryExecute()
        {
            // 1- TENER LOS AP NECESARIOS
            if (performerIOcuppy.GetCurrentActionPoints() < GetActionPointsRequiredToUseAbility())
            {
                Debug.Log("Spawn Ability: Not Enough Action Points");
                return false;
            }

            // 2- QUE EXISTA UNA TILE SELECCIONADA
            if (selectedTile == null)
            {
                Debug.Log("Spawn Ability: Not Selected Tile");
                return false;
            }

            // 2- QUE SEA UNA TILE DE LA BASE
            if (selectedTile.tileType != TILETYPE.SPAWN)
            {
                Debug.Log("Spawn Ability: Not Selected Spawn Tile");
                return false;
            }

            // 3- QUE SEA UN SPAWN AUTENTICO
            SpawnTile spawnTile = (SpawnTile)selectedTile;

            if (spawnTile == null)
            {
                Debug.Log("Spawn Ability: Spawn Tile Null");
                return false;
            }

            // 3- QUE SEA LA BASE DEL PLAYER QUE LA CLICKEO
            if (spawnTile.PlayerID != player.PlayerID)
            {
                Debug.Log("Spawn Ability: Enemy Spawn Tile Selected");
                return false;
            }

            // 5- QUE LA TILE NO ESTE OCUPADA POR UN ENEMIGO, SINO YA SERIA LA HABILIDAD DE ATTACK EN BASE
            if (spawnTile.IsOccupied())
            {
                if (spawnTile.GetOccupier().OccupierType == OCUPPIERTYPE.UNIT)
                {
                    Kimboko unit = (Kimboko)spawnTile.GetOccupier();
                    Debug.Log("Spawn Ability: Tile Selected Is Occupied By a Kimboko");
                    if (unit.ownerPlayer != player)
                    {
                        return false;
                    }
                    return false;
                }
            }


            // 6- SI ESTOY ONLINE TENGO QUE PREGUNTARLE AL SERVER SI ES UN MOVIMIENTO VALIDO
            // QUIEN QUIERE SPAWNEAR, Y EN DONDE QUIERE SPAWNEAR
            // SI EL PLAYER ES VALIDO Y ES SU TURNO
            // Y SI EL LUGAR PARA SPAWNEAR ES UN LUGAR VALIDO
            // ENTONCES EL SERVER TE DICE SI, PODES SPAWNEAR

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
                Debug.Log("SpawnAbility CANCEL");
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
            OnActionEndExecute?.Invoke(spawnAbilityInfo);
        }

    }
}
