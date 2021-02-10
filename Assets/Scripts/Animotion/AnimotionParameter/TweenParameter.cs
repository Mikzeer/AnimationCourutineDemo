using UnityEngine;
using PositionerDemo;
using DG.Tweening;

namespace MikzeerGame.Animotion
{
    #region ANIMOTION PARAMETERS

    public class TweenParameter
    {
        TweenReproducer tweenReproducer;
        private Ease ease = Ease.Linear;
        private int tweenDuration;

        public TweenParameter(TweenReproducer tweenReproducer, Ease ease, int tweenDuration)
        {
            this.tweenReproducer = tweenReproducer;
            this.ease = ease;
            this.tweenDuration = tweenDuration;
        }

        public Tween Reproduce(Transform transform)
        {
            return tweenReproducer.Apply(transform, ease, tweenDuration);
        }

        public bool isOver(Transform transform)
        {
            return tweenReproducer.isOver(transform);
        }

        public void End(Tween actualTween, Transform transform)
        {
            tweenReproducer.End(actualTween, transform);
        }

    }

    #endregion
}