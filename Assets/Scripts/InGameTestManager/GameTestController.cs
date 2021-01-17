using CommandPatternActions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PositionerDemo
{
    public class GameTestController : MonoBehaviour, IGame
    {
        #region VARIABLES
        [Header("SPAWN MANAGER UI")]
        [SerializeField] private SpawnManagerUI spawnManagerUI = default; // DE ESTA MANERA EVITAMOS EL WARNING EN EL INSPECTOR
        public SpawnManager spawnManager { get; private set; }
        [Header("BOARD MANAGER UI")]
        [SerializeField] private Board2DManagerUI board2DManagerUI = default;
        public Board2DManager board2DManager { get; private set; }
        [Header("TILE SELECTION MANAGER UI")]
        [SerializeField] private TileSelectionManagerUI tileSelectionManagerUI = default;
        [Header("TOGGLE CONTROLLER")]
        [SerializeField] private ToggleController toggleController = default;
        MouseController mouseController;
        KeyBoardController keyBoardController;
        Player[] players;
        public CombineManager combineManager { get; private set; }
        #endregion

        private void Start()
        {
            board2DManager = new Board2DManager(board2DManagerUI, 5, 7);
            CreatePlayers();
            Motion motion = board2DManager.CreateBoard(players, OnBoardComplete);
            InvokerMotion.AddNewMotion(motion);
            InvokerMotion.StartExecution(this);
            spawnManager = new SpawnManager(spawnManagerUI, this);
            combineManager = new CombineManager(this);
            tileSelectionManagerUI.onTileSelected += ExecuteActions;
        }

        private void CreatePlayers()
        {
            Player playerOne = new Player(0);
            playerOne.SetStatsAndAbilities(OccupierAbilityDatabase.CreatePlayerAbilities(playerOne), OccupierStatDatabase.CreatePlayerStat());

            Player playerTwo = new Player(1);
            playerTwo.SetStatsAndAbilities(OccupierAbilityDatabase.CreatePlayerAbilities(playerTwo), OccupierStatDatabase.CreatePlayerStat());

            // DEBERIAMOS TENER UN ABILITYMODIFIER MANAGER O ALGO SIMILIAR PARA ENCARGARSE DE ESTO TAL VEZ
            //SpawnAbility spw = (SpawnAbility)playerOne.Abilities[ABILITYTYPE.SPAWN];
            //ChangeUnitClassAbilityModifier ab = new ChangeUnitClassAbilityModifier(playerOne);
            //spw.AddAbilityModifier(ab);
            //CanceclSpawnAbilityModifier cancelSpawn = new CanceclSpawnAbilityModifier(playerOne);
            //spw.AddAbilityModifier(cancelSpawn);

            //TestAbilityModifier cnlEnemy = new TestAbilityModifier(playerTwo);
            //IAddSimpleAbilityActionModifierCommand simple = new IAddSimpleAbilityActionModifierCommand(playerTwo.GeneralModifiers, cnlEnemy);
            //Invoker.AddNewCommand(simple);
            //Invoker.ExecuteCommands();
            players = new Player[2];
            players[0] = playerOne;
            players[1] = playerTwo;
        }
     
        private void OnBoardComplete()
        {
            Debug.Log("BOARD SE TERMINO DE CREAR");
            mouseController = new MouseController(0, board2DManager, Camera.main);
            keyBoardController = new KeyBoardController(0, board2DManager, Camera.main);
            tileSelectionManagerUI.SetController(board2DManager, mouseController, keyBoardController);
        }

        private void ExecuteActions(Tile tile)
        {
            switch (toggleController.StateType)
            {
                case ToggleController.STATETYPE.SPAWN:
                    spawnManager.OnTrySpawn(tile, players[0]);
                    break;
                case ToggleController.STATETYPE.MOVE:
                    break;
                case ToggleController.STATETYPE.ATTACK:
                    break;
                default:
                    break;
            }
            Invoker.ExecuteCommands();
            InvokerMotion.StartExecution(this);
        }

    }
}