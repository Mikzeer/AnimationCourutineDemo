using System;
namespace PositionerDemo
{
    public class EventInvokerGenericContainer
    {
        public event Action eventAction;

        public EventInvokerGenericContainer(Action eventAction)
        {
            this.eventAction = eventAction;
        }

        public void Execute()
        {
            eventAction?.Invoke();
        }
    }
}