using CommandPatternActions;
using PositionerDemo;
using StateMachinePattern;
using System.Collections;
using System.Collections.Generic;
using UIButtonPattern;
using UnityEngine;

public abstract class SelectionState<T> : State
{
    List<T> posibleSelectionTargets;
    public IGame game { get; private set; }
    public abstract GameCreator gameCreator { get; }
    public abstract bool endState { get; set; }

    public State previousState;

    public SelectionState(State previousState, IGame game)
    {
        this.game = game;
        this.previousState = previousState;
    }

    public virtual void SetTargets(List<T> posibleTargets)
    {
        posibleSelectionTargets = posibleTargets;
        // MOVE => Pinto Las Tiles
        // ATTACK => Selecciono las tiles a atacar y marco a los enemigos como van a quedar de vida ?
        // SPAWN => Pinto las tiles y marco a las unidades combinables
    }

    public virtual void SetSelection(T selection)
    {
        // MOVE/ATTACK/COMBINE/SPAWN Si esta dentro de las posibles Ejecuto, sino Cancelo
        // DECOMBINE Si esta dentro de las posibles y no esta esa tile y la lista cumplio con lo requerido Ejecuto, sino Cancelo
        // USE CARD // SELECT THE CARD TARGETS
    }

    public virtual void Enter()
    {

    }

    public virtual void Exit()
    {

    }

    public virtual void GetBack()
    {

    }

    public virtual bool HaveReachCondition()
    {
        return true;
    }

    public State Update()
    {
        return previousState.Update();
    }

    public abstract void OnTileSelection(Tile tile);
}

namespace StateMachinePattern
{
    public interface IStateMachineHandler
    {
        GameObject GetGameObject();
    }

    public class BaseStateMachine
    {
        public bool IsInitialized { get; protected set; }

        // STACK DE TODOS LOS STATES QUE TENEMOS ACTUALMENTE
        private readonly Stack<IState> stack = new Stack<IState>();
        /// <summary>
        ///     Handler for the FSM. Usually the Monobehavior which holds this FSM.
        /// </summary>
        public IStateMachineHandler Handler { get; set; }

        public IState currentState => PeekState();


        public BaseStateMachine(IStateMachineHandler Handler = null)
        {
            this.Handler = Handler;
        }

        public void Update()
        {
            if (currentState != null)
            {
                currentState.OnUpdate();
            }
        }

        public IState PeekState()
        {
            if (stack.Count <= 0)
            {
                return null;
            }
            return stack.Peek();
            //return stack.Count > 0 ? stack.Peek() : null;
        }

        public void Initialize()
        {
            currentState.OnEnter();
            IsInitialized = true;
        }

        public void PushState(IState state, bool isSilent = false)
        {
            stack.Push(state);
            if (isSilent == false)
            {
                state.OnEnter();
            }
        }

        public void PopState()
        {
            if (currentState == null)
                return;

            var state = stack.Pop();
            state.OnExit();
        }

        public void ChangeAndEnterState(IState nextState)
        {
            PopState();
            PushState(nextState, false);
        }

        public void FinishActualState()
        {
            PopState();
        }
    }

    public interface IState
    {
        void OnEnter();
        void OnExit();
        void OnUpdate();
        bool HaveReachCondition();
        void OnClear();
        void OnNextState(IState nextState);
    }

    public abstract class BaseState : IState
    {
        protected IGame game { get; private set; }

        public BaseState(IGame game)
        {
            this.game = game;
        }

        public virtual bool HaveReachCondition()
        {
            return true;
        }

        public virtual void OnClear()
        {

        }

        public virtual void OnEnter()
        {

        }

        public virtual void OnExit()
        {

        }

        public void OnNextState(IState nextState)
        {
            // ACA DEBERIA REVISAR SI EL INVOKER MOTION ESTA EJECUTANDO...
            if (InvokerMotion.IsExecuting())
            {
                game.baseStateMachine.PopState();
                var wait = new WaitEndAnimationState(game, nextState);
                game.baseStateMachine.PushState(wait);
            }
            else
            {
                game.baseStateMachine.ChangeAndEnterState(nextState);
            }
        }

        public virtual void OnUpdate()
        {

        }
    }

    public abstract class TimeState : BaseState
    {
        protected GameTimer gameTimer;

        public TimeState(int duration, IGame game) : base(game)
        {
            gameTimer = new GameTimer(duration);
        }

        public override void OnEnter()
        {
            gameTimer.Start();
        }

        public override void OnExit()
        {
            gameTimer.Stop();
        }

        public override void OnUpdate()
        {
            gameTimer.RestTime();
        }
    }

    public abstract class ActionState<T> : TimeState
    {
        public ActionState(IGame game, int duration) : base(duration, game)
        {
        }

        public virtual void ExecuteAction(T action)
        {

        }
    }

    public abstract class SubSelectionState<T> : BaseState
    {
        #region blablabla
        // SELECT => SPAWN / ATTACK / COMBINE / DECOMBINE / MOVE / USE CARD
        // NORMAL STATE => TURN STATE / ADMIN STATE
        // SPECIAL STATE => WAIT FOR ANIMATION STATE // WAIT STATE // ABILITY RESOLUTION STATE
        // CHAIN STATE => SELECT => CHAIN START STATE 
        //             => SELECT => CHAIN CARD SELECTION STATE
        //             => SPECIAL => CHAIN DEFINITION STATE

        // SI ES UN SUBSTATE DE TIPO SPAWN VAMOS A SELECCIONAR LAS TILES
        // SI ES UN SUBSTATE DE TIPO ATTACK VAMOS A SELECCIONAR A LOS TARGETS O A LAS TILES
        // LA SELECCION VA A VENIR DEL CLICK... Y EL CLICK VA A TIRAR UN EVENTO QUE SIEMPRE ES DE TIPO TILE
        // ENTONCES LA SELECCION PUEDE SER DE TIPO TILE O DE TIPO NULL
        // EL CLICK EN LA UI NO DEVUELVE NADA NI GENERA EL EVENTO
        #endregion

        public IState previousState;
        List<T> posibleSelectionTargets;
        List<T> selectedTargets;

        public SubSelectionState(IGame game, IState previousState) : base(game)
        {
            this.previousState = previousState;
        }

        public virtual void SetTargets(List<T> posibleTargets)
        {
            posibleSelectionTargets = posibleTargets;
            // MOVE => Pinto Las Tiles
            // ATTACK => Selecciono las tiles a atacar y marco a los enemigos como van a quedar de vida ?
            // SPAWN => Pinto las tiles y marco a las unidades combinables
        }

        public virtual void SetSelection(T selection)
        {
            // MOVE/ATTACK/COMBINE/SPAWN Si esta dentro de las posibles Ejecuto, sino Cancelo
            // DECOMBINE Si esta dentro de las posibles y no esta esa tile y la lista cumplio con lo requerido Ejecuto, sino Cancelo
            // USE CARD // SELECT THE CARD TARGETS
        }

        public override void OnUpdate()
        {
            previousState.OnUpdate();
        }
    }

    public class WaitEndAnimationState : BaseState
    {
        IState nextState;

        public WaitEndAnimationState(IGame game, IState nextState) : base(game)
        {
            this.nextState = nextState;
        }

        public override void OnUpdate()
        {
            if (HaveReachCondition())
            {
                OnNextState(nextState);
            }
        }

        public override bool HaveReachCondition()
        {
            if (InvokerMotion.IsExecuting())
            {
                return false;
            }
            return true;
        }
    }

    public class AdministrationState : ActionState<Tile>
    {
        private const string name = "ADMINISTRATION";
        private int managmentPoints;

        UIInputButtonPattern uiInputButtonPattern;
        protected GameMachine gmMachine;
        public AdministrationState(int duration, GameMachine game, int managmentPoints) : base(game, duration)
        {
            this.managmentPoints = managmentPoints;
            this.gmMachine = game;
            //gmMachine.tileSelectionManagerUI.onTileSelected += ExecuteAction;
        }

        public override void OnEnter()
        {
            // 2 - TENGO QUE SETEAR LOS ACTIONS POINTS PARA ESTE JUGADOR
            game.actionsManager.IncrementPlayerActions(game.turnController.CurrentPlayerTurn, managmentPoints);

            // COMENZAMOS EL CONTADOR DE TIEMPO
            base.OnEnter();


            //game.tileSelectionManagerUI.onTileSelected += ExecuteAction;
            // ACA TENGO QUE PRENDER EL BOTON DE CARD
            //CreateButtonSelectionControl();
            //uiInputButtonPattern.Suscribe();
            //gameCreator.TakeCardAvailable(true);
            //Debug.Log("Enter Administration State Player " + GameCreator.Instance.turnManager.GetActualPlayerTurn().PlayerID);
        }

        public override void OnExit()
        {
            // ACA TENGO QUE APAGAR EL BOTON DE CARD
            //gameCreator.TakeCardAvailable(false);
            // DETENEMOS EL TIEMPO
            base.OnExit();

            //uiInputButtonPattern.Unsuscribe();

            // RESTAMOS LAS ACCIONES DEL PLAYER PARA QUE NO PUEDE HACER NADA MAS
            game.actionsManager.RestPlayerActions(game.turnController.CurrentPlayerTurn);

            // CAMBIAMOS EL TURNO AL OTRO JUGADOR
            game.turnController.ChangeCurrentRound();
        }

        public override bool HaveReachCondition()
        {
            // SIN ACTION POINTS A UTILIZAR
            if (game.turnController.CurrentPlayerTurn.GetCurrentActionPoints() <= 0)
            {
                return true;
            }

            // SIN ACCIONES A EJECUTAR
            if (game.actionsManager.DoesThePlayerHaveActionToExecute(game.turnController.CurrentPlayerTurn) == false)
            {
                return true;
            }

            // END TIME
            if (gameTimer.running == false)
            {
                return true;
            }

            // CHEQUEO ESPECIAL, SI NO TENGO CARDS, NI HAY UN ENEMIGO EN BASE, NI HAY UNA TILE POSIBLE PARA SPAWNEAR
            bool havaCardsInDeck = game.turnController.CurrentPlayerTurn.Deck.Count > 0;
            bool isAnEnemyInBase = game.board2DManager.IsThereAPosibleAttackableEnemyInBase(game.turnController.CurrentPlayerTurn.PlayerID);
            bool isThereAPosibleSpawnTile = game.board2DManager.IsThereAPosibleSpawneableTile(game.turnController.CurrentPlayerTurn.PlayerID); ;

            if (!havaCardsInDeck && !isAnEnemyInBase && !isThereAPosibleSpawnTile)
            {
                return true;
            }

            return false;
        }

        public override void OnUpdate()
        {
            // UPDATEO EL TIEMPO... PODRIA TENER ESTO EN UN MONO BEHAVIOUR PEEEEEEROOOOOO.... NO SE
            base.OnUpdate();


            if (HaveReachCondition())
            {
                AdministrationState adminState = new AdministrationState(20, gmMachine, 1);
                OnNextState(adminState);
            }

            //if (HaveReachCondition())
            //{
            //    State nextState = new AdministrationState(15, gameCreator, 2);
            //    gameCreator.highLightTile.OnTileSelection(null, gameCreator.turnManager.GetActualPlayerTurn());
            //    return gameCreator.turnManager.ChangeTurnState(managmentPoints, nextState);
            //    //return nextState;
            //}
        }

        public override void ExecuteAction(Tile action)
        {
            game.spawnManager.OnTrySpawn(action, game.turnController.CurrentPlayerTurn);
            Invoker.ExecuteCommands();
            InvokerMotion.StartExecution(gmMachine);
        }

        public void CreateButtonSelectionControl()
        {
            CardButtonEventFire cardButtonEvent = new CardButtonEventFire();
            SpecificCardButtonExecution cardButtonExecution = new SpecificCardButtonExecution(game.turnController.CurrentPlayerTurn, game.cardManager);
            ButtonAndEventContainer cardButtonPartners = new ButtonAndEventContainer(cardButtonEvent, cardButtonExecution);

            //EndTurnButtonEventFire EndTurnButtonEvent = new EndTurnButtonEventFire();
            //SpecificEndTurnButtonExecution EndTurnButtonExecution = new SpecificEndTurnButtonExecution(game.turnController);
            //ButtonAndEventContainer cardButtonPartners = new ButtonAndEventContainer(EndTurnButtonEvent, EndTurnButtonExecution);

            List<ButtonAndEventContainer> buttonPartners = new List<ButtonAndEventContainer>();
            buttonPartners.Add(cardButtonPartners);
            uiInputButtonPattern = new UIInputButtonPattern(buttonPartners);
        }
    }

    public class InitialAdministrationStateA : AdministrationState
    {
        public InitialAdministrationStateA(int duration, GameMachine gmMachine, int mngPoints) : base(duration, gmMachine, mngPoints)
        {
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (HaveReachCondition())
            {
                InitialAdministrationStateB adminState = new InitialAdministrationStateB(20, gmMachine, 2);
                OnNextState(adminState);
            }
        }
    }

    public class InitialAdministrationStateB : AdministrationState
    {
        public InitialAdministrationStateB(int duration, GameMachine gmMachine, int mngPoints) : base(duration, gmMachine, mngPoints)
        {
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (HaveReachCondition())
            {
                InitialAdministrationStateC adminState = new InitialAdministrationStateC(20, gmMachine, 2);
                OnNextState(adminState);
            }
        }
    }

    public class InitialAdministrationStateC : AdministrationState
    {
        public InitialAdministrationStateC(int duration, GameMachine gmMachine, int mngPoints) : base(duration, gmMachine, mngPoints)
        {
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (HaveReachCondition())
            {
                InitialAdministrationStateD adminState = new InitialAdministrationStateD(40, gmMachine, 4);
                OnNextState(adminState);
            }
        }
    }

    public class InitialAdministrationStateD : AdministrationState
    {
        public InitialAdministrationStateD(int duration, GameMachine gmMachine, int mngPoints) : base(duration, gmMachine, mngPoints)
        {
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (HaveReachCondition())
            {
                AdministrationState adminState = new AdministrationState(20, gmMachine, 1);
                OnNextState(adminState);
            }
        }
    }

    public class SpawnState : SubSelectionState<Tile>
    {
        public SpawnState(GameMachine game, IState previousState) : base(game, previousState)
        {
            game.tileSelectionManagerUI.onTileSelected += SetSelection;
        }

        public override void SetSelection(Tile selection)
        {
            game.spawnManager.OnTrySpawn(selection, game.turnController.CurrentPlayerTurn);
            OnNextState(previousState);
        }
    }
}

namespace AbilitySelectionUI
{
    public enum ABILITYSELECTIONTYPE
    {
        SIMPLE,
        MULTIPLE
    }

    public enum HIGHLIGHTUITYPE
    {
        SPAWN,
        ATTACK,
        MOVE,
        COMBINE,
        DECOMBINE,
        BUFF,
        NERF,
        NEUTRAL,
        NONE
    }

    public class AbilitySelectinUIContainer
    {
        ABILITYSELECTIONTYPE selectionType;
        Dictionary<Tile, HIGHLIGHTUITYPE> tileHighlightTypesDictionary;
        bool canSelectSameTarget;
        int totalSelectionRequirement;
        IOcuppy abilityExecuter;
    }
}
