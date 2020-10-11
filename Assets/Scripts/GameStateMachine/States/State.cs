public interface State
{
    void Enter();
    State Update();
    void Exit();
    void GetBack();
    GameCreator gameCreator { get; }
    bool endState { get; set; }
}