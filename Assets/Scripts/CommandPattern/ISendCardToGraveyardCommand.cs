using PositionerDemo;

namespace CommandPatternActions
{
    public class ISendCardToGraveyardCommand : ICommand
    {
        public COMMANDEXECUTINSTATE executionState { get; set; }
        public bool logInsert { get; set; }
        Card card;
        Player player;
        public ISendCardToGraveyardCommand(Player player, Card card)
        {
            this.player = player;
            this.card = card;
        }

        public void Execute()
        {
            player.PlayersHands.Remove(card);
            player.Graveyard.Add(card);
        }

        public void Unexecute()
        {
            player.Graveyard.Remove(card);
            player.PlayersHands.Add(card);
        }
    }
}