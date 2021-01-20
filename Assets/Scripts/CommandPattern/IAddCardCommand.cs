using PositionerDemo;

namespace CommandPatternActions
{
    public class IAddCardCommand : ICommand
    {
        public COMMANDEXECUTINSTATE executionState { get; set; }
        public bool logInsert { get; set; }
        Card card;
        Player player;
        public IAddCardCommand(Player player, Card card)
        {
            this.player = player;
            this.card = card;
        }

        public void Execute()
        {
            player.PlayersHands.Add(card);
        }

        public void Unexecute()
        {
            player.PlayersHands.Remove(card);
        }
    }
}