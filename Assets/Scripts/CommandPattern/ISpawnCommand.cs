using PositionerDemo;
namespace CommandPatternActions
{
    public class ISpawnCommand : ICommand
    {
        public COMMANDEXECUTINSTATE executionState { get; set; }
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
            TileObject.OcupyTile(kimboko);
            player.AddUnit(kimboko);
            executionState = COMMANDEXECUTINSTATE.FINISH;
        }

        public void Unexecute()
        {
            TileObject.Vacate();
            player.RemoveUnit(kimboko);
            kimboko.goAnimContainer.DestroyPrefab();
        }
    }

    public class ICombineCommand : ICommand
    {
        public COMMANDEXECUTINSTATE executionState { get; set; }
        public bool logInsert { get; set; }

        Player player;
        Kimboko kimboko;
        public ICombineCommand(Player player, Kimboko kimboko)
        {
            logInsert = true;
            this.player = player;
            this.kimboko = kimboko;
        }

        public void Execute()
        {
            player.AddUnit(kimboko);
            executionState = COMMANDEXECUTINSTATE.FINISH;
        }

        public void Unexecute()
        {
            player.RemoveUnit(kimboko);
            kimboko.goAnimContainer.DestroyPrefab();
        }
    }
}