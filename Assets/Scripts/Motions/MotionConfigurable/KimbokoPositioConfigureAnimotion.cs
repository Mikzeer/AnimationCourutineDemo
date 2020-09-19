using UnityEngine;
namespace PositionerDemo
{
    public class KimbokoPositioConfigureAnimotion<T,O> : ConfigureAnimotion<T, O> where T : Transform where O : Transform
    {
        public KimbokoPositioConfigureAnimotion(T firstConfigure, O secondConfigure, int configureOrder, bool isForced = false) : base(firstConfigure, secondConfigure, configureOrder, isForced)
        {
        }

        public override void Configure()
        {
            firstConfigure.position = secondConfigure.position;
        }
    }

}

