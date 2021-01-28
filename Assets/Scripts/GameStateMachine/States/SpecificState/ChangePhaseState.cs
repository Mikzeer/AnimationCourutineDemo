using CommandPatternActions;
using PositionerDemo;
using UnityEngine;

namespace StateMachinePattern
{
    public class ChangePhaseState : BaseState
    {
        IState nextState;
        private bool isEnded;
        public ChangePhaseState(IGame game, IState nextState) : base(game)
        {
            this.nextState = nextState;
        }

        public override void OnEnter()
        {
            if (logOn) Debug.Log("ENTER CHANGE PHASE STATE");
            if (InvokerMotion.IsExecuting())
            {
                //Debug.Log("Invoker Motion Is Executing");
                var wait = new WaitEndAnimationState(game, this);
                game.baseStateMachine.PushState(wait);
            }
        }

        public override void OnExit()
        {
            if (logOn) Debug.Log("EXIT CHANGE PHASE STATE");
        }


        public override void OnBack()
        {
            isEnded = true;
        }

        public override void OnUpdate()
        {
            if (HaveReachCondition())
            {
                // PUEDE SER UN INITIAL / ADMIN / TURN
                OnNextState(nextState);
            }
        }

        public override bool HaveReachCondition()
        {
            return isEnded;
        }
    }
}
