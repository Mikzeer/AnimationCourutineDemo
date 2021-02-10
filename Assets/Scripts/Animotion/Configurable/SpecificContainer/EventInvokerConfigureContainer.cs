using System;

namespace MikzeerGame.Animotion
{
    #region CONGIFURABLE

    public class EventInvokerConfigureContainer : ConfigureContainer
    {
        //InvokeEventConfigureAnimotion<T, O> : ConfigureAnimotion<T, O> where T : EventInvokerGenericContainer where O : Transform
        public event Action eventAction;

        public EventInvokerConfigureContainer(Action eventAction)
        {
            this.eventAction = eventAction;
        }

        public void Execute()
        {
            eventAction?.Invoke();
        }
    }

    #endregion
}