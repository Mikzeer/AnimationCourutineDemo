using UnityEngine;
namespace PositionerDemo
{
    public class MovePathTweenMotion : TweenMotion
    {
        int tweenDuration = 20;

        public MovePathTweenMotion(MonoBehaviour coroutineMono, Transform transform, int reproductionOrder, Vector3[] pathPositions) : base(coroutineMono, transform, reproductionOrder)
        {
            animotionParameter = new TweenAnimotionParameter(new TweenDoPathReproducer(transform, pathPositions), ease, tweenDuration);
        }
    }
}