using PositionerDemo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SelectionState<T> : SubStatee
{
    List<T> posibleSelectionTargets;

    public SelectionState(State previousState, IGame game) : base(previousState, game)
    {
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
}

public interface Statee
{
    void Enter();
    State Update();
    void Exit();
    IGame game { get; }
    bool HaveReachCondition();
}

public abstract class SubStatee : Statee
{
    public IGame game { get; private set; }
    public State previousState;

    public SubStatee(State previousState, IGame game)
    {
        this.game = game;
        this.previousState = previousState;
    }

    public virtual void Enter()
    {

    }

    public virtual void Exit()
    {

    }

    public virtual State Update()
    {
        return previousState.Update();
    }

    public virtual void GetBack()
    {

    }

    public virtual bool HaveReachCondition()
    {
        return true;
    }

}

public abstract class TimeConditionStatee : Statee
{
    public IGame game { get; private set; }
    public int duration;
    string actualStateName;

    public GameTimer gameTimer;

    public TimeConditionStatee(int duration, IGame game, string actualStateName)
    {
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

    public virtual State Update()
    {
        gameTimer.RestTime();

        return null;
    }

    public virtual void GetBack()
    {
    }

    public virtual bool HaveReachCondition()
    {
        return true;
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

    public State Update()
    {
        if (HaveReachCondition() == false) return null;

        State nextState = new InitialAdministrationState(40, GameCreator.Instance, 4, 0);
        return nextState;
    }


}
