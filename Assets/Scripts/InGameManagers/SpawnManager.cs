using AbilitySelectionUI;
using CommandPatternActions;
using StateMachinePattern;
using System.Collections.Generic;
using UnityEngine;

namespace PositionerDemo
{
    public class SpawnManager : AbilityManager
    {
        SpawnManagerUI spawnManagerUI;
        int spawnIndexID = 0;
        bool debugOn = false;
        GameMachine game;
        public SpawnManager(SpawnManagerUI spawnManagerUI, GameMachine game)
        {
            this.spawnManagerUI = spawnManagerUI;
            this.game = game;
        }

        public bool CanIEnterSpawnState(Player player)
        {
            // 6- SI ESTOY ONLINE TENGO QUE PREGUNTARLE AL SERVER SI ES UN MOVIMIENTO VALIDO
            //    SINO CHEQUEO TODO NORMALMENTE
            // QUIEN QUIERE SPAWNEAR, Y EN DONDE QUIERE SPAWNEAR
            // SI EL PLAYER ES VALIDO Y ES SU TURNO
            // Y SI EL LUGAR PARA SPAWNEAR ES UN LUGAR VALIDO
            // ENTONCES EL SERVER TE DICE SI, PODES SPAWNEAR MANDA EL CMD SPAWN A LOS DOS JUGADORES

            // SI EL PLAYER ESTA EN SU TURNO
            if (player != game.turnController.CurrentPlayerTurn)
            {
                if (debugOn) Debug.Log("NO ES EL TURNO DEL PLAYER");
                return false;
            }

            if (player.Abilities.ContainsKey(ABILITYTYPE.SPAWN) == false)
            {
                if (debugOn) Debug.Log("ERROR HABILIDAD SPAWN NO ENCONTRADA EN PLAYER");
                return false;
            }
            SpawnAbility spw = (SpawnAbility)player.Abilities[ABILITYTYPE.SPAWN];
            if (spw == null)
            {
                if (debugOn) Debug.Log("ERROR HABILIDAD SPAWN NO ENCONTRADA EN PLAYER");
                return false;
            }

            return true;
        }

        private bool IsLegalSpawn(Tile TileObject, Player player)
        {
            return true;
        }

        public void OnEnterSpawnState(Player player)
        {
            // CREO LA LISTA/DICCTIONARY DE LAS POSIBLES TILES A SPAWNEAR / SPAWN COMBINAR CON SU HIGHLIGHT CORRESPONDIENTE
            Dictionary<Tile, HIGHLIGHTUITYPE> tileHighlightTypesDictionary = CreateHighlightUIDictionary(player);
            SpawnAbilitySelectionUIContainer spawnUIContainer = new SpawnAbilitySelectionUIContainer(tileHighlightTypesDictionary);
            SpawnState spawn = new SpawnState(game, game.baseStateMachine.currentState, spawnUIContainer);
            game.baseStateMachine.PopState(true);
            game.baseStateMachine.PushState(spawn);
        }

        public bool CanISpawn(Tile TileObject, Player player)
        {
            if (CanIEnterSpawnState(player) == false)
            {
                return false;
            }
            if (TileObject == null)
            {
                if (debugOn) Debug.Log("No Tile Object");
                return false;
            }
            if (!IsLegalSpawn(TileObject, player))
            {
                if (debugOn) Debug.Log("Ilegal Spawn");
                return false;
            }
            SpawnAbility spawnAbility = (SpawnAbility)player.Abilities[ABILITYTYPE.SPAWN];
            if (spawnAbility == null)
            {
                if (debugOn) Debug.Log("ERROR HABILIDAD SPAWN NO ENCONTRADA EN PLAYER");
                return false;
            }

            return true;
        }

        public void OnSpawn(Tile TileObject, Player player)
        {
            SpawnAbility spawnAbility = (SpawnAbility)player.Abilities[ABILITYTYPE.SPAWN];
            SpawnAbilityEventInfo spawnInfo = new SpawnAbilityEventInfo(player, UNITTYPE.X, TileObject, spawnIndexID);
            if (spawnInfo.spawnTile.IsOccupied() && spawnInfo.spawnTile.GetOcuppy().OccupierType == OCUPPIERTYPE.UNIT)
            {
                ExecuteSpecialSpawn(spawnAbility, spawnInfo);
            }
            else if (spawnInfo.spawnTile.IsOccupied() == false)
            {
                ExecuteNormalSpawn(spawnAbility, spawnInfo);
            }
            else
            {
                // ACA ESTA OCUPADA POR UNA BARRICADA ENTONCES NO PODEMOS SPAWNEAR UN CHOTO
                // ES RARO LLEGAR ACA YA QUE ANTES DEBERIA HABER CORTADO LA INVOCACION
            }
        }

        private void ExecuteNormalSpawn(SpawnAbility spawnAbility, SpawnAbilityEventInfo spwInf)
        {
            spawnAbility.SetRequireGameData(spwInf);
            StartPerform(spawnAbility);
            if (spawnAbility.CanIExecute() == false)
            {
                if (debugOn) Debug.Log("SPAWN ABILITY NO SE PUEDE EJECUTAR");
                return;
            }

            GameObject goKimboko = spawnManagerUI.GetKimbokoPrefab();
            ISpawnCommand spawnCommand = new ISpawnCommand(spwInf, goKimboko, game);
            Invoker.AddNewCommand(spawnCommand);
            Invoker.ExecuteCommands();

            Vector3 spawnPosition = spwInf.spawnTile.GetRealWorldLocation();
            Motion normalSpawnMotion = spawnManagerUI.NormalSpawn(spawnPosition, goKimboko);
            InvokerMotion.AddNewMotion(normalSpawnMotion);
            InvokerMotion.StartExecution(spawnManagerUI);

            Perform(spawnAbility);
            EndPerform(spawnAbility);
            spawnIndexID++;
        }

        private void ExecuteSpecialSpawn(SpawnAbility spawnAbility, SpawnAbilityEventInfo spwInf)
        {
            Kimboko unit = (Kimboko)spwInf.spawnTile.GetOcuppy();
            if (unit == null) return;
            if (unit.OwnerPlayerID != spwInf.spawnerPlayer.PlayerID) return;
            bool canCombine = CombineKimbokoRules.CanICombineWithUnitType(unit, spwInf.spawnUnitType);
            bool canCombineAndEvolve = CombineKimbokoRules.CanICombineAndEvolveWithUnitType(unit, spwInf.spawnUnitType);

            if (canCombineAndEvolve)
            {
                ExecuteSpawnCombineAndEvolve(unit, spwInf);
            }
            else if (canCombine)
            {
                ExecuteSpawnCombine(unit, spawnAbility, spwInf);
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

        private void ExecuteSpawnCombine(Kimboko actualCombiner, SpawnAbility spawnAbility, SpawnAbilityEventInfo spwInf)
        {
            CombineAbility combineAbility = (CombineAbility)actualCombiner.Abilities[ABILITYTYPE.COMBINE];
            if (combineAbility == null)
            {
                if (debugOn) Debug.Log("ERROR HABILIDAD COMBINE NULL");
                return;
            }
            Kimboko spawnedKimboko = GetNewKimboko(spwInf);

            CombineAbilityEventInfo cmbInfo = new CombineAbilityEventInfo(actualCombiner, spawnedKimboko, spwInf.spawnerPlayer, spwInf.spawnIndexID);

            spawnAbility.SetRequireGameData(spwInf);
            combineAbility.SetRequireGameData(cmbInfo);

            StartPerform(spawnAbility);
            if (spawnAbility.CanIExecute() == false)
            {
                if (debugOn) Debug.Log("SPAWN ABILITY NO SE PUEDE EJECUTAR");
                return;
            }

            game.combineManager.StartPerform(combineAbility);
            // NO VOY A GENERAR ESTE CHEQUEO, YA QUE EL SPAWN GENERA AUTOMATICAMENTE LA COMBINACION
            //if (combineAbility.CanIExecute() == false)
            //{
            //    if (debugOn) Debug.Log("COMBINE ABILITY NO SE PUEDE EJECUTAR");
            //    return;
            //}

            // C - CombineSpawn(Kimboko actualCombiner, SpawnAbilityEventInfo spwInf)
            GameObject goKimboko = spawnManagerUI.GetKimbokoPrefab();
            spawnedKimboko.SetGoAnimContainer(new GameObjectAnimatorContainer(goKimboko, goKimboko.GetComponent<Animator>()));
            ISpawnCombineCommand spawnCombineCmd = new ISpawnCombineCommand(spawnedKimboko, spwInf, actualCombiner, game);
            Invoker.AddNewCommand(spawnCombineCmd);
            Invoker.ExecuteCommands();
            Vector3 spawnPosition = spwInf.spawnTile.GetRealWorldLocation();
            List<GameObject> combinersGO = new List<GameObject>();
            if (actualCombiner.UnitType == UNITTYPE.COMBINE)
            {
                KimbokoCombine kimbComb = (KimbokoCombine)actualCombiner;
                for (int i = 0; i < kimbComb.kimbokos.Count; i++)
                {
                    combinersGO.Add(kimbComb.kimbokos[i].goAnimContainer.GetGameObject());
                }
            }
            else
            {
                combinersGO.Add(actualCombiner.goAnimContainer.GetGameObject());
            }

            Motion combineSpawnMotion = spawnManagerUI.CombineSpawn(spawnPosition, goKimboko, combinersGO, game);
            InvokerMotion.AddNewMotion(combineSpawnMotion);
            InvokerMotion.StartExecution(spawnManagerUI);

            // D - Perform(spawnAbility);
            //     Perform(combineAbility);
            Perform(spawnAbility);
            game.combineManager.Perform(combineAbility);
            // E - EndPerform(spawnAbility);
            //     EndPerform(combineAbility);
            EndPerform(spawnAbility);
            game.combineManager.EndPerform(combineAbility);
            // F - spawnIndexID++;
            spawnIndexID++;
        }

        private void ExecuteSpawnCombineAndEvolve(Kimboko unit, SpawnAbilityEventInfo spwInf)
        {
            // ACA DEBERIA TENER UNA REFERENCIA AL COMBINE MANAGER
            // ACA DEBERIA TENER UNA REFERENCIA AL EVOLVE MANAGER

            // CREAMOS UNA COMBINE MOTION
            // A - Motion normalSpawnMotion = spawnManagerUI.NormalSpawn(spawnPosition, goKimboko);
            // B - GENERAMOS UN DESTELLO O ALGO SIMILAR EN LA POSICION DE SPAWNEO
            // C - DESTRUIMOS LOS OBJETOS ACTUALES DE LOS KIMBOKOS
            // D - CREAMOS EL NUEVO KIMBOKO EVOLUCIONADO
        }

        private Dictionary<Tile, HIGHLIGHTUITYPE> CreateHighlightUIDictionary(Player player)
        {
            Dictionary<Tile, HIGHLIGHTUITYPE> tileHighlightTypesDictionary = new Dictionary<Tile, HIGHLIGHTUITYPE>();

            List<SpawnTile> spawnTiles = game.board2DManager.GetPlayerSpawnTiles(player.PlayerID);

            if (spawnTiles.Count <= 0) return tileHighlightTypesDictionary;

            for (int i = 0; i < spawnTiles.Count; i++)
            {
                if (spawnTiles[i].IsOccupied())
                {
                    tileHighlightTypesDictionary.Add(spawnTiles[i], HIGHLIGHTUITYPE.COMBINE);
                }
                else
                {
                    tileHighlightTypesDictionary.Add(spawnTiles[i], HIGHLIGHTUITYPE.SPAWN);
                }

            }

            return tileHighlightTypesDictionary;
        }

        private Kimboko GetNewKimboko(SpawnAbilityEventInfo spawnInfo)
        {
            Kimboko kimboko = null;
            switch (spawnInfo.spawnUnitType)
            {
                case UNITTYPE.X:
                    KimbokoXFactory kimbokoXFac = new KimbokoXFactory();
                    kimboko = kimbokoXFac.CreateKimboko(spawnInfo.spawnIndexID, spawnInfo.spawnerPlayer);
                    break;
                case UNITTYPE.Y:
                    KimbokoYFactory kimbokoYFac = new KimbokoYFactory();
                    kimboko = kimbokoYFac.CreateKimboko(spawnInfo.spawnIndexID, spawnInfo.spawnerPlayer);
                    break;
                case UNITTYPE.Z:
                    KimbokoZFactory kimbokoZFac = new KimbokoZFactory();
                    kimboko = kimbokoZFac.CreateKimboko(spawnInfo.spawnIndexID, spawnInfo.spawnerPlayer);
                    break;
                default:
                    break;
            }
            return kimboko;
        }
    }
}
