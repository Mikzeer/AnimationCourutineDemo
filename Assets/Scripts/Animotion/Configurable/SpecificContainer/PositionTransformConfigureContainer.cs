using UnityEngine;

namespace MikzeerGame.Animotion
{
    #region CONGIFURABLE

    public class PositionTransformConfigureContainer : ConfigureContainer
    {
        // KimbokoPositioConfigureAnimotion<T, O> : ConfigureAnimotion<T, O> where T : Transform where O : Transform
        public Transform transformToRealocate { get; private set; }
        public Transform transformNewPosition { get; private set; }
        public PositionTransformConfigureContainer(Transform transformNewPosition, Transform transformToRealocate)
        {
            this.transformNewPosition = transformNewPosition;
            this.transformToRealocate = transformToRealocate;
        }

        public void Execute()
        {
            transformNewPosition.position = transformToRealocate.position;
        }
    }

    #endregion
}