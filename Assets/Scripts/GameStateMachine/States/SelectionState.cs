using CommandPatternActions;
using PositionerDemo;
using System.Collections;
using System.Collections.Generic;
using UIButtonPattern;
using UnityEngine;

public abstract class SelectionState<T> : Statee
{
    List<T> posibleSelectionTargets;
    public IGame game { get; private set; }
    public Statee previousState;

    public SelectionState(Statee previousState, IGame game)
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

    public virtual void Update()
    {
        previousState.Update();
    }

    public virtual void GetBack()
    {

    }

    public virtual bool HaveReachCondition()
    {
        return true;
    }


}

public interface Statee
{
    void Enter();
    void Update();
    void Exit();
    IGame game { get; }
    bool HaveReachCondition();
}

public abstract class TimeConditionStatee<T> : Statee
{
    public IGame game { get; private set; }
    public int duration;
    string actualStateName;

    public GameTimer gameTimer;

    public TimeConditionStatee(int duration, IGame game, string actualStateName)
    {
        this.game = game;
        this.duration = duration;
        this.actualStateName = actualStateName;
        gameTimer = new GameTimer();
    }

    public virtual void Enter()
    {
        gameTimer.Start(duration);
    }

    public virtual void Exit()
    {
        gameTimer.Stop();
    }

    public virtual void Update()
    {
        gameTimer.RestTime();        
    }

    public virtual void GetBack()
    {
    }

    public virtual bool HaveReachCondition()
    {
        return true;
    }

    public virtual void ExecuteAction(T action)
    {

    }
}

public class CreationState : Statee
{
    public IGame game { get; private set; }
    MonoBehaviour dummy;
    //bool isCardCollectionLoaded = false;
    //bool isBoardLoaded = false;
    public CreationState(IGame game, MonoBehaviour dummy)
    {
        this.game = game;
        this.dummy = dummy;
    }

    public bool HaveReachCondition()
    {
        return true;
    }

    public void Enter()
    {
    }

    public void Exit()
    {
    }

    public void Update()
    {
        if (HaveReachCondition() == false) return;

        State nextState = new InitialAdministrationState(40, GameCreator.Instance, 4, 0);
        return;
    }


}

public class AdministrationStatee : TimeConditionStatee<Tile>
{
    private const string name = "ADMINISTRATION";
    private int managmentPoints;

    UIInputButtonPattern uiInputButtonPattern;

    public AdministrationStatee(int duration, IGame game, int managmentPoints) : base(duration, game, name)
    {
        this.managmentPoints = managmentPoints;
    }

    public override void Enter()
    {        



        // 2 - TENGO QUE SETEAR LOS ACTIONS POINTS PARA ESTE JUGADOR
        game.actionsManager.IncrementPlayerActions(game.turnController.CurrentPlayerTurn, managmentPoints);

        // COMENZAMOS EL CONTADOR DE TIEMPO
        base.Enter();


        //game.tileSelectionManagerUI.onTileSelected += ExecuteAction;
        // ACA TENGO QUE PRENDER EL BOTON DE CARD
        //CreateButtonSelectionControl();
        //uiInputButtonPattern.Suscribe();
        //gameCreator.TakeCardAvailable(true);
        //Debug.Log("Enter Administration State Player " + GameCreator.Instance.turnManager.GetActualPlayerTurn().PlayerID);
    }

    public override void Exit()
    {
        // ACA TENGO QUE APAGAR EL BOTON DE CARD
        //gameCreator.TakeCardAvailable(false);
        // DETENEMOS EL TIEMPO
        base.Exit();

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

    public override void GetBack()
    {
        base.GetBack();
    }

    public override void Update()
    {
        // UPDATEO EL TIEMPO... PODRIA TENER ESTO EN UN MONO BEHAVIOUR PEEEEEEROOOOOO.... NO SE
        base.Update();


        if (HaveReachCondition())
        {
            game.baseStateMachine.FinishActualState();
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
        //game.spawnManager.OnTrySpawn(action, game.turnController.CurrentPlayerTurn);

        //Invoker.ExecuteCommands();
        //InvokerMotion.StartExecution(game);
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

public interface IStateMachineHandler
{
}

public class BaseStateMachine
{
    public bool IsInitialized { get; protected set; }

    // STACK DE TODOS LOS STATES QUE TENEMOS ACTUALMENTE
    private readonly Stack<Statee> stack = new Stack<Statee>();
    /// <summary>
    ///     Handler for the FSM. Usually the Monobehavior which holds this FSM.
    /// </summary>
    public IStateMachineHandler Handler { get; set; }

    public Statee currentState => PeekState();


    public BaseStateMachine(IStateMachineHandler Handler = null)
    {
        this.Handler = Handler;
    }

    public void Update()
    {
        if (currentState != null)
        {
            currentState.Update();
        }
    }

    public Statee PeekState()
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
        currentState.Enter();
        IsInitialized = true;
    }

    public void PushState(Statee state, bool isSilent = false)
    {
        stack.Push(state);
        if (isSilent == false)
        {
            state.Enter();
        }
    }

    private void PopState()
    {
        if (currentState == null)
            return;

        var state = stack.Pop();
        state.Exit();
    }

    public void ChangeState(Statee nextState, bool isSilent = false)
    {
        PopState();
        PushState(nextState, isSilent);
    }

    public void FinishActualState()
    {
        PopState();
    }
}