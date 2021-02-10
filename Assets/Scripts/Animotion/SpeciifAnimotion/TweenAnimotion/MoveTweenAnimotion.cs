using UnityEngine;
using PositionerDemo;

namespace MikzeerGame.Animotion
{
    #region TWEEN MOTION

    public class MoveTweenAnimotion : TweenAnimotion
    {
        public MoveTweenAnimotion(MonoBehaviour coroutineMono, int reproductionOrder, Transform transform, Vector3 endPostion, int tweenDuration = 20) : base(coroutineMono, reproductionOrder, transform)
        {
            tweenParameter = new TweenParameter(new TweenDoMoveReproduce(transform, endPostion), ease, tweenDuration);
        }
    }

    #endregion
}