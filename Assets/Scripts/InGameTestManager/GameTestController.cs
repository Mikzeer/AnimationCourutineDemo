using CommandPatternActions;
using System.Collections.Generic;
using UnityEngine;

namespace PositionerDemo
{
    public class GameTestController : MonoBehaviour
    {
        public enum UPDATESTATE
        {
            ACTION,
            SELECTION
        }
        [Header("UPDATE STATE ACTION")]
        public UPDATESTATE updateState = UPDATESTATE.ACTION;

        [Header("SPAWN MANAGER UI")]
        [SerializeField] private SpawnManagerUI spawnManagerUI = default; // DE ESTA MANERA EVITAMOS EL WARNING EN EL INSPECTOR
        SpawnManager spawnManager;
        [Header("BOARD MANAGER UI")]
        [SerializeField] private Board2DManagerUI board2DManagerUI = default;
        Board2DManager board2DManager;
        [Header("TILE SELECTION MANAGER UI")]
        [SerializeField] private TileSelectionManagerUI tileSelectionManagerUI = default;
        [Header("TOGGLE CONTROLLER")]
        [SerializeField] private ToggleController toggleController = default;

        MouseController mouseController;
        Player[] players;       


        private void Start()
        {
            board2DManager = new Board2DManager(board2DManagerUI, 5, 7);

            CreatePlayers();
            Motion motion = board2DManager.CreateBoard(players, OnBoardComplete);
            InvokerMotion.AddNewMotion(motion);
            InvokerMotion.StartExecution(this);
            spawnManager = new SpawnManager(spawnManagerUI);
        }

        private void CreatePlayers()
        {
            Player playerOne = new Player(0);
            playerOne.SetStatsAndAbilities(OccupierAbilityDatabase.CreatePlayerAbilities(playerOne), OccupierStatDatabase.CreatePlayerStat());

            Player playerTwo = new Player(1);
            playerTwo.SetStatsAndAbilities(OccupierAbilityDatabase.CreatePlayerAbilities(playerTwo), OccupierStatDatabase.CreatePlayerStat());

            // DEBERIAMOS TENER UN ABILITYMODIFIER MANAGER O ALGO SIMILIAR PARA ENCARGARSE DE ESTO TAL VEZ
            //ChangeUnitClassAbilityModifier ab = new ChangeUnitClassAbilityModifier(playerOne);
            //SpawnAbility spw = (SpawnAbility)playerOne.Abilityes[ABILITYTYPE.SPAWN];
            //Invoker.AddNewCommand(new IAddAbilityActionModifierCommand(spw, ab));
            //CanceclSpawnAbilityModifier cancelSpawn = new CanceclSpawnAbilityModifier(playerOne);
            //Invoker.AddNewCommand(new IAddAbilityActionModifierCommand(spw, cancelSpawn));

            //Invoker.ExecuteCommands();
            

            players = new Player[2];
            players[0] = playerOne;
            players[1] = playerTwo;
        }
     
        private void OnBoardComplete()
        {
            Debug.Log("BOARD SE TERMINO DE CREAR");
            mouseController = new MouseController(0, board2DManager, Camera.main);
            tileSelectionManagerUI.SetController(board2DManager, mouseController);
        }

        private void Update()
        {
            if (mouseController == null) return;
            if (mouseController.Select())
            {
                Tile tile = mouseController.GetTile();
                switch (updateState)
                {
                    case UPDATESTATE.ACTION:
                        ExecuteActions(tile);
                        break;
                    case UPDATESTATE.SELECTION:
                        ExecuteSelection();
                        break;
                    default:
                        break;
                }
            }
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

        private void ExecuteSelection()
        {
            if (tileSelectionManagerUI.SelectedTilePlayerOne == null) return;
            Debug.Log("Selected Tile " + tileSelectionManagerUI.SelectedTilePlayerOne.PosX + "," + tileSelectionManagerUI.SelectedTilePlayerOne.PosY);
        }
    }
}