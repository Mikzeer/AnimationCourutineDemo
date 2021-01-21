using PositionerDemo;

namespace CommandPatternActions
{
    public class IMoveCommand : ICommand
    {
        public COMMANDEXECUTINSTATE executionState { get; set; }
        public bool logInsert { get; set; }

        MoveAbilityEventInfo movInfo;
        IGame game;

        public IMoveCommand(MoveAbilityEventInfo movInfo, IGame game)
        {
            this.movInfo = movInfo;
            this.game = game;
        }

        public void Execute()
        {
            movInfo.fromTile.Vacate();
            movInfo.endPosition.OcupyTile(movInfo.moveOccupy);
            PositionerDemo.Position pos = new PositionerDemo.Position(movInfo.endPosition.position.posX, movInfo.endPosition.position.posY);
            game.board2DManager.AddModifyOccupierPosition(movInfo.moveOccupy, pos);
            movInfo.moveOccupy.SetPosition(pos);
        }

        public void Unexecute()
        {
            movInfo.endPosition.Vacate();
            movInfo.fromTile.OcupyTile(movInfo.moveOccupy);
            PositionerDemo.Position pos = new PositionerDemo.Position(movInfo.fromTile.position.posX, movInfo.fromTile.position.posY);
            game.board2DManager.AddModifyOccupierPosition(movInfo.moveOccupy, pos);
        }
    }
}