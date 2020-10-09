using PositionerDemo;

public class CreationState : EventConditionState
{
    private const string name = "CREATION";
    CreateGameCommonAction createGameCommonAction;

    public CreationState() : base(name)
    {
    }

    public override void Enter()
    {
        base.Enter();
        createGameCommonAction = new CreateGameCommonAction(End);
        //createGameCommonAction = new CreateGameCommonAction(Exit);
        createGameCommonAction.Perform();
    }

    private void End()
    {
        endState = true;
    }

    public override State Update()
    {
        if (endState == false)
        {
            return null;
        }
        else
        {
            State nextState = new InitialAdministrationState(40, GameCreator.Instance, 4, 0);
            return GameCreator.Instance.turnManager.ChangeTurnState(4, nextState);
            //return new InitialAdministrationState(40, GameCreator.Instance, 4, 0);
        }
    }

}