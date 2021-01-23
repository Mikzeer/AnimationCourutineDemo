using System.Collections.Generic;

namespace StateMachinePattern
{
    public class BaseStateMachine
    {
        #region VARIABLES

        public bool IsInitialized { get; protected set; }
        // STACK DE TODOS LOS STATES QUE TENEMOS ACTUALMENTE
        private readonly Stack<IState> stack = new Stack<IState>();
        /// <summary>
        ///     Handler for the FSM. Usually the Monobehavior which holds this FSM.
        /// </summary>
        public IStateMachineHandler Handler { get; set; }
        public IState currentState => PeekState();

        #endregion

        public BaseStateMachine(IStateMachineHandler Handler = null)
        {
            this.Handler = Handler;
        }

        public void Update()
        {
            if (currentState != null)
            {
                currentState.OnUpdate();
            }
        }

        public IState PeekState()
        {
            if (stack.Count <= 0)
            {
                return null;
            }
            return stack.Peek();
            //return stack.Count > 0 ? stack.Peek() : null;
        }

        public void Initialize()
        {
            currentState.OnEnter();
            IsInitialized = true;
        }

        public void PushState(IState state, bool isSilent = false)
        {
            stack.Push(state);
            if (isSilent == false)
            {
                state.OnEnter();
            }
        }

        public void PopState(bool isSilent = false)
        {
            if (currentState == null)
                return;

            var state = stack.Pop();
            if (!isSilent)
            {
                state.OnExit();
            }

        }

        public void ChangeAndEnterState(IState nextState)
        {
            PopState();
            PushState(nextState, false);
        }

        public void ChangeAndGetBack(IState previousState)
        {
            PopState();
            previousState.OnBack();
            PushState(previousState, true);
        }

        public void FinishActualState()
        {
            PopState();
        }
    }
}
