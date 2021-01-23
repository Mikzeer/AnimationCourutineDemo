using PositionerDemo;

namespace StateMachinePattern
{
    public abstract class ActionState<T> : TimeState
    {
        public ActionState(IGame game, int duration) : base(duration, game)
        {
        }

        public virtual void ExecuteAction(T action)
        {

        }
    }
}
