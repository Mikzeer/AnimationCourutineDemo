using CommandPatternActions;
using PositionerDemo;

namespace StateMachinePattern
{
    public abstract class BaseState : IState
    {
        protected IGame game { get; private set; }
        public string stateName { get; protected set; }
        protected bool logOn = false;
        public BaseState(IGame game)
        {
            this.game = game;
        }

        public virtual bool HaveReachCondition()
        {
            return true;
        }

        public virtual void OnClear()
        {
        }

        public virtual void OnEnter()
        {
        }

        public virtual void OnExit()
        {
        }

        public virtual void OnNextState(IState nextState)
        {
            // ACA DEBERIA REVISAR SI EL INVOKER MOTION ESTA EJECUTANDO...
            if (InvokerMotion.IsExecuting())
            {
                game.baseStateMachine.PopState();
                var wait = new WaitEndAnimationState(game, nextState);
                game.baseStateMachine.PushState(wait);
            }
            else
            {
                game.baseStateMachine.ChangeAndEnterState(nextState);
            }
        }

        public virtual void OnUpdate()
        {
        }

        public virtual void OnBack()
        {
        }
    }
}