using PositionerDemo;

namespace StateMachinePattern
{
    public class InitialAdministrationStateB : AdministrationState
    {
        public InitialAdministrationStateB(int duration, GameMachine gmMachine, int mngPoints) : base(duration, gmMachine, mngPoints)
        {
        }

        public override void OnUpdate()
        {
            gameTimer.RestTime();
            if (HaveReachCondition())
            {
                InitialAdministrationStateC adminState = new InitialAdministrationStateC(20, gmMachine, 2);
                OnNextState(adminState);
            }
        }
    }
}
