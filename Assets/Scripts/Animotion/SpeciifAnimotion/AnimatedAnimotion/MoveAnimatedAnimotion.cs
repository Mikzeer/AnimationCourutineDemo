﻿using UnityEngine;

namespace MikzeerGame.Animotion
{
    #region ANIMATION MOTION

    public class MoveAnimatedAnimotion : AnimatedAnimotion
    {
        private const ANIMATEDMOTIONCHECK MOTION_CHECK = ANIMATEDMOTIONCHECK.LOOP;
        public MoveAnimatedAnimotion(MonoBehaviour coroutineMono, int reproductionOrder, Animator animator) : base(coroutineMono, reproductionOrder, animator, MOTION_CHECK)
        {
        }

        protected override void SetAnimationInformation()
        {
            animationInfo = new MoveAnimationInformation();
        }
    }

    #endregion
}