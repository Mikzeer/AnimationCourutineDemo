using StateMachinePattern;

namespace PositionerDemo
{
    public interface IGame
    {
        SpawnManager spawnManager { get; }
        Board2DManager board2DManager { get; }
        CombineManager combineManager { get; }
        MovementManager movementManager { get; }
        TurnController turnController { get; }
        PlayerManager playerManager { get; }
        CardController cardManager { get; }
        BaseStateMachine baseStateMachine { get; }
        ActionsManager actionsManager { get; }
    }
}