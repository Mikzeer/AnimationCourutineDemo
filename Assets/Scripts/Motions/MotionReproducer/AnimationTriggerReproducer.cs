using UnityEngine;
namespace PositionerDemo
{
    public class AnimationTriggerReproducer : AnimationReproducer
    {
        public AnimationTriggerReproducer(string activationParameterString) : base(activationParameterString)
        {
        }

        public override void Apply(Animator animator)
        {
            animator.SetTrigger(activationParameterString);
        }
    }
}