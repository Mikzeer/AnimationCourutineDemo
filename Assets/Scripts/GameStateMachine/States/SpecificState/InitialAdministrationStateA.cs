using PositionerDemo;
using UnityEngine;

namespace StateMachinePattern
{
    public class InitialAdministrationStateA : AdministrationState
    {
        public InitialAdministrationStateA(int duration, GameMachine gmMachine, int mngPoints) : base(duration, gmMachine, mngPoints)
        {
        }

        public override void OnUpdate()
        {
            Debug.Log("IN ADMIN STATE A");
            gameTimer.RestTime();
            if (HaveReachCondition())
            {
                InitialAdministrationStateB adminState = new InitialAdministrationStateB(20, gmMachine, 2);
                OnNextState(adminState);
            }
        }
    }
}
