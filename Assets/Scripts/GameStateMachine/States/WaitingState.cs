using PositionerDemo;
using System.Collections.Generic;
using UnityEngine;

public class WaitingState : TimeConditionState
{
    private const string name = "ADMINISTRATION";
    List<AbilityAction> UnfinishActions;
    State nextState;

    public WaitingState(int duration, GameCreator gameCreator, List<AbilityAction> UnfinishActions, State nextState) : base(duration, gameCreator, name)
    {
        this.UnfinishActions = UnfinishActions;
        this.nextState = nextState;
    }

    public override void Enter()
    {
        base.Enter();
        //GameCreator.Instance.turnManager.ChangeTurn(managmentPoints);
        //Debug.Log("Enter Waiting State ");
    }

    public override void Exit()
    {
        base.Exit();
        //Debug.Log("exiiiiiiiiiiittttttt ");
    }

    public override bool CheckCondition()
    {
        for (int i = 0; i < UnfinishActions.Count; i++)
        {
            if (UnfinishActions[i].actionStatus == ABILITYEXECUTIONSTATUS.STARTED)
            {
                return false;
            }
        }

        return true;
    }

    public override void GetBack()
    {
        base.GetBack();
    }

    public override State Update()
    {
        base.Update();

        if (CheckCondition())
        {
            //Debug.Log("VOLVEMOS AL STATE QUE ESTABAMOS State ");
            return nextState;
        }
        else
        {
            return null;
        }
    }
}
