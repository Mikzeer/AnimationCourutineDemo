using UnityEngine;
namespace PositionerDemo
{
    public class AttackMotion : AnimatedMotion
    {
        private const string attackTriggerString = "Active";
        private const string SkipTriggerString = "Idlle";

        public AttackMotion(MonoBehaviour coroutineMono, Animator animator, int reproductionOrder) : base(coroutineMono, animator, reproductionOrder)
        {
            animotionParameter = new AnimationAnimotionParameter(new AnimationTriggerReproducer(attackTriggerString), new AnimationTriggerReproducer(SkipTriggerString));
        }
    }
}