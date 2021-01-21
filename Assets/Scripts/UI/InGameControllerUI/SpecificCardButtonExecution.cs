using PositionerDemo;

namespace UIButtonPattern
{
    public class SpecificCardButtonExecution : SpecificButtonExecution
    {
        Player player;
        CardController cardManager;
        public SpecificCardButtonExecution(Player player, CardController cardManager)
        {
            this.player = player;
            this.cardManager = cardManager;
        }

        public void Execute()
        {
            cardManager.OnTryTakeCard(player);
        }
    }
}
