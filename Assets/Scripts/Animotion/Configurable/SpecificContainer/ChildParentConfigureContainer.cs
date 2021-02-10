using UnityEngine;

namespace MikzeerGame.Animotion
{
    #region CONGIFURABLE

    public class ChildParentConfigureContainer : ConfigureContainer
    {
        //SetParentConfigureAnimotion<T, O> : ConfigureAnimotion<T, O> where T : Transform where O : Transform
        public Transform childTransform { get; private set; }
        public Transform parentTransform { get; private set; }

        public ChildParentConfigureContainer(Transform parentTransform, Transform childTransform)
        {
            this.parentTransform = parentTransform;
            this.childTransform = childTransform;
        }

        public void Execute()
        {
            childTransform.SetParent(parentTransform);
        }
    }

    #endregion
}