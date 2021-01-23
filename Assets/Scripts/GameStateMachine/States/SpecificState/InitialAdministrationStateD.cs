using PositionerDemo;

namespace StateMachinePattern
{
    public class InitialAdministrationStateD : AdministrationState
    {
        public InitialAdministrationStateD(int duration, GameMachine gmMachine, int mngPoints) : base(duration, gmMachine, mngPoints)
        {
        }

        public override void OnUpdate()
        {
            gameTimer.RestTime();
            if (HaveReachCondition())
            {
                AdministrationState adminState = new AdministrationState(20, gmMachine, 1);
                OnNextState(adminState);
            }
        }
    }
}
