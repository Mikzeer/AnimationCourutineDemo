using UnityEngine;

namespace MikzeerGame.Animotion
{
    #region ANIMATION MOTION

    public class DamageAnimatedAnimotion : AnimatedAnimotion
    {
        public DamageAnimatedAnimotion(MonoBehaviour coroutineMono, int reproductionOrder, Animator animator) : base(coroutineMono, reproductionOrder, animator)
        {
        }

        protected override void SetAnimationInformation()
        {
            animationInfo = new DamgeAnimationInformation();
        }
    }

    #endregion
}