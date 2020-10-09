using System;
using UnityEngine;
namespace PositionerDemo
{
    public class InvokeEventConfigureAnimotion<T, O> : ConfigureAnimotion<T, O> where T : EventInvokerGenericContainer where O : Transform
    {
        public InvokeEventConfigureAnimotion(T firstConfigure, int configureOrder, bool isForced = true) : base(firstConfigure, configureOrder, isForced)
        {
        }

        public override void Configure()
        {
            firstConfigure.Execute();            
        }
    }

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

