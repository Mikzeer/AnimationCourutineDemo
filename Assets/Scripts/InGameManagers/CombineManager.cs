using AbilitySelectionUI;
using CommandPatternActions;
using StateMachinePattern;
using System.Collections.Generic;
using UnityEngine;

namespace PositionerDemo
{
    public class CombineManager : AbilityManager
    {
        bool debugOn = false;
        GameMachine game;
        CombineManagerUI combineManagerUI;
        public CombineManager(GameMachine game, CombineManagerUI combineManagerUI)
        {
            this.game = game;
            this.combineManagerUI = combineManagerUI;
        }

        public bool CanIEnterCombineState(Kimboko kimboko)
        {
            // 6- SI ESTOY ONLINE TENGO QUE PREGUNTARLE AL SERVER SI ES UN MOVIMIENTO VALIDO
            //    SINO CHEQUEO TODO NORMALMENTE
            // QUIEN QUIERE SPAWNEAR, Y EN DONDE QUIERE SPAWNEAR
            // SI EL PLAYER ES VALIDO Y ES SU TURNO
            // Y SI EL LUGAR PARA SPAWNEAR ES UN LUGAR VALIDO
            // ENTONCES EL SERVER TE DICE SI, PODES SPAWNEAR MANDA EL CMD SPAWN A LOS DOS JUGADORES

            // SI EL PLAYER ESTA EN SU TURNO
            //if (player != game.turnController.CurrentPlayerTurn)
            //{
            //    if (debugOn) Debug.Log("NO ES EL TURNO DEL PLAYER");
            //    return false;
            //}

            //if (player.Abilities.ContainsKey(ABILITYTYPE.SPAWN) == false)
            //{
            //    if (debugOn) Debug.Log("ERROR HABILIDAD SPAWN NO ENCONTRADA EN PLAYER");
            //    return false;
            //}
            //SpawnAbility spw = (SpawnAbility)player.Abilities[ABILITYTYPE.SPAWN];
            //if (spw == null)
            //{
            //    if (debugOn) Debug.Log("ERROR HABILIDAD SPAWN NO ENCONTRADA EN PLAYER");
            //    return false;
            //}

            return false;
        }

        public bool IsLegalCombine(CombineAbilityEventInfo cmbInfo)
        {
            return true;
        }

        public void OnEnterCombineState(Player player)
        {
            // CREO LA LISTA/DICCTIONARY DE LAS POSIBLES TILES A SPAWNEAR / SPAWN COMBINAR CON SU HIGHLIGHT CORRESPONDIENTE
            //Dictionary<Tile, HIGHLIGHTUITYPE> tileHighlightTypesDictionary = CreateHighlightUIDictionary(player);
            //SpawnAbilitySelectionUIContainer spawnUIContainer = new SpawnAbilitySelectionUIContainer(tileHighlightTypesDictionary);
            //SpawnState spawn = new SpawnState(game, game.baseStateMachine.currentState, spawnUIContainer);
            //game.baseStateMachine.PopState(true);
            //game.baseStateMachine.PushState(spawn);
        }

        public bool CanICombine(CombineAbilityEventInfo cmbInfo)
        {
            if (CanIEnterCombineState(cmbInfo.combiner) == false)
            {
                return false;
            }
            if (!IsLegalCombine(cmbInfo))
            {
                if (debugOn) Debug.Log("Ilegal Combine");
                return false;
            }
            if (cmbInfo.combiner.Abilities.ContainsKey(ABILITYTYPE.COMBINE) == false)
            {
                if (debugOn) Debug.Log("ERROR HABILIDAD COMBINE NO ENCONTRADA");
                return false; 
            }
            CombineAbility cmb = (CombineAbility)cmbInfo.combiner.Abilities[ABILITYTYPE.COMBINE];
            if (cmb == null)
            {
                if (debugOn) Debug.Log("ERROR HABILIDAD COMBINE NULL");
                return false;
            }
            return true;
        }

        public void OnCombine(CombineAbilityEventInfo cmbInfo)
        {
            CombineAbility combineAbility = (CombineAbility)cmbInfo.combiner.Abilities[ABILITYTYPE.COMBINE];
            ExecuteNormalCombine(combineAbility, cmbInfo);
        }

        public void ExecuteNormalCombine(CombineAbility combineAbility, CombineAbilityEventInfo cmbInfo)
        {
            combineAbility.SetRequireGameData(cmbInfo);
            StartPerform(combineAbility);
            if (combineAbility.CanIExecute() == false)
            {
                if (debugOn) Debug.Log("COMBINE ABILITY NO SE PUEDE EJECUTAR");
                return;
            }

            ICombineCommand combineCommand = new ICombineCommand(cmbInfo, game);
            Invoker.AddNewCommand(combineCommand);
            Invoker.ExecuteCommands();

            Vector3 spawnPosition = game.board2DManager.GetUnitPosition(cmbInfo.combiner).GetRealWorldLocation();

            List<GameObject> combinersGO = new List<GameObject>();
            if (cmbInfo.combiner.UnitType == UNITTYPE.COMBINE)
            {
                KimbokoCombine kimbComb = (KimbokoCombine)cmbInfo.combiner;
                for (int i = 0; i < kimbComb.kimbokos.Count; i++)
                {
                    combinersGO.Add(kimbComb.kimbokos[i].goAnimContainer.GetGameObject());
                }
            }
            else
            {
                combinersGO.Add(cmbInfo.combiner.goAnimContainer.GetGameObject());
            }

            Motion combineMoveMotion = combineManagerUI.NormalCombineMotion(spawnPosition, cmbInfo.kimbokoToCombine.goAnimContainer.GetGameObject(), combinersGO, game);
            InvokerMotion.AddNewMotion(combineMoveMotion);
            InvokerMotion.StartExecution(combineManagerUI);

            Perform(combineAbility);
            EndPerform(combineAbility);
        }

        private Dictionary<Tile, HIGHLIGHTUITYPE> CreateHighlightUIDictionary(Player player)
        {
            Dictionary<Tile, HIGHLIGHTUITYPE> tileHighlightTypesDictionary = new Dictionary<Tile, HIGHLIGHTUITYPE>();

            //List<SpawnTile> spawnTiles = game.board2DManager.GetPlayerSpawnTiles(player.PlayerID);

            //if (spawnTiles.Count <= 0) return tileHighlightTypesDictionary;

            //for (int i = 0; i < spawnTiles.Count; i++)
            //{
            //    if (spawnTiles[i].IsOccupied())
            //    {
            //        tileHighlightTypesDictionary.Add(spawnTiles[i], HIGHLIGHTUITYPE.COMBINE);
            //    }
            //    else
            //    {
            //        tileHighlightTypesDictionary.Add(spawnTiles[i], HIGHLIGHTUITYPE.SPAWN);
            //    }

            //}

            return tileHighlightTypesDictionary;
        }
    }
}
