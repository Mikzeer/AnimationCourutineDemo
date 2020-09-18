using UnityEngine;
namespace PositionerDemo
{
    public class KimbokoPositioConfigureAnimotion<T,O> : ConfigureAnimotion<T, O> where T : Transform where O : Transform
    {
        public KimbokoPositioConfigureAnimotion(T firstConfigure, O secondConfigure, int configureOrder) : base(firstConfigure, secondConfigure, configureOrder)
        {
        }

        public override void Configure()
        {
            firstConfigure.position = secondConfigure.position;
            //firstConfigure.gameObject.SetActive(true);
        }
    }

}

