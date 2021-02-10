using CommandPatternActions;
using PositionerDemo;

namespace StateMachinePattern
{
    public class WaitForActionState : BaseState
    {
        public IState previousState;
        public WaitForActionState(IGame game, IState previousState) : base(game)
        {
            this.previousState = previousState;
        }

        public override void OnUpdate()
        {
            if (previousState.HaveReachCondition())
            {
                game.baseStateMachine.PopState();
                game.baseStateMachine.PushState(previousState, true);
            }
            else
            {
                previousState.OnUpdate();
            }
        }

        public override void OnNextState(IState nextState)
        {
            if (InvokerAnimotion.IsExecuting())
            {
                game.baseStateMachine.PopState();
                var wait = new WaitEndAnimationState(game, nextState);
                game.baseStateMachine.PushState(wait);
            }
            else
            {
                game.baseStateMachine.ChangeAndGetBack(nextState);
            }
        }
    }
}
