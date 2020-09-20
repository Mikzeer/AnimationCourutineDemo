using DG.Tweening;
using UnityEngine;
namespace PositionerDemo
{
    public class TweenDoMoveReproduce : TweenReproducer
    {
        Vector3 finalPosition;
        public TweenDoMoveReproduce(Transform transform, Vector3 finalPosition) : base(transform)
        {
            this.finalPosition = finalPosition;
        }

        public override Tweener Apply(Transform transform,Ease ease, int duration)
        {
            return transform.DOMove(finalPosition, duration).SetEase(ease);//.OnComplete(() => Debug.Log("TERMINE CON CALLBAKC"));


            //// Callback without parameters
            //transform.DOMoveX(4, 1).OnComplete(MyCallback);
            //// Callback with parameters
            //transform.DOMoveX(4, 1).OnComplete(() => MyCallback(someParam, someOtherParam));

        }

        public override bool isOver(Transform transform)
        {
            if (transform.position != finalPosition) return false;

            return true;
        }

        public override void End(Tween actualTween, Transform transform)
        {
            actualTween.Kill();
            transform.position = finalPosition;
        }
    }

}

