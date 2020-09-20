using DG.Tweening;
using UnityEngine;
namespace PositionerDemo
{
    public class TweenDoRectScaleReproduce : TweenReproducer
    {
        Vector3 finalScale;
        RectTransform rectTransform;
        public TweenDoRectScaleReproduce(RectTransform transform, Vector3 finalScale) : base(transform)
        {
            this.finalScale = finalScale;
            this.rectTransform = transform;
        }

        public override Tweener Apply(Transform transform, Ease ease, int duration)
        {
            return rectTransform.DOScale(finalScale, duration).SetEase(ease);
        }

        public override bool isOver(Transform transform)
        {
            if (rectTransform.localScale != finalScale) return false;

            return true;
        }

        public override void End(Tween actualTween, Transform transform)
        {
            actualTween.Kill();
            rectTransform.localScale = finalScale;
        }
    }

}

