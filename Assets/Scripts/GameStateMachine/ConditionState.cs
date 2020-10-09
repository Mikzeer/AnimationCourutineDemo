using System;
using System.Collections.Generic;

public class ConditionState : State
{
    public GameCreator gameCreator { get; set; }
    public bool endState { get; set; }
    public bool conditionReach;
    public static event Action<string> OnStateChange;
    string actualState;
    
    public ConditionState(string actualStateName)
    {
        this.actualState = actualStateName;
    }

    public virtual void Enter()
    {
        OnStateChange?.Invoke(actualState);
    }

    public virtual void Exit()
    {

    }

    public virtual State Update()
    {
        return null;
    }

    public virtual void GetBack()
    {

    }

    public virtual bool CheckCondition()
    {
        if (conditionReach) return true;
        return false;
    }

}