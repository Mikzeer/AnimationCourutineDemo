using PositionerDemo;

namespace StateMachinePattern
{
    public class TakeCardState : WaitForActionState
    {
        GameMachine gmMachine;

        public TakeCardState(GameMachine game, IState previousState) : base(game, previousState)
        {
            gmMachine = game;
        }

        public override void OnEnter()
        {
            OnNextState(previousState);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

    }
}
