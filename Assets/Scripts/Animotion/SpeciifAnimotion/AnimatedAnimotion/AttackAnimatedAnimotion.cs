using UnityEngine;

namespace MikzeerGame.Animotion
{
    #region ANIMATION MOTION

    public class AttackAnimatedAnimotion : AnimatedAnimotion
    {
        private const ANIMATEDMOTIONCHECK MOTION_CHECK = ANIMATEDMOTIONCHECK.SELFCHECK;
        public AttackAnimatedAnimotion(MonoBehaviour coroutineMono, int reproductionOrder, Animator animator) : base(coroutineMono, reproductionOrder, animator, MOTION_CHECK)
        {
        }

        protected override void SetAnimationInformation()
        {
            animationInfo = new AttackAnimationInformation();
        }
    }

    #endregion
}