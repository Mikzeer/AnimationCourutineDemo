using PositionerDemo;
namespace CommandPatternActions
{
    public class ISpawnCommand : ICommand
    {
        public COMMANDEXECUTINSTATE executionState { get; set; }
        public bool isRunning { get; set; }
        public bool logInsert { get; set; }

        Tile TileObject;
        Player player;
        Kimboko kimboko;
        public ISpawnCommand(Tile TileObject, Player player, Kimboko kimboko)
        {
            logInsert = true;
            this.TileObject = TileObject;
            this.player = player;
            this.kimboko = kimboko;
        }

        public void Execute()
        {
            isRunning = true;
            TileObject.OcupyTile(kimboko);
            player.AddUnit(kimboko);
            executionState = COMMANDEXECUTINSTATE.FINISH;
            isRunning = false;
        }

        public void Unexecute()
        {
            TileObject.Vacate();
            player.RemoveUnit(kimboko);
            kimboko.DestroyPrefab();
        }
    }
}