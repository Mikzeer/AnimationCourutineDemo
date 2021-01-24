using PositionerDemo;

namespace StateMachinePattern
{
    public class InitialAdministrationStateD : InitialAdministrationStateBase
    {
        public InitialAdministrationStateD(int duration, GameMachine gmMachine, int mngPoints) : base(duration, gmMachine, mngPoints)
        {
            stateName = "INITIAL ADMINISTRATION STATE D";
            nextState = new AdministrationState(20, gmMachine, 1);
        }
    }
}
