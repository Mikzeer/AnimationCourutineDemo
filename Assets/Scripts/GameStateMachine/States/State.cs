using PositionerDemo;

public interface State
{
    void Enter();
    State Update();
    void Exit();
    void GetBack();
    void OnTileSelection(Tile tile);
    GameCreator gameCreator { get; }
    bool endState { get; set; }
}