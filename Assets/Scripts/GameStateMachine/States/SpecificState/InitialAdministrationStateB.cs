using PositionerDemo;

namespace StateMachinePattern
{
    public class InitialAdministrationStateB : InitialAdministrationStateBase
    {
        public InitialAdministrationStateB(int duration, GameMachine gmMachine, int mngPoints) : base(duration, gmMachine, mngPoints)
        {
            stateName = "INITIAL ADMINISTRATION STATE B";
            nextState = new InitialAdministrationStateC(20, gmMachine, 2);
        }
    }
}