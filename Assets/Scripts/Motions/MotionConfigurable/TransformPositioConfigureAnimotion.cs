using UnityEngine;
namespace PositionerDemo
{
    public class TransformPositioConfigureAnimotion<T, O> : ConfigureAnimotion<T, O> where T : Transform where O : ConfigurePositionAssistant
    {
        public TransformPositioConfigureAnimotion(T firstConfigure, O secondConfigure, int configureOrder, bool isForced = false) : base(firstConfigure, secondConfigure, configureOrder, isForced)
        {
        }

        public override void Configure()
        {
            firstConfigure.position = secondConfigure.positionToRelocate;
        }
    }

}

