using UnityEngine;
using PositionerDemo;

namespace MikzeerGame.Animotion
{
    #region TWEEN MOTION

    public class ScaleRectTweenAnimotion : TweenAnimotion
    {
        public ScaleRectTweenAnimotion(MonoBehaviour coroutineMono, int reproductionOrder, RectTransform transform, Vector3 endPostion, int tweenDuration = 20) : base(coroutineMono, reproductionOrder, transform)
        {
            tweenParameter = new TweenParameter(new TweenDoRectScaleReproduce(transform, endPostion), ease, tweenDuration);
        }
    }

    #endregion
}