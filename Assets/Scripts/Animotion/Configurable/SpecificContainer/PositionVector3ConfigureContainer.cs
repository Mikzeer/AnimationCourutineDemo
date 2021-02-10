using UnityEngine;

namespace MikzeerGame.Animotion
{
    #region CONGIFURABLE

    public class PositionVector3ConfigureContainer : ConfigureContainer
    {
        // TransformPositioConfigureAnimotion<T, O> : ConfigureAnimotion<T, O> where T : Transform where O : ConfigurePositionAssistant
        public Transform transformToRealocate { get; private set; }
        public Vector3 positionToRelocate { get; private set; }
        public PositionVector3ConfigureContainer(Vector3 positionToRelocate, Transform transformToRealocate)
        {
            this.positionToRelocate = positionToRelocate;
            this.transformToRealocate = transformToRealocate;
        }

        public void Execute()
        {
            transformToRealocate.position = positionToRelocate;
        }
    }

    #endregion
}