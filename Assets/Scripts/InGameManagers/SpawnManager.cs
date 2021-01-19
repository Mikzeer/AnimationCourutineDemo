using CommandPatternActions;
using UnityEngine;

namespace PositionerDemo
{
    public class SpawnManager : AbilityManager
    {
        SpawnManagerUI spawnManagerUI;
        int spawnIndexID = 0;
        bool debugOn = false;
        IGame game;
        public SpawnManager(SpawnManagerUI spawnManagerUI, IGame game)
        {
            this.spawnManagerUI = spawnManagerUI;
            this.game = game;
        }

        public void OnTrySpawn(Tile TileObject, Player player)
        {
            // 6- SI ESTOY ONLINE TENGO QUE PREGUNTARLE AL SERVER SI ES UN MOVIMIENTO VALIDO
            //    SINO CHEQUEO TODO NORMALMENTE
            // QUIEN QUIERE SPAWNEAR, Y EN DONDE QUIERE SPAWNEAR
            // SI EL PLAYER ES VALIDO Y ES SU TURNO
            // Y SI EL LUGAR PARA SPAWNEAR ES UN LUGAR VALIDO
            // ENTONCES EL SERVER TE DICE SI, PODES SPAWNEAR MANDA EL CMD SPAWN A LOS DOS JUGADORES
            if (TileObject == null)
            {
                if(debugOn) Debug.Log("No Tile Object");
                return;
            }
            if (!IsLegalSpawn(TileObject, player))
            {
                if (debugOn) Debug.Log("Ilegal Spawn");
                return;
            }
            if (player.Abilities.ContainsKey(ABILITYTYPE.SPAWN) == false)
            {
                if (debugOn) Debug.Log("ERROR HABILIDAD SPAWN NO ENCONTRADA EN PLAYER");
                return;
            }
            SpawnAbility spw = (SpawnAbility)player.Abilities[ABILITYTYPE.SPAWN];
            if (spw == null)
            {
                if (debugOn) Debug.Log("ERROR HABILIDAD SPAWN NO ENCONTRADA EN PLAYER");
                return;
            }

            SpawnAbilityEventInfo spwInf = new SpawnAbilityEventInfo(player, UNITTYPE.X, TileObject, spawnIndexID);
            spw.SetRequireGameData(spwInf);
            StartPerform(spw);

            if (spw.CanIExecute() == false)
            {
                if (debugOn) Debug.Log("SPAWN ABILITY NO SE PUEDE EJECUTAR");
                return;
            }
            Spawn(spwInf);
            EndPerform(spw);
            spawnIndexID++;
        }

        private bool IsLegalSpawn(Tile TileObject, Player player)
        {
            return true;
        }

        private void Spawn(SpawnAbilityEventInfo spwInf)
        {
            if (spwInf.spawnTile.IsOccupied() && spwInf.spawnTile.GetOcuppy().OccupierType == OCUPPIERTYPE.UNIT)
            {
                SpecialSpawn(spwInf);
            }
            else if (spwInf.spawnTile.IsOccupied() == false)
            {
                NormalSpawn(spwInf);
            }
            else
            {
                // ACA ESTA OCUPADA POR UNA BARRICADA ENTONCES NO PODEMOS SPAWNEAR UN CHOTO
                // ES RARO LLEGAR ACA YA QUE ANTES DEBERIA HABER CORTADO LA INVOCACION
            }
        }

        private void NormalSpawn(SpawnAbilityEventInfo spwInf)
        {
            GameObject goKimboko = spawnManagerUI.GetKimbokoPrefab();
            ISpawnCommand spawnCommand = new ISpawnCommand(spwInf, goKimboko, game);
            Invoker.AddNewCommand(spawnCommand);
            Invoker.ExecuteCommands();

            Vector3 spawnPosition = spwInf.spawnTile.GetRealWorldLocation();
            Motion normalSpawnMotion = spawnManagerUI.NormalSpawn(spawnPosition, goKimboko);
            InvokerMotion.AddNewMotion(normalSpawnMotion);
        }

        private void SpecialSpawn(SpawnAbilityEventInfo spwInf)
        {
            Kimboko unit = (Kimboko)spwInf.spawnTile.GetOcuppy();
            if (unit == null) return;
            if (unit.OwnerPlayerID != spwInf.spawnerPlayer.PlayerID) return;
            bool canCombine = CombineKimbokoRules.CanICombineWithUnitType(unit, spwInf.spawnUnitType);
            bool canCombineAndEvolve = CombineKimbokoRules.CanICombineAndEvolveWithUnitType(unit, spwInf.spawnUnitType);


            if (canCombineAndEvolve)
            {
                CombineAndEvolveSpawn(unit, spwInf);
            }
            else if (canCombine)
            {
                CombineSpawn(unit, spwInf);
            }
            else
            {
                // Y ACA NO SE PUDO COMBINAR NI EVOLUCIONAR
                // ESTO PUEDE PASAR POR INVOCAR UNA UNIDAD Y QUE SE LE CAMBIE DE CLASE
                // AL HACER ESTO NO PODEMOS INVOCARLA POR QUE SON INCOMPATIBLES
                // SE EJECUTA IGUAL EL END PERFORM?????
                // PODRIAMOS TENER UN ESTADO EN EL EVENT INFO PARA PONER CANCEL EXECUTION
                // ENTONCES DE ESTA MANERA SABEMOS QUE SE CANCELO Y NO HACEMOS EL END PERFORM...
            }
        }

        private void CombineSpawn(Kimboko unit, SpawnAbilityEventInfo spwInf)
        {
            // ACA DEBERIA TENER UNA REFERENCIA AL COMBINE MANAGER

            // 1 - TENEMOS QUE CREAR EL COMBINE COMMAND... Y QUE MIERDA SERIA LO QUE HACE????? XD XD XD


            // ENTONCES CREAMOS UNA COMBINE MOTION CON ESTOS TRES 
            // A - MOVEMOS A LOS KIMBOKOS QUE OCUPEN LA TILE A LOS COSTADOS

            // B - Motion normalSpawnMotion = spawnManagerUI.NormalSpawn(spawnPosition, goKimboko);
            GameObject goKimboko = spawnManagerUI.GetKimbokoPrefab();
            ISpawnCommand spawnCommand = new ISpawnCommand(spwInf, goKimboko, game);
            Invoker.AddNewCommand(spawnCommand);
            Invoker.ExecuteCommands();

            Vector3 spawnPosition = spwInf.spawnTile.GetRealWorldLocation();
            Motion normalSpawnMotion = spawnManagerUI.NormalSpawn(spawnPosition, goKimboko);
            InvokerMotion.AddNewMotion(normalSpawnMotion);


            // C - REPOSICIONAMOS A LOS KIMBOKO
        }

        private void CombineAndEvolveSpawn(Kimboko unit, SpawnAbilityEventInfo spwInf)
        {
            // ACA DEBERIA TENER UNA REFERENCIA AL COMBINE MANAGER
            // ACA DEBERIA TENER UNA REFERENCIA AL EVOLVE MANAGER

            // CREAMOS UNA COMBINE MOTION
            // A - Motion normalSpawnMotion = spawnManagerUI.NormalSpawn(spawnPosition, goKimboko);
            // B - GENERAMOS UN DESTELLO O ALGO SIMILAR EN LA POSICION DE SPAWNEO
            // C - DESTRUIMOS LOS OBJETOS ACTUALES DE LOS KIMBOKOS
            // D - CREAMOS EL NUEVO KIMBOKO EVOLUCIONADO
        }
    }
}
