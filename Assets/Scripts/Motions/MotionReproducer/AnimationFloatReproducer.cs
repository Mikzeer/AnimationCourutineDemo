using UnityEngine;
namespace PositionerDemo
{
    public class AnimationFloatReproducer : AnimationReproducer
    {
        private float parameterValue;
        public AnimationFloatReproducer(string activationParameterString, float parameterValue) : base(activationParameterString)
        {
            this.parameterValue = parameterValue;
        }

        public override void Apply(Animator animator)
        {
            animator.SetFloat(activationParameterString, parameterValue);
        }
    }

}

