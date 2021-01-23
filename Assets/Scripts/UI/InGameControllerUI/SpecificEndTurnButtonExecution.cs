using StateMachinePattern;

namespace UIButtonPattern
{
    public class SpecificEndTurnButtonExecution : SpecificButtonExecution
    {
        bool hasPresed;
        TurnState turnState;
        public SpecificEndTurnButtonExecution(TurnState turnState)
        {
            this.turnState = turnState;
        }
        
        public void Execute()
        {
            if (!hasPresed)
            {
                turnState.abruptEnd = true;
                hasPresed = true;
            }
        }
    }
}
