using UnityEngine;

namespace MikzeerGame.Animotion
{
    #region ANIMATION MOTION

    public class SpawnAnimatedAnimotion : AnimatedAnimotion
    {
        public SpawnAnimatedAnimotion(MonoBehaviour coroutineMono, int reproductionOrder, Animator animator) : base(coroutineMono, reproductionOrder, animator)
        {
        }

        protected override void SetAnimationInformation()
        {
            animationInfo = new SpawnAnimationInformation();
        }
    }

    #endregion
}