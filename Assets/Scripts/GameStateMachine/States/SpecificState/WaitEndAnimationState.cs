using CommandPatternActions;
using PositionerDemo;
using UnityEngine;

namespace StateMachinePattern
{
    public class WaitEndAnimationState : BaseState
    {
        IState nextState;

        public WaitEndAnimationState(IGame game, IState nextState) : base(game)
        {
            this.nextState = nextState;
        }

        public override void OnUpdate()
        {
            if (HaveReachCondition())
            {
                game.baseStateMachine.ChangeAndGetBack(nextState);
            }
        }

        public override bool HaveReachCondition()
        {
            if (InvokerMotion.IsExecuting())
            {
                if (logOn) Debug.Log("Waiting For The End Of Animation");
                return false;
            }
            return true;
        }
    }
}
