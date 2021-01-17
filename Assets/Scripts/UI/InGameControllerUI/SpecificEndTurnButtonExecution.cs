namespace UIButtonPattern
{
    public class SpecificEndTurnButtonExecution : SpecificButtonExecution
    {
        bool hasPresed;
        TurnController turnController;
        public SpecificEndTurnButtonExecution(TurnController turnController)
        {
            this.turnController = turnController;
        }

        public void Execute()
        {
            if (!hasPresed)
            {
                turnController.ChangeCurrentRound();
                hasPresed = true;
            }
        }
    }
}
