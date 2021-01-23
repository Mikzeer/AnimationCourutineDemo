using PositionerDemo;

namespace StateMachinePattern
{
    public abstract class TimeState : BaseState
    {
        protected GameTimer gameTimer;

        public TimeState(int duration, IGame game) : base(game)
        {
            gameTimer = new GameTimer(duration);
        }

        public override void OnEnter()
        {
            gameTimer.Start();
        }

        public override void OnExit()
        {
            gameTimer.Stop();
        }

        public override void OnUpdate()
        {
            gameTimer.RestTime();
        }
    }
}
