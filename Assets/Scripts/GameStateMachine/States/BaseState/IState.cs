namespace StateMachinePattern
{
    public interface IState
    {
        void OnEnter();
        void OnExit();
        void OnUpdate();
        bool HaveReachCondition();
        void OnClear();
        void OnNextState(IState nextState);
        void OnBack();
    }
}
