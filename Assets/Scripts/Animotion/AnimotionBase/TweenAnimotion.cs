using UnityEngine;
using DG.Tweening;
using System.Collections;

namespace MikzeerGame.Animotion
{
    public abstract class TweenAnimotion : Animotion
    {
        private Transform transform;
        protected Ease ease = Ease.Linear;
        private Tween actualTween;

        protected float tweenNormalSpeed = 1;
        public float tweenSpeedUp { get; protected set; }
        public float tweenActualSpeed { get; protected set; }

        protected TweenParameter tweenParameter;

        public TweenAnimotion(MonoBehaviour coroutineMono, int reproductionOrder, Transform transform) : base(coroutineMono, reproductionOrder)
        {
            tweenActualSpeed = tweenNormalSpeed;
            this.transform = transform;
        }

        protected override void StartMotion()
        {
            actualTween = tweenParameter.Reproduce(transform);
            actualTween.timeScale = tweenActualSpeed;
        }

        protected override IEnumerator CheckPendingRunningMotions()
        {
            while (tweenParameter.isOver(transform) == false)
            {
                //Debug.Log("CheckPendingRunningMotions Tweener");
                yield return null;
            }
        }

        public override void OnMotionSkip()
        {
            base.OnMotionSkip();
            tweenParameter.End(actualTween, transform);
        }

        protected override void OnMotionEnd()
        {
            tweenParameter.End(actualTween, transform);
        }

        #region SPEED UP DOWN TWEEN MOTION

        protected override void SpeedUpMotionOnMotion()
        {
            tweenSpeedUp = Mathf.FloorToInt(speed * 2);
            tweenActualSpeed = tweenSpeedUp;
            if (actualTween == null) return;
            actualTween.timeScale = tweenSpeedUp;
        }

        protected override void SetNormalSpeedInMotion()
        {
            tweenActualSpeed = tweenNormalSpeed;
            if (actualTween == null) return;
            actualTween.timeScale = tweenNormalSpeed;
        }

        #endregion
    }
}