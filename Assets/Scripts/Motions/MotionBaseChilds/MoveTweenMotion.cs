using UnityEngine;
namespace PositionerDemo
{
    public class MoveTweenMotion : TweenMotion
    {
        public MoveTweenMotion(MonoBehaviour coroutineMono, Transform transform, int reproductionOrder, Vector3 endPostion, int tweenDuration = 20) : base(coroutineMono, transform, reproductionOrder)
        {
            animotionParameter = new TweenAnimotionParameter(new TweenDoMoveReproduce(transform, endPostion), ease, tweenDuration);
        }
    }
}