using PositionerDemo;
namespace CommandPatternActions
{
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