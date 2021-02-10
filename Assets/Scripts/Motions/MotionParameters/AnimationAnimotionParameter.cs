using UnityEngine;
namespace PositionerDemo
{
    public class AnimationAnimotionParameter
    {
        AnimationReproducer startReproducer;
        AnimationReproducer endReproducer;

        public AnimationAnimotionParameter(AnimationReproducer startReproducer, AnimationReproducer endReproducer)
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

}