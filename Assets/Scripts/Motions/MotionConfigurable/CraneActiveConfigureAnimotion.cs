using UnityEngine;
namespace PositionerDemo
{
    public class CraneActiveConfigureAnimotion<T, O> : ConfigureAnimotion<T, O> where T : Transform where O : Transform
    {
        public CraneActiveConfigureAnimotion(T firstConfigure, int configureOrder) : base(firstConfigure, configureOrder)
        {

        }

        public override void Configure()
        {
            firstConfigure.gameObject.SetActive(false);
        }
    }

}

