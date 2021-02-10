using System.Collections;
using UnityEngine;
namespace PositionerDemo
{
    public class ShieldMotion : AnimatedMotion
    {
        private const string attackTriggerString = "Active";
        private const string SkipTriggerString = "Idlle";

        int fullPathHash;
        float waitTime;
        public ShieldMotion(MonoBehaviour coroutineMono, Animator animator, int reproductionOrder) : base(coroutineMono, animator, reproductionOrder, true)
        {
            animotionParameter = new AnimationAnimotionParameter(new AnimationTriggerReproducer(attackTriggerString), new AnimationTriggerReproducer(SkipTriggerString));

            string layerName = animator.GetLayerName(0);
            fullPathHash = Animator.StringToHash(layerName + ".Active");
            waitTime = animator.GetCurrentAnimatorStateInfo(0).length;
        }

        protected override IEnumerator CheckPendingRunningMotions()
        {
            while (fullPathHash != animator.GetCurrentAnimatorStateInfo(0).fullPathHash)
            {
                yield return null;
            }

            float counter = 0;
            
            //Now, Wait until the current state is done playing
            while (counter < (waitTime))
            {
                counter += Time.deltaTime;
                yield return null;
            }

        }

        protected override void CheckMotionAfterEnd()
        {
        }

        protected override void StartMotion()
        {
        }

        public override void OnMotionSkip()
        {
            SetNormalSpeedMotion();           
        }
    }
}