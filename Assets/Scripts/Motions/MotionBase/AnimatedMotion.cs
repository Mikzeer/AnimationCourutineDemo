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

        public AnimatedMotion(MonoBehaviour coroutineMono, Animator animator, int reproductionOrder, bool isSpecialCheck = false) : base(coroutineMono, reproductionOrder)
        {
            //AnimatorControllerParameter[] animatorControllParameters = animator.parameters;
            //AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
            //int d = animator.GetInstanceID();
            //int j = animator.GetLayerIndex("Base Layer");
            string layerName = animator.GetLayerName(0);

            //int pipo = Animator.StringToHash(layerName + ".Idlle");

            int shortNameHash = Animator.StringToHash(layerName + ".Spawn_Animation");

            animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);

            this.animator = animator;
            this.isSpecialCheck = isSpecialCheck;
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
                    yield return null;
                }
            }

            //if (isSpecialCheck == false)
            //{
            //    while (animatorStateInfo.fullPathHash != animator.GetCurrentAnimatorStateInfo(0).fullPathHash)
            //    {
            //        yield return null;
            //    }
            //}

            //while (!animator.GetCurrentAnimatorStateInfo(0).IsName(animParameter.skipParameterString))
            //{
            //    //Debug.Log("CheckPendingRunningMotions");
            //    yield return null;
            //}
        }

        public override void OnMotionSkip()
        {
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
        }

        protected override void SetNormalSpeedInMotion()
        {
            animator.SetFloat(animationSpeedParameterString, animationNormalSpeed);
        }

    }

}



