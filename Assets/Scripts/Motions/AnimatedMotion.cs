using DG.Tweening;
using UnityEngine;
namespace PositionerDemo
{
    public abstract class AnimatedMotion : Motion
    {
        protected AnimatorStateInfo animst;
        protected Animator animator;
        private const string animationSpeedParameterString = "AnimationSpeed";
        public AnimatedMotion(MonoBehaviour coroutineMono, Animator animator) : base(coroutineMono)
        {
            animst = animator.GetCurrentAnimatorStateInfo(0);
            this.animator = animator;
            tweenActualSpeed = tweenNormalSpeed;            
        }

        protected override void SpeedUpMotionOnMotion()
        {
            //if (animationSpeedParameterString == null) return;

            animationSpeedUpVelocity = Mathf.FloorToInt(speed);

            animator.SetFloat(animationSpeedParameterString, animationSpeedUpVelocity);
            //enemies[i].animator.SetFloat(animationSpeedParameter, animationSpeedUpVelocity);
            //animator.SetFloat(animationSpeedParameter, animationSpeedUpVelocity);
            //entry.Key.animator.SetFloat(animationSpeedParameter, animationSpeedUpVelocity);
            //craneAnimator.SetFloat(animationSpeedParameter, animationSpeedUpVelocity);
            //kimbokoAnimator.SetFloat(animationSpeedParameter, animationSpeedUpVelocity);
        }

        protected override void SetNormalSpeedInMotion()
        {
            animator.SetFloat(animationSpeedParameter, animationNormalSpeed);
            //enemies[i].animator.SetFloat(animationSpeedParameter, animationNormalSpeed);
            //animator.SetFloat(animationSpeedParameter, animationNormalSpeed);
            //entry.Key.animator.SetFloat(animationSpeedParameter, animationNormalSpeed);
            //craneAnimator.SetFloat(animationSpeedParameter, animationNormalSpeed);
            //kimbokoAnimator.SetFloat(animationSpeedParameter, animationNormalSpeed);
        }

    }

    public abstract class TweenMotion : Motion
    {
        private Tween movingTween;
        private Ease ease = Ease.Linear;

        public TweenMotion(MonoBehaviour coroutineMono) : base(coroutineMono)
        {
        }

        protected override void SpeedUpMotionOnMotion()
        {
            tweenSpeedUp = Mathf.FloorToInt(speed * 2);

            if (movingTween == null) return;

            movingTween.timeScale = tweenSpeedUp;

            //movingTween[index].timeScale = tweenSpeedUp;

            //if (craneTween == null) return;
            //craneTween.timeScale = tweenSpeedUp;
            //if (spawnKimbokoTween == null) return;
            //spawnKimbokoTween.timeScale = tweenSpeedUp;


            tweenActualSpeed = tweenSpeedUp;
        }

        protected override void SetNormalSpeedInMotion()
        {
            if (movingTween == null) return;

            movingTween.timeScale = tweenNormalSpeed;

            //movingTween[index].timeScale = tweenNormalSpeed;

            //if (craneTween == null) return;
            //craneTween.timeScale = tweenNormalSpeed;
            //if (spawnKimbokoTween == null) return;
            //spawnKimbokoTween.timeScale = tweenNormalSpeed;

            tweenActualSpeed = tweenNormalSpeed;
        }

    }

    public abstract class CombineMotion : Motion
    {
        public Motion firstMotion;
        public Motion secondMotion;

        public CombineMotion(MonoBehaviour coroutineMono) : base(coroutineMono)
        {
        }

        protected override void SpeedUpMotionOnMotion()
        {
            firstMotion.SpeedUpMotion(speed);
            secondMotion.SpeedUpMotion(speed);
        }

        protected override void SetNormalSpeedInMotion()
        {
            firstMotion.SetNormalSpeedMotion();
            secondMotion.SetNormalSpeedMotion();
        }
    }




    public class AnimationAnimotionParameter<T>
    {
        // Nombre del parametro de la Animacion para activar
        public string activationParameterString { get; private set; }
        // Nombre del parametro de la Animacion para skip
        public string skipParameterString { get; private set; }
        // Valor del paramatro para setear
        public T parameterValue;
        // Set float / Trigger / Bool / Integer
        public AnimationParameterType parameterType { get; private set; }
        // Orden en el que se deberia reproducir
        public int reproductionOrder { get; private set; }

        public AnimationAnimotionParameter(string activationParameterString, string skipParameterString, AnimationParameterType parameterType, int reproductionOrder)
        {
            this.activationParameterString = activationParameterString;
            this.skipParameterString = skipParameterString;
            this.parameterType = parameterType;
            this.reproductionOrder = reproductionOrder;
        }

        public AnimationAnimotionParameter(string activationParameterString, string skipParameterString, AnimationParameterType parameterType, int reproductionOrder, T parameterValue)
        {
            this.activationParameterString = activationParameterString;
            this.skipParameterString = skipParameterString;
            this.parameterType = parameterType;
            this.reproductionOrder = reproductionOrder;
            this.parameterValue = parameterValue;
        }

        public void SetValue(T parameterValue)
        {
            this.parameterValue = parameterValue;
        }

    }

    public enum AnimationParameterType
    {
        FLOAT,
        TRIGGER,
        BOOL,
        INTEGER            
    }
        


}

