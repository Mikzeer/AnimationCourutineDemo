using UnityEngine;
namespace PositionerDemo
{
    public class SetCanvasGroupBlockRaycastConfigureAnimotion<T, O> : ConfigureAnimotion<T, O> where T : CardInGameUINEW where O : Transform
    {
        private bool active;

        public SetCanvasGroupBlockRaycastConfigureAnimotion(T firstConfigure, O secondConfigure, int configureOrder, bool isForced = true, bool active = true) : base(firstConfigure, secondConfigure, configureOrder, isForced)
        {
            this.active = active;
        }

        public override void Configure()
        {
            firstConfigure.CanvasGroupRaycast(active);
        }
    }
}

