using PositionerDemo;

namespace UIButtonPattern
{
    public class SpecificCardButtonExecution : SpecificButtonExecution
    {
        Player player;
        CardManager cardManager;
        public SpecificCardButtonExecution(Player player, CardManager cardManager)
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
