using System;
using UnityEngine;

public class EventConditionState : State
{
    public GameCreator gameCreator { get; set; }
    public bool endState { get; set; }
    public static event Action<string> OnStateChange;
    string actualState;

    public EventConditionState(string actualStateName)
    {
        this.actualState = actualStateName;
    }

    public virtual void Enter()
    {
        OnStateChange?.Invoke(actualState);
    }

    public void Exit()
    {
        Debug.Log("Exit EVENT CONDIOTION STATE");
        //endState = true;
    }

    public virtual void GetBack()
    {
    }

    public virtual State Update()
    {
        return null;
    }
}









