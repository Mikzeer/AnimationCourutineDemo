using DG.Tweening;
using UnityEngine;
namespace PositionerDemo
{
    public class SpawnCardTweenMotion : TweenMotion
    {
        Ease spawnEase = Ease.InOutBack;
        public SpawnCardTweenMotion(MonoBehaviour coroutineMono, Transform transform, int reproductionOrder, Vector3 endPostion, int tweenDuration = 20) : base(coroutineMono, transform, reproductionOrder)
        {
            //animotionParameter = new TweenAnimotionParameter(new TweenDoMoveReproduce(transform, endPostion), ease, tweenDuration);
            animotionParameter = new TweenAnimotionParameter(new TweenDoMoveReproduce(transform, endPostion), spawnEase, tweenDuration);
        }

    }
}

