using System;
using UnityEngine;

namespace PositionerDemo
{
    public class SpawnAbility : GenericAbilityAction<SpawnAbilityEventInfo>
    {
        private const ABILITYTYPE TYPEABILITY = ABILITYTYPE.SPAWN;
        private const int ACTIONPOINTSREQUIRED = 1;
        Tile selectedTile;        
        private Player player;

        public SpawnAbility(IOcuppy performerIOcuppy) : base(performerIOcuppy, ACTIONPOINTSREQUIRED, TYPEABILITY)
        {
            actionStatus = ABILITYEXECUTIONSTATUS.WAIT;
            if (performerIOcuppy.OccupierType == OCUPPIERTYPE.PLAYER)
            {
                player = (Player)performerIOcuppy;
            }
        }

        public override void SetRequireGameData(SpawnAbilityEventInfo gameData)
        {
            selectedTile = gameData.spawnTile;
            actionInfo = new SpawnAbilityEventInfo(gameData.spawnerPlayer, gameData.spawnUnitType , gameData.spawnTile);
            //Debug.Log("Set Data Spawn Ability");
        }

        public override bool CanIExecute()
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
                if (spawnTile.GetOcuppy().OccupierType == OCUPPIERTYPE.UNIT)
                {
                    Kimboko unit = (Kimboko)spawnTile.GetOcuppy();
                    Debug.Log("Spawn Ability: Tile Selected Is Occupied By a Kimboko");
                    if (unit.OwnerPlayerID != player.PlayerID)
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

        public override void Execute()
        {
            // ACA TAMBIEN SE DEBERIA CREAR EL CMD

            if (actionStatus == ABILITYEXECUTIONSTATUS.CANCELED)
            {
                Debug.Log("SpawnAbility CANCEL");
                return;
            }

            if (CanIExecute() == false)
            {
                actionStatus = ABILITYEXECUTIONSTATUS.CANCELED;
                return;
            }

            actionStatus = ABILITYEXECUTIONSTATUS.STARTED;
        }

    }
}
