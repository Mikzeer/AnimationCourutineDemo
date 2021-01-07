using CommandPatternActions;
using PositionerDemo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PositionerDemo
{
    public class GameTestController : MonoBehaviour
    {
        MotionController motionController = new MotionController();

        [SerializeField] private SpawnManagerUI spawnManagerUI;
        SpawnManager spawnManager;

        [SerializeField] private Board2DManagerUI board2DManagerUI;
        Board2DManager board2DManager;

        [SerializeField] private TileSelectionManagerUI tileSelectionManagerUI;

        MouseController mouseController;

        Player[] players;

        private void Start()
        {
            board2DManager = new Board2DManager(board2DManagerUI, 5, 7);

            Player playerOne = new Player(0, PLAYERTYPE.PLAYER);
            Player playerTwo = new Player(1, PLAYERTYPE.PLAYER);
            players = new Player[2];
            players[0] = playerOne;
            players[1] = playerTwo;
            Motion motion = board2DManager.CreateBoard(players, OnBoardComplete);
            ReproduceMotion(motion);
            spawnManager = new SpawnManager(spawnManagerUI);
        }

        private void OnBoardComplete()
        {
            Debug.Log("BOARD SE TERMINO DE CREAR");
            mouseController = new MouseController(0, board2DManager, Camera.main);
            tileSelectionManagerUI.SetController(board2DManager, mouseController);
        }

        private void ReproduceMotion(Motion motion)
        {
            MotionController motionController = new MotionController();
            motionController.SetUpMotion(motion);
            motionController.TryReproduceMotion();
        }

        private void Update()
        {
            if (mouseController == null) return;

            if (mouseController.Select())
            {
                Tile tile = mouseController.GetTile();
                if (tile != null)
                {
                    spawnManager.Spawn(tile, players[0], 0);
                    Invoker.ExecuteCommands();
                    InvokerMotion.StartExecution(this);
                }
            }
        }
    }
}
namespace CommandPatternActions
{

    public interface ICommand
    {
        COMMANDEXECUTINSTATE executionState { get; set; }
        PositionerDemo.Motion motionToExecute { get; set; }
        bool isRunning { get; set; }
        bool logInsert { get; set; } // Define si lo podemos guardar como un cmd desejecutable
        void Execute();
        void Unexecute();
    }

    public enum COMMANDEXECUTINSTATE
    {
        WAITFOREXECUTION,
        EXECUTE,
        FINISH,
        ABORT,
        CANCEL
    }

    public class ISpawnCommand : ICommand
    {
        public COMMANDEXECUTINSTATE executionState { get; set; }
        public bool isRunning { get; set; }
        public bool logInsert { get; set; }
        public PositionerDemo.Motion motionToExecute { get; set; }

        Tile TileObject;
        Player player;
        Kimboko kimboko;
        public ISpawnCommand(Tile TileObject, Player player, Kimboko kimboko)
        {
            logInsert = true;
            this.TileObject = TileObject;
            this.player = player;
            this.kimboko = kimboko;
        }

        public void Execute()
        {
            isRunning = true;
            TileObject.OcupyTile(kimboko);
            player.AddUnit(kimboko);
            executionState = COMMANDEXECUTINSTATE.FINISH;
            isRunning = false;
        }

        public void Unexecute()
        {
            TileObject.Vacate();
            player.RemoveUnit(kimboko);
            kimboko.DestroyPrefab();
        }
    }

    public static class Invoker
    {
        private static List<ICommand> commandsToExecute = new List<ICommand>();
        private static Stack<ICommand> executedCmd = new Stack<ICommand>();

        public static void AddNewCommand(ICommand command)
        {
            commandsToExecute.Add(command);
        }

        public static void RemoveCommand(ICommand command)
        {
            commandsToExecute.Remove(command);
        }

        public static void ExecuteCommands()
        {
            for (int i = 0; i < commandsToExecute.Count; i++)
            {
                if (commandsToExecute[i].isRunning) continue;

                switch (commandsToExecute[i].executionState)
                {
                    case COMMANDEXECUTINSTATE.WAITFOREXECUTION:
                        commandsToExecute[i].Execute();
                        commandsToExecute[i].executionState = COMMANDEXECUTINSTATE.EXECUTE;
                        break;
                    case COMMANDEXECUTINSTATE.EXECUTE: // NO ESTA ESPERANDO Y SE ESTA EJECUTANDO... POR LAS DUDAS
                        continue;
                    case COMMANDEXECUTINSTATE.FINISH:
                        if (commandsToExecute[i].logInsert == true) executedCmd.Push(commandsToExecute[i]);
                        commandsToExecute.Remove(commandsToExecute[i]);
                        i--;
                        break;
                    case COMMANDEXECUTINSTATE.ABORT:
                        commandsToExecute[i].Unexecute();
                        commandsToExecute.Remove(commandsToExecute[i]);
                        i--;
                        break;
                    case COMMANDEXECUTINSTATE.CANCEL:
                        commandsToExecute[i].Unexecute();
                        commandsToExecute.Remove(commandsToExecute[i]);
                        i--;
                        break;
                    default:
                        continue;
                }
            }

            // Mientras tengamos cmd en la lista para ejecutar vamos a seguir ejecutandolos hasta que terminen
            // esto puede trabar potencialmente la ejecucion del programa...
            //if (commandsToExecute.Count > 0) ExecuteCommands();
        }

        public static void UnexecuteLastCommand()
        {
            if (executedCmd.Count > 0)
            {
                ICommand cmdAux = executedCmd.Pop();
                cmdAux.Unexecute();
            }
        }
    }

    public static class InvokerMotion
    {
        private static List<PositionerDemo.Motion> motionsToExecute = new List<PositionerDemo.Motion>();
        public static bool isExecuting;
        private static MotionController motionController = new MotionController();

        public static void AddNewMotion(PositionerDemo.Motion motion)
        {
            motionsToExecute.Add(motion);
            Debug.Log("NEW MOTION ADDED");
        }

        public static void RemoveMotion(PositionerDemo.Motion motion)
        {
            motionsToExecute.Remove(motion);
            Debug.Log("MOTION REMOVED");
        }

        public static void StartExecution(MonoBehaviour dummy)
        {
            if (isExecuting)
            {
                Debug.Log("IS EXECUTION");
                return;
            }
            dummy.StartCoroutine(ExecuteMotion());
        }

        public static IEnumerator ExecuteMotion()
        {
            isExecuting = true;

            while (isExecuting)
            {
                // SI ESTA EJECUTANDO ENTONCES ESPERAMOS
                if (motionController.IsPerforming)
                {
                    Debug.Log("IS PERFORMING");
                    yield return null;
                }
                else
                {
                    // SI NO HAY MAS MOTIONS QUE EXECUTE ENTONCES TERMINAMOS
                    if (motionsToExecute.Count == 0)
                    {
                        Debug.Log("MOTION HAS FINISH");
                        isExecuting = false;
                    }
                    else
                    {
                        // SI NO ESTA EJECUTANDO, EJECUTAMOS
                        PositionerDemo.Motion motionToExecute = motionsToExecute[0];
                        ReproduceMotion(motionToExecute);
                        RemoveMotion(motionsToExecute[0]);
                        yield return null;
                    }
                }                
            }
            Debug.Log("MOTION HAS FINISH");

            if (motionsToExecute.Count > 0)
            {
                Debug.Log("SE AGREGO UNA MOTION DESPUES DE FINALIZAR");
            }
        }

        private static void ReproduceMotion(PositionerDemo.Motion motion)
        {
            motionController.SetUpMotion(motion);
            motionController.TryReproduceMotion();
        }
    }
}