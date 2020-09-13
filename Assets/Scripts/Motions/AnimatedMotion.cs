using System.Collections;
using UnityEngine;
namespace PositionerDemo
{
    public abstract class AnimatedMotion : Motion
    {
        protected AnimatorStateInfo animatorStateInfo;
        protected Animator animator;

        protected float animationNormalSpeed = 1;
        public float animationSpeedUpVelocity { get; protected set; }
        private const string animationSpeedParameterString = "AnimationSpeed";

        public bool isSpecialCheck = false;

        protected AnimationAnimotionParameter animotionParameter; 

        public AnimatedMotion(MonoBehaviour coroutineMono, Animator animator, int reproductionOrder, bool specialCheck = false) : base(coroutineMono, reproductionOrder)
        {
            animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
            this.animator = animator;
            this.isSpecialCheck = specialCheck;
        }

        protected override void StartMotion()
        {
            animotionParameter.Reproduce(animator);
        }

        protected override IEnumerator CheckPendingRunningMotions()
        {

            if (isSpecialCheck == false)
            {
                while (animatorStateInfo.shortNameHash != animator.GetCurrentAnimatorStateInfo(0).shortNameHash)
                {
                    Debug.Log("INTERNAL CheckPendingRunningMotions");
                    yield return null;
                }

                hasEnded = true;
            }
            else
            {
                while (performing)
                {
                    //Debug.Log("INTERNAL CheckPendingRunningMotions");
                    yield return null;

                }
            }
            // PUEDO CHEQUEAR POR NOMBRE
            // PUEDO CHEQUAR TAMBIEN POR SI FINALIZO... DEBERIA BUSCARLO

            //while (!animator.GetCurrentAnimatorStateInfo(0).IsName(animParameter.skipParameterString))
            //{
            //    //Debug.Log("CheckPendingRunningMotions");
            //    yield return null;
            //}
        }

        public override void OnMotionSkip()
        {
            Debug.Log("Skip ANIMATION");
            animotionParameter.End(animator);
            base.OnMotionSkip();
        }

        protected override void CheckMotionAfterEnd()
        {
            OnMotionSkip();
        }

        protected override void SpeedUpMotionOnMotion()
        {

            animationSpeedUpVelocity = Mathf.FloorToInt(speed);
            animator.SetFloat(animationSpeedParameterString, animationSpeedUpVelocity);
            //enemies[i].animator.SetFloat(animationSpeedParameter, animationSpeedUpVelocity);
            //entry.Key.animator.SetFloat(animationSpeedParameter, animationSpeedUpVelocity);
            //craneAnimator.SetFloat(animationSpeedParameter, animationSpeedUpVelocity);
            //kimbokoAnimator.SetFloat(animationSpeedParameter, animationSpeedUpVelocity);
        }

        protected override void SetNormalSpeedInMotion()
        {

            animator.SetFloat(animationSpeedParameterString, animationNormalSpeed);
            //enemies[i].animator.SetFloat(animationSpeedParameter, animationNormalSpeed);
            //entry.Key.animator.SetFloat(animationSpeedParameter, animationNormalSpeed);
            //craneAnimator.SetFloat(animationSpeedParameter, animationNormalSpeed);
            //kimbokoAnimator.SetFloat(animationSpeedParameter, animationNormalSpeed);
        }

    }

}

