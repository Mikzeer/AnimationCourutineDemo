using UnityEngine;
namespace PositionerDemo
{
    public class SetActiveConfigureAnimotion<T, O> : ConfigureAnimotion<T, O> where T : Transform where O : Transform
    {
        private bool active;

        public SetActiveConfigureAnimotion(T firstConfigure, int configureOrder, bool isForced = true, bool active = true) : base(firstConfigure, configureOrder, isForced)
        {
            this.active = active;
        }

        public override void Configure()
        {
            firstConfigure.gameObject.SetActive(active);
        }
    }

}

