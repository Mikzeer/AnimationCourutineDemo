using UnityEngine;
namespace PositionerDemo
{
    public class SpawnMotion : AnimatedMotion
    {
        private const string SkipTriggerString = "Idlle";
        private const string spawnTriggerString = "Spawn";
        public SpawnMotion(MonoBehaviour coroutineMono, Animator animator, int reproductionOrder) : base(coroutineMono, animator, reproductionOrder)
        {
            animotionParameter = new AnimationAnimotionParameter(new AnimationTriggerReproducer(spawnTriggerString), new AnimationTriggerReproducer(SkipTriggerString));
        }
    }
}