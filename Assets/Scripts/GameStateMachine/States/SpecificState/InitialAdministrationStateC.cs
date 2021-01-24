using PositionerDemo;

namespace StateMachinePattern
{
    public class InitialAdministrationStateC : InitialAdministrationStateBase
    {
        public InitialAdministrationStateC(int duration, GameMachine gmMachine, int mngPoints) : base(duration, gmMachine, mngPoints)
        {
            stateName = "INITIAL ADMINISTRATION STATE C";
            nextState = new InitialAdministrationStateD(40, gmMachine, 4);
        }
    }
}