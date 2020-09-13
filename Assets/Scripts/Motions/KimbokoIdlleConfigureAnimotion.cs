using UnityEngine;
namespace PositionerDemo
{
    public class KimbokoIdlleConfigureAnimotion<T, O> : ConfigureAnimotion<T, O> where T : Animator where O : Transform
    {
        public KimbokoIdlleConfigureAnimotion(T firstConfigure, int configureOrder) : base(firstConfigure, configureOrder)
        {
        }

        public override void Configure()
        {
            firstConfigure.SetTrigger("Idlle");
        }
    }

    public class MotionIdlleConfigureAnimotion<T, O> : ConfigureAnimotion<T, O> where T : AnimatedMotion where O : Transform
    {
        public MotionIdlleConfigureAnimotion(T firstConfigure, int configureOrder) : base(firstConfigure, configureOrder)
        {
        }

        public override void Configure()
        {
            //Debug.Log("Enter to interrupo CONFIGURE");
            firstConfigure.SetPerforming();
        }
    }

}

