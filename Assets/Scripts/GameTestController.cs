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

            CreatePlayers();
            Motion motion = board2DManager.CreateBoard(players, OnBoardComplete);
            ReproduceMotion(motion);
            spawnManager = new SpawnManager(spawnManagerUI);
        }

        private void CreatePlayers()
        {
            Player playerOne = new Player(0);
            playerOne.SetStatsAndAbilities(CreatePlayerAbilities(playerOne), CreatePlayerStat());

            Player playerTwo = new Player(1);
            playerTwo.SetStatsAndAbilities(CreatePlayerAbilities(playerTwo), CreatePlayerStat());

            // DEBERIAMOS TENER UN ABILITYMODIFIER MANAGER O ALGO SIMILIAR PARA ENCARGARSE DE ESTO TAL VEZ
            ChangeUnitClassAbilityModifier ab = new ChangeUnitClassAbilityModifier(playerOne);
            SpawnAbility spw = (SpawnAbility)playerOne.Abilityes[ABILITYTYPE.SPAWN];
            Invoker.AddNewCommand(new IAddAbilityActionModifierCommand(spw, ab));
            CanceclSpawnAbilityModifier cancelSpawn = new CanceclSpawnAbilityModifier(playerOne);
            Invoker.AddNewCommand(new IAddAbilityActionModifierCommand(spw, cancelSpawn));

            Invoker.ExecuteCommands();


            players = new Player[2];
            players[0] = playerOne;
            players[1] = playerTwo;
        }

        private Dictionary<STATTYPE, Stat> CreatePlayerStat()
        {
            Dictionary<STATTYPE, Stat> Stats = new Dictionary<STATTYPE, Stat>();
            AttackPowerStat attackPow = new AttackPowerStat(2, 2);
            AttackRangeStat attackRan = new AttackRangeStat(1, 3);
            HealthStat healthStat = new HealthStat(2, 2);
            ActionPointStat actionPStat = new ActionPointStat(2, 2);

            Stats.Add(attackPow.StatType, attackPow);
            Stats.Add(attackRan.StatType, attackRan);
            Stats.Add(healthStat.StatType, healthStat);
            Stats.Add(actionPStat.StatType, actionPStat);

            return Stats;
        }

        private Dictionary<ABILITYTYPE, AbilityAction> CreatePlayerAbilities(Player player)
        {
            Dictionary<ABILITYTYPE, AbilityAction> Abilities = new Dictionary<ABILITYTYPE, AbilityAction>();
            SpawnAbility spawnAbility = new SpawnAbility(player);
            TakeCardAbility takeCardAbility = new TakeCardAbility(player);
            Abilities.Add(spawnAbility.AbilityType, spawnAbility);
            Abilities.Add(takeCardAbility.AbilityType, takeCardAbility);
            return Abilities;
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
                    spawnManager.OnTrySpawn(tile, players[0]);
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

    public class IAddAbilityActionModifierCommand : ICommand
    {
        public COMMANDEXECUTINSTATE executionState { get; set; }
        public bool isRunning { get; set; }
        public bool logInsert { get; set; }

        AbilityAction ability;
        AbilityModifier abilityModifier;

        public IAddAbilityActionModifierCommand(AbilityAction ability, AbilityModifier abilityModifier)
        {
            this.ability = ability;
            this.abilityModifier = abilityModifier;
        }

        public void Execute()
        {
            isRunning = true;

            ability.AddAbilityModifier(abilityModifier);

            executionState = COMMANDEXECUTINSTATE.FINISH;
            isRunning = false;
        }

        public void Unexecute()
        {
            ability.RemoveAbilityModifier(abilityModifier);
        }
    }

    public class IApplyModifierCommand : ICommand
    {
        public COMMANDEXECUTINSTATE executionState { get; set; }
        public bool isRunning { get; set; }
        public bool logInsert { get; set; }

        AbilityAction ability;
        AbilityModifier abilityModifier;

        public IApplyModifierCommand(AbilityAction ability, AbilityModifier abilityModifier)
        {
            this.ability = ability;
            this.abilityModifier = abilityModifier;
        }

        public void Execute()
        {
            isRunning = true;

            abilityModifier.Execute(ability);

            executionState = COMMANDEXECUTINSTATE.FINISH;
            isRunning = false;
        }

        public void Unexecute()
        {
            //ability.RemoveAbilityModifier(abilityModifier);
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