using UnityEngine;
namespace PositionerDemo
{
    public class AnimationBoolReproducer : AnimationReproducer
    {
        public AnimationBoolReproducer(string activationParameterString) : base(activationParameterString)
        {
        }

        public override void Apply(Animator animator)
        {
            animator.SetBool(activationParameterString, true);
        }
    }

}

