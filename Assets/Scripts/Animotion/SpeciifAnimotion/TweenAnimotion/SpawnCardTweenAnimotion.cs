using UnityEngine;
using PositionerDemo;
using DG.Tweening;

namespace MikzeerGame.Animotion
{
    #region TWEEN MOTION

    public class SpawnCardTweenAnimotion : TweenAnimotion
    {
        public SpawnCardTweenAnimotion(MonoBehaviour coroutineMono, int reproductionOrder, Transform transform, Vector3 endPostion, int tweenDuration = 20) : base(coroutineMono, reproductionOrder, transform)
        {
            ease = Ease.InOutBack;
            tweenParameter = new TweenParameter(new TweenDoMoveReproduce(transform, endPostion), ease, tweenDuration);
        }
    }

    #endregion
}