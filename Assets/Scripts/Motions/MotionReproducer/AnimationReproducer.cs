using UnityEngine;
namespace PositionerDemo
{
    public abstract class AnimationReproducer
    {
        // Nombre del parametro que va a activar esta animacion
        protected string activationParameterString;

        public AnimationReproducer(string activationParameterString)
        {
            this.activationParameterString = activationParameterString;
        }
        public virtual void Apply(Animator animator)
        {

        }
    }

}

