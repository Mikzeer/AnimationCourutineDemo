using CommandPatternActions;
using PositionerDemo;
using System.Collections.Generic;

namespace StateMachinePattern
{
    public abstract class SubSelectionState<T> : BaseState
    {
        public IState previousState;
        List<T> posibleSelectionTargets;
        List<T> selectedTargets;

        public SubSelectionState(IGame game, IState previousState) : base(game)
        {
            this.previousState = previousState;
        }

        public virtual void SetSelection(Tile selection)
        {
            // MOVE/ATTACK/COMBINE/SPAWN Si esta dentro de las posibles Ejecuto, sino Cancelo
            // DECOMBINE Si esta dentro de las posibles y no esta esa tile y la lista cumplio con lo requerido Ejecuto, sino Cancelo
            // USE CARD // SELECT THE CARD TARGETS
        }

        public override void OnUpdate()
        {
            previousState.OnUpdate();
        }

        public override void OnNextState(IState nextState)
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
                game.baseStateMachine.ChangeAndGetBack(nextState);
            }
        }
    }
}
