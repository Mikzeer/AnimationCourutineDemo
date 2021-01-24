using PositionerDemo;

namespace StateMachinePattern
{
    public class InitialAdministrationStateA : InitialAdministrationStateBase
    {
        public InitialAdministrationStateA(int duration, GameMachine gmMachine, int mngPoints) : base(duration, gmMachine, mngPoints)
        {
            stateName = "INITIAL ADMINISTRATION STATE A";
            nextState = new InitialAdministrationStateB(20, gmMachine, 2);
        }
    }
}