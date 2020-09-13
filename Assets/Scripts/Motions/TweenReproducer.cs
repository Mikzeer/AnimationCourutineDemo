using DG.Tweening;
using UnityEngine;
namespace PositionerDemo
{
    public abstract class TweenReproducer
    {

        public TweenReproducer(Transform transform)
        {

        }

        public virtual Tweener Apply(Transform transform, Ease ease, int duration)
        {
            return null;
        }

        public virtual bool isOver(Transform transform)
        {
            return true;
        }

        public virtual void End(Tween actualTween, Transform transform)
        {
            
        }
    }

}

