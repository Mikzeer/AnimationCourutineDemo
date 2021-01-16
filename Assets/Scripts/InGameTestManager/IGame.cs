namespace PositionerDemo
{
    public interface IGame
    {
        SpawnManager spawnManager { get; }
        Board2DManager board2DManager { get; }
        CombineManager combineManager { get; }
    }
}