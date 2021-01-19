using PositionerDemo;

public abstract class SubState : State
{
    public GameCreator gameCreator { get; private set; }
    public bool endState { get; set; }
    public State previousState;

    public SubState(State previousState, GameCreator gsMachine)
    {
        this.gameCreator = gsMachine;
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

    public virtual bool CheckCondition()
    {
        return true;
    }

    public virtual void OnTileSelection(Tile tile)
    {

    }

}