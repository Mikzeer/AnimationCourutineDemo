using CommandPatternActions;
using UnityEngine;

namespace PositionerDemo
{
    public class SpawnManager : AbilityManager
    {
        SpawnManagerUI spawnManagerUI;
        int spawnIndexID = 0;
        bool debugOn = false;

        public SpawnManager(SpawnManagerUI spawnManagerUI)
        {
            this.spawnManagerUI = spawnManagerUI;
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

        public bool IsLegalSpawn(Tile TileObject, Player player)
        {
            return true;
        }

        public void Spawn(SpawnAbilityEventInfo spwInf)
        {
            if (spwInf.spawnTile.IsOccupied() && spwInf.spawnTile.GetOcuppy().OccupierType == OCUPPIERTYPE.UNIT)
            {
                CombineSpawn(spwInf);
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

        public void NormalSpawn(SpawnAbilityEventInfo spwInf)
        {
            Kimboko kimboko = GetNewKimboko(spwInf.spawnerPlayer, spwInf.spawnIndexID, spwInf.spawnUnitType);
            GameObject goKimboko = spawnManagerUI.GetKimbokoPrefab();

            kimboko.SetGoAnimContainer(new GameObjectAnimatorContainer(goKimboko, goKimboko.GetComponent<Animator>()));

            ISpawnCommand spawnCommand = new ISpawnCommand(spwInf.spawnTile, spwInf.spawnerPlayer, kimboko);
            Invoker.AddNewCommand(spawnCommand);
            Invoker.ExecuteCommands();

            Vector3 spawnPosition = spwInf.spawnTile.GetRealWorldLocation();
            Motion normalSpawnMotion = spawnManagerUI.NormalSpawn(spawnPosition, goKimboko);
            InvokerMotion.AddNewMotion(normalSpawnMotion);
        }

        public void CombineSpawn(SpawnAbilityEventInfo spwInf)
        {
            Kimboko unit = (Kimboko)spwInf.spawnTile.GetOcuppy();
            if (unit == null) return;
            if (unit.OwnerPlayerID != spwInf.spawnerPlayer.PlayerID) return;
            Kimboko kimboko = GetNewKimboko(spwInf.spawnerPlayer, spwInf.spawnIndexID, spwInf.spawnUnitType);

            bool canCombine = CombineKimbokoRules.CanICombine(unit, kimboko);
            bool canCombineAndEvolve = CombineKimbokoRules.CanICombineAndEvolve(unit, kimboko);

            if (canCombineAndEvolve)
            {
                // ACA DEBERIA TENER UNA REFERENCIA AL COMBINE MANAGER
                // ACA DEBERIA TENER UNA REFERENCIA AL EVOLVE MANAGER

                // CREAMOS UNA COMBINE MOTION
                // A - Motion normalSpawnMotion = spawnManagerUI.NormalSpawn(spawnPosition, goKimboko);
                // B - GENERAMOS UN DESTELLO O ALGO SIMILAR EN LA POSICION DE SPAWNEO
                // C - DESTRUIMOS LOS OBJETOS ACTUALES DE LOS KIMBOKOS
                // D - CREAMOS EL NUEVO KIMBOKO EVOLUCIONADO
            }
            else if (canCombine)
            {
                // ACA DEBERIA TENER UNA REFERENCIA AL COMBINE MANAGER

                // ENTONCES CREAMOS UNA COMBINE MOTION CON ESTOS TRES 
                // A - MOVEMOS A LOS KIMBOKOS QUE OCUPEN LA TILE A LOS COSTADOS
                // B - Motion normalSpawnMotion = spawnManagerUI.NormalSpawn(spawnPosition, goKimboko);
                // C - REPOSICIONAMOS A LOS KIMBOKO
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

        public Kimboko GetNewKimboko(Player player, int spawnIndexID, UNITTYPE unitType)
        {
            Kimboko kimboko = null;
            switch (unitType)
            {
                case UNITTYPE.X:
                    KimbokoXFactory kimbokoXFac = new KimbokoXFactory();
                    kimboko = kimbokoXFac.CreateKimboko(spawnIndexID, player);
                    break;
                case UNITTYPE.Y:
                    KimbokoYFactory kimbokoYFac = new KimbokoYFactory();
                    kimboko = kimbokoYFac.CreateKimboko(spawnIndexID, player);
                    break;
                case UNITTYPE.Z:
                    KimbokoZFactory kimbokoZFac = new KimbokoZFactory();
                    kimboko = kimbokoZFac.CreateKimboko(spawnIndexID, player);
                    break;
                case UNITTYPE.COMBINE:
                    break;
                case UNITTYPE.FUSION:
                    break;
                default:
                    break;
            }

            return kimboko;
        }
    }
}
