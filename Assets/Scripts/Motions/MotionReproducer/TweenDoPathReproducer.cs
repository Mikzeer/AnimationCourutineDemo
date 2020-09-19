using DG.Tweening;
using UnityEngine;
namespace PositionerDemo
{
    public class TweenDoPathReproducer : TweenReproducer
    {
        Vector3[] pathPositions;
        public TweenDoPathReproducer(Transform transform, Vector3[] pathPositions) : base(transform)
        {
            this.pathPositions = pathPositions;
        }

        public override Tweener Apply(Transform transform,Ease ease, int duration)
        {
            return transform.DOPath(pathPositions, duration).SetEase(ease);
        }

        public override bool isOver(Transform transform)
        {
            if (transform.position != pathPositions[pathPositions.Length - 1]) return false;

            return true;
        }

        public override void End(Tween actualTween, Transform transform)
        {
            actualTween.Kill();
            transform.position = pathPositions[pathPositions.Length - 1];
        }
    }

}

