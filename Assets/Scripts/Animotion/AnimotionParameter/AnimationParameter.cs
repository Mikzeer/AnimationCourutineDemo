using UnityEngine;
using PositionerDemo;

namespace MikzeerGame.Animotion
{
    #region ANIMOTION PARAMETERS

    public class AnimationParameter
    {
        AnimationReproducer startReproducer;
        AnimationReproducer endReproducer;

        public AnimationParameter(AnimationReproducer startReproducer, AnimationReproducer endReproducer)
        {
            this.startReproducer = startReproducer;
            this.endReproducer = endReproducer;
        }

        public void Reproduce(Animator animator)
        {
            startReproducer.Apply(animator);
        }

        public void End(Animator animator)
        {
            endReproducer.Apply(animator);
        }
    }

    #endregion
}