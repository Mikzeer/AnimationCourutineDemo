using UnityEngine;
namespace PositionerDemo
{
    public class DamageMotion : AnimatedMotion
    {
        private const string damageTriggerString = "Damage";
        private const string SkipTriggerString = "Idlle";

        public DamageMotion(MonoBehaviour coroutineMono, Animator animator, int reproductionOrder) : base(coroutineMono, animator, reproductionOrder)
        {
            animotionParameter = new AnimationAnimotionParameter(new AnimationTriggerReproducer(damageTriggerString), new AnimationTriggerReproducer(SkipTriggerString));
        }
    }
}