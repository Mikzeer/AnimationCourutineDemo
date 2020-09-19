using UnityEngine;
namespace PositionerDemo
{
    public class KimbokoIdlleConfigureAnimotion<T, O> : ConfigureAnimotion<T, O> where T : Animator where O : Transform
    {
        public KimbokoIdlleConfigureAnimotion(T firstConfigure, int configureOrder, bool isForced = true) : base(firstConfigure, configureOrder, isForced)
        {
        }

        public override void Configure()
        {
            firstConfigure.SetTrigger("Idlle");
        }
    }
}

