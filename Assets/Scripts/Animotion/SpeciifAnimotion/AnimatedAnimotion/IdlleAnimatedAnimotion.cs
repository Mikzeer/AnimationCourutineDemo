using UnityEngine;

namespace MikzeerGame.Animotion
{
    #region ANIMATION MOTION

    public class IdlleAnimatedAnimotion : AnimatedAnimotion
    {
        private const ANIMATEDMOTIONCHECK MOTION_CHECK = ANIMATEDMOTIONCHECK.JUSTEXECUTION;
        public IdlleAnimatedAnimotion(MonoBehaviour coroutineMono, int reproductionOrder, Animator animator) : base(coroutineMono, reproductionOrder, animator, MOTION_CHECK)
        {
        }

        protected override void SetAnimationInformation()
        {
            animationInfo = new IdlleAnimationInformation();
        }
    }

    #endregion
}