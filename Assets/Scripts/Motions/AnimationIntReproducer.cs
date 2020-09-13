using UnityEngine;
namespace PositionerDemo
{
    public class AnimationIntReproducer : AnimationReproducer
    {
        private int parameterValue;
        public AnimationIntReproducer(string activationParameterString, int parameterValue) : base(activationParameterString)
        {
            this.parameterValue = parameterValue;
        }

        public override void Apply(Animator animator)
        {
            animator.SetInteger(activationParameterString, parameterValue);
        }
    }

}

