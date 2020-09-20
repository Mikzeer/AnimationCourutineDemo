using UnityEngine;
namespace PositionerDemo
{
    public class ScaleRectTweenMotion : TweenMotion
    {

        public ScaleRectTweenMotion(MonoBehaviour coroutineMono, RectTransform transform, int reproductionOrder, Vector3 endPostion, int tweenDuration = 20) : base(coroutineMono, transform, reproductionOrder)
        {
            animotionParameter = new TweenAnimotionParameter(new TweenDoRectScaleReproduce(transform, endPostion), ease, tweenDuration);
        }

        public override bool CheckCorrectInput()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                return true;
            }
            return false;
        }
    }

}

