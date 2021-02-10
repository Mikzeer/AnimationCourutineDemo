using UnityEngine;

namespace MikzeerGame.Animotion
{
    #region ANIMATION MOTION

    public class ShieldAnimatedAnimotion : AnimatedAnimotion
    {
        public ShieldAnimatedAnimotion(MonoBehaviour coroutineMono, int reproductionOrder, Animator animator) : base(coroutineMono, reproductionOrder, animator)
        {
        }

        protected override void SetAnimationInformation()
        {
            animationInfo = new AttackAnimationInformation();
        }
    }

    #endregion
}