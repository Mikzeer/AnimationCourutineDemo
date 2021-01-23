using PositionerDemo;

namespace StateMachinePattern
{
    public class InitialAdministrationStateC : AdministrationState
    {
        public InitialAdministrationStateC(int duration, GameMachine gmMachine, int mngPoints) : base(duration, gmMachine, mngPoints)
        {
        }

        public override void OnUpdate()
        {
            gameTimer.RestTime();
            if (HaveReachCondition())
            {
                InitialAdministrationStateD adminState = new InitialAdministrationStateD(40, gmMachine, 4);
                OnNextState(adminState);
            }
        }
    }
}
