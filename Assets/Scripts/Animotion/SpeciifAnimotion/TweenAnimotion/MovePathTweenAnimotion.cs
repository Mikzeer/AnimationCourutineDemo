using UnityEngine;
using PositionerDemo;

namespace MikzeerGame.Animotion
{
    #region TWEEN MOTION

    public class MovePathTweenAnimotion : TweenAnimotion
    {
        public MovePathTweenAnimotion(MonoBehaviour coroutineMono, int reproductionOrder, Transform transform, Vector3[] pathPositions, int tweenDuration = 20) : base(coroutineMono, reproductionOrder, transform)
        {
            tweenParameter = new TweenParameter(new TweenDoPathReproducer(transform, pathPositions), ease, tweenDuration);
        }
    }

    #endregion
}