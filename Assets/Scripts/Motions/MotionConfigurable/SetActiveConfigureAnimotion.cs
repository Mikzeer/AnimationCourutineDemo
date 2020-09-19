using UnityEngine;
namespace PositionerDemo
{
    public class SetActiveConfigureAnimotion<T, O> : ConfigureAnimotion<T, O> where T : Transform where O : Transform
    {
        public SetActiveConfigureAnimotion(T firstConfigure, int configureOrder, bool isForced = true) : base(firstConfigure, configureOrder, isForced)
        {
        }

        public override void Configure()
        {
            firstConfigure.gameObject.SetActive(true);
        }
    }

}

