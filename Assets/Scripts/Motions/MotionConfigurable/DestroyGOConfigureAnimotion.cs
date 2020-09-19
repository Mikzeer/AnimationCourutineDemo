using UnityEngine;
namespace PositionerDemo
{
    public class DestroyGOConfigureAnimotion<T, O> : ConfigureAnimotion<T, O> where T : Transform where O : Transform
    {
        public DestroyGOConfigureAnimotion(T firstConfigure, int configureOrder, bool isForced = true) : base(firstConfigure, configureOrder, isForced)
        {
        }

        public override void Configure()
        {
            GameObject.Destroy(firstConfigure.gameObject);
        }
    }

}

