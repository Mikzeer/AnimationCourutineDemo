using UnityEngine;
using PositionerDemo;
using System.Collections;

namespace MikzeerGame.Animotion
{
    public abstract class AnimatedAnimotion : Animotion
    {
        private const string ANIMATION_SPEED_PARAMETER_NAME = "AnimationSpeed";
        private const float ANIMATION_NORMAL_SPEED = 1;
        public float animationSpeedUpVelocity { get; private set; }

        ANIMATEDMOTIONCHECK motionCheck;
        protected Animator animator;
        protected AnimationParameter animationParameter;
        protected AnimationInformation animationInfo;
        protected AnimatorStateInfo animatorStateInfo;

        float waitTime;

        // ESTE ES EL PROXIMO STATE O ANIMACION AL CUAL QUEREMOS ENTRAR CADA CHILD SE VA A ENCARGAR DE EL
        public int nextFullPathHash { get; protected set; } = 0;
        // EL INDEX QUE A COMPARAR CON EL ANIMATOR
        public int animatorLayerIndex { get; protected set; } = 0;

        public AnimatedAnimotion(MonoBehaviour coroutineMono, int reproductionOrder, Animator animator, ANIMATEDMOTIONCHECK motionCheck = ANIMATEDMOTIONCHECK.BACKTOPREVIOUS)
            : base(coroutineMono, reproductionOrder)
        {
            this.animator = animator;
            this.motionCheck = motionCheck;
            // OBTENES EL STRING DEL PROXIMO STATE/ANIMACION AL CUAL QUEREMOS ENTRAR/EJECUTAR
            SetAnimationInformation();
            animationParameter = new AnimationParameter(new AnimationTriggerReproducer(animationInfo.startParameter),
                                            new AnimationTriggerReproducer(animationInfo.endParameter));
            nextFullPathHash = Animator.StringToHash(animationInfo.fullAnimationPath);
            animatorLayerIndex = animationInfo.animatorLayerIndex;
            animatorStateInfo = animator.GetCurrentAnimatorStateInfo(animationInfo.animatorLayerIndex);
        }

        protected override void StartMotion()
        {
            animationParameter.Reproduce(animator);
        }

        protected override IEnumerator CheckPendingRunningMotions()
        {
            // 1 - WAIT TO ENTER NEXT STATE
            if (nextFullPathHash != 0)
            {
                // NOS ASEGURAMOS QUE HAYAMOS ENTRADO SIGUIENTE STATE/ANIMACION
                while (animator.GetCurrentAnimatorStateInfo(animatorLayerIndex).fullPathHash != nextFullPathHash)
                {
                    Debug.Log("WAIT TO ENTER SELECTED ANIMATION");
                    yield return null;
                }
            }

            // 2 - CHECK MOTION END
            switch (motionCheck)
            {
                case ANIMATEDMOTIONCHECK.BACKTOPREVIOUS:
                    while (animatorStateInfo.fullPathHash != animator.GetCurrentAnimatorStateInfo(animatorLayerIndex).fullPathHash)
                    {
                        //Debug.Log("animatorStateInfo.fullPathHash " + animatorStateInfo.fullPathHash);
                        //Debug.Log("animator.GetCurrentAnimatorStateInfo(0).fullPathHash " + animator.GetCurrentAnimatorStateInfo(0).fullPathHash);
                        yield return null;
                    }
                    break;
                case ANIMATEDMOTIONCHECK.LOOP:
                case ANIMATEDMOTIONCHECK.SELFCHECK:
                    while (animator.GetCurrentAnimatorStateInfo(animatorLayerIndex).fullPathHash == nextFullPathHash)
                    {
                        yield return null;
                    }
                    break;
                case ANIMATEDMOTIONCHECK.TIMED:
                    waitTime = animator.GetCurrentAnimatorStateInfo(0).length;
                    float counter = 0;
                    //Now, Wait until the current state is done playing
                    while (counter < (waitTime))
                    {
                        counter += Time.deltaTime;
                        yield return null;
                    }
                    break;
                case ANIMATEDMOTIONCHECK.JUSTEXECUTION:
                default:
                    break;
            }
        }

        public override void OnMotionSkip()
        {
            if (motionCheck == ANIMATEDMOTIONCHECK.JUSTEXECUTION)
            {
                return;
            }
            base.OnMotionSkip();
            animationParameter.End(animator);
        }

        protected override void OnMotionEnd()
        {
            if (motionCheck == ANIMATEDMOTIONCHECK.JUSTEXECUTION)
            {
                return;
            }
            animationParameter.End(animator);
        }

        protected abstract void SetAnimationInformation();

        #region SPEED UP DOWN ANIMATED MOTION

        protected override void SpeedUpMotionOnMotion()
        {
            animationSpeedUpVelocity = Mathf.FloorToInt(speed);
            animator.SetFloat(ANIMATION_SPEED_PARAMETER_NAME, animationSpeedUpVelocity);
        }

        protected override void SetNormalSpeedInMotion()
        {
            animator.SetFloat(ANIMATION_SPEED_PARAMETER_NAME, ANIMATION_NORMAL_SPEED);
        }

        #endregion
    }
}