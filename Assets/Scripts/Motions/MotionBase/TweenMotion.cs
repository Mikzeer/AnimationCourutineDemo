﻿using DG.Tweening;
using System.Collections;
using UnityEngine;
namespace PositionerDemo
{
    public abstract class TweenMotion : Motion
    {
        private Transform transform;
        protected Ease ease = Ease.Linear;
        private Tween actualTween;

        protected float tweenNormalSpeed = 1;
        public float tweenSpeedUp { get; protected set; }
        public float tweenActualSpeed { get; protected set; }

        protected TweenAnimotionParameter animotionParameter;

        public TweenMotion(MonoBehaviour coroutineMono, Transform transform, int reproductionOrder) : base(coroutineMono, reproductionOrder)
        {
            tweenActualSpeed = tweenNormalSpeed;
            //tweenActualSpeed = 5;
            this.transform = transform;
        }

        protected override void StartMotion()
        {
            //movingTween = transform.DOMove(endPostion, 20).SetEase(ease);
            //movingTween[index] = entry.Key.transform.DOPath(entry.Value, 10).SetEase(ease);
            actualTween = animotionParameter.Reproduce(transform);
            actualTween.timeScale = tweenActualSpeed;
        }

        protected override IEnumerator CheckPendingRunningMotions()
        {
            while (animotionParameter.isOver(transform) == false)
            {
                //Debug.Log("CheckPendingRunningMotions Tweener");
                yield return null;
            }
            //while (transform.position != finishPosition)
            //{
            //    //Debug.Log("Wait for End Animation Time Own Animator Check");
            //    yield return null;
            //}
        }

        public override void OnMotionSkip()
        {
            animotionParameter.End(actualTween, transform);
            base.OnMotionSkip();
        }

        protected override void CheckMotionAfterEnd()
        {
            OnMotionSkip();
        }

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

    }
}

