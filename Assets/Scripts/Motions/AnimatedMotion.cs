using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PositionerDemo
{
    public abstract class AnimatedMotion : Motion
    {
        protected AnimatorStateInfo animatorStateInfo;
        protected Animator animator;

        protected float animationNormalSpeed = 1;
        public float animationSpeedUpVelocity { get; protected set; }
        private const string animationSpeedParameterString = "AnimationSpeed";

        protected AnimationAnimotionParameter animotionParameter; 

        public AnimatedMotion(MonoBehaviour coroutineMono, Animator animator, int reproductionOrder) : base(coroutineMono, reproductionOrder)
        {
            animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
            this.animator = animator;       
        }

        protected override void StartMotion()
        {
            animotionParameter.Reproduce(animator);
        }

        protected override IEnumerator CheckPendingRunningMotions()
        {
            // PUEDO CHEQUEAR POR NOMBRE
            // PUEDO CHEQUAR TAMBIEN POR SI FINALIZO... DEBERIA BUSCARLO

            while (animatorStateInfo.shortNameHash != animator.GetCurrentAnimatorStateInfo(0).shortNameHash)
            {
                //Debug.Log("CheckPendingRunningMotions");
                yield return null;
                
            }
            //while (!animator.GetCurrentAnimatorStateInfo(0).IsName(animParameter.skipParameterString))
            //{
            //    //Debug.Log("CheckPendingRunningMotions");
            //    yield return null;
            //}
        }

        public override void OnMotionSkip()
        {
            animotionParameter.End(animator);
        }

        protected override void CheckMotionAfterEnd()
        {
            OnMotionSkip();
        }

        protected override void SpeedUpMotionOnMotion()
        {
            animationSpeedUpVelocity = Mathf.FloorToInt(speed);
            animator.SetFloat(animationSpeedParameterString, animationSpeedUpVelocity);
            //enemies[i].animator.SetFloat(animationSpeedParameter, animationSpeedUpVelocity);
            //entry.Key.animator.SetFloat(animationSpeedParameter, animationSpeedUpVelocity);
            //craneAnimator.SetFloat(animationSpeedParameter, animationSpeedUpVelocity);
            //kimbokoAnimator.SetFloat(animationSpeedParameter, animationSpeedUpVelocity);
        }

        protected override void SetNormalSpeedInMotion()
        {
            animator.SetFloat(animationSpeedParameterString, animationNormalSpeed);
            //enemies[i].animator.SetFloat(animationSpeedParameter, animationNormalSpeed);
            //entry.Key.animator.SetFloat(animationSpeedParameter, animationNormalSpeed);
            //craneAnimator.SetFloat(animationSpeedParameter, animationNormalSpeed);
            //kimbokoAnimator.SetFloat(animationSpeedParameter, animationNormalSpeed);
        }

    }

    public abstract class TweenMotion : Motion
    {
        private Transform transform;
        protected Ease ease = Ease.Linear;
        private Tween actualTween;

        protected float tweenNormalSpeed = 1;
        public float tweenSpeedUp { get; protected set; }
        public float tweenActualSpeed { get; protected set; }

        protected TweenAnimotionParameter animotionParameter;

        public TweenMotion(MonoBehaviour coroutineMono, Transform transform, int reproductionOrder) : base(coroutineMono, reproductionOrder)
        {
            tweenActualSpeed = tweenNormalSpeed;
            this.transform = transform;
        }

        protected override void StartMotion()
        {
            //movingTween = transform.DOMove(endPostion, 20).SetEase(ease);

            //movingTween[index] = entry.Key.transform.DOPath(entry.Value, 10).SetEase(ease);

            actualTween = animotionParameter.Reproduce(transform);

            actualTween.timeScale = tweenActualSpeed;
        }

        protected override IEnumerator CheckPendingRunningMotions()
        {
            while (animotionParameter.isOver(transform) == false)
            {
                //Debug.Log("CheckPendingRunningMotions Tweener");
                yield return null;
            }

            //while (transform.position != finishPosition)
            //{
            //    //Debug.Log("Wait for End Animation Time Own Animator Check");
            //    yield return null;
            //}
        }

        public override void OnMotionSkip()
        {
            animotionParameter.End(actualTween, transform);
        }

        protected override void CheckMotionAfterEnd()
        {
            OnMotionSkip();
        }

        protected override void SpeedUpMotionOnMotion()
        {
            tweenSpeedUp = Mathf.FloorToInt(speed * 2);

            if (actualTween == null) return;

            actualTween.timeScale = tweenSpeedUp;

            //movingTween[index].timeScale = tweenSpeedUp;

            //if (craneTween == null) return;
            //craneTween.timeScale = tweenSpeedUp;
            //if (spawnKimbokoTween == null) return;
            //spawnKimbokoTween.timeScale = tweenSpeedUp;


            tweenActualSpeed = tweenSpeedUp;
        }

        protected override void SetNormalSpeedInMotion()
        {
            if (actualTween == null) return;

            actualTween.timeScale = tweenNormalSpeed;

            //movingTween[index].timeScale = tweenNormalSpeed;

            //if (craneTween == null) return;
            //craneTween.timeScale = tweenNormalSpeed;
            //if (spawnKimbokoTween == null) return;
            //spawnKimbokoTween.timeScale = tweenNormalSpeed;

            tweenActualSpeed = tweenNormalSpeed;
        }

    }

    public class CombineMotion : Motion
    {
        public List<Motion> motions;
        public List<Configurable> configureAnimotion;
        private int actualMotionIndex = 1;
        private int currentPerformingIndex;

        public CombineMotion(MonoBehaviour coroutineMono, int reproductionOrder, List<Motion> motions, List<Configurable> configureAnimotion = null) : base(coroutineMono, reproductionOrder)
        {
            this.motions = motions;
            if (configureAnimotion != null)
            {
                this.configureAnimotion = configureAnimotion;
            }
        }

        protected override void StartMotion()
        {
            if (motions.Count <= 0) return;

            bool notStarted = false;

            while (notStarted == false)
            {
                if (configureAnimotion != null)
                {
                    for (int i = configureAnimotion.Count; i >= 0; i--)
                    {
                        if (configureAnimotion[i].configureOrder == actualMotionIndex)
                        {
                            configureAnimotion[i].Configure();
                            configureAnimotion.RemoveAt(i);
                        }
                    }
                }


                for (int i = 0; i < motions.Count; i++)
                {
                    if (motions[i].reproductionOrder == actualMotionIndex)
                    {
                        motions[i].CheckMotionBeforeStart();
                        notStarted = true;
                    }
                }

                if (notStarted == false)
                {
                    currentPerformingIndex = actualMotionIndex;
                    actualMotionIndex++;
                }
            }

            currentPerformingIndex = actualMotionIndex;
            actualMotionIndex++;
        }

        protected override IEnumerator CheckPendingRunningMotions()
        {
            bool hasAllEnded = false;

            while (hasAllEnded)
            {
                if (configureAnimotion != null)
                {
                    for (int i = configureAnimotion.Count; i >= 0; i--)
                    {
                        if (configureAnimotion[i].configureOrder == actualMotionIndex)
                        {
                            configureAnimotion[i].Configure();
                            configureAnimotion.RemoveAt(i);
                        }
                    }
                }




                bool hasMatch = false;
                int totalReproducingAnimotion = 0;
                int totalEndedReproducingAnimotion = 0;
                // recorro todas las motions
                // segun el index de lo que deberia estar ejecutandose es lo que voy a revisar si se termino, sino, hago un yield return null
                for (int i = motions.Count; i >= 0; i--)
                {
                    if (motions[i].reproductionOrder == currentPerformingIndex && !motions[i].performing)
                    {
                        motions.RemoveAt(i);
                        totalReproducingAnimotion++;
                        totalEndedReproducingAnimotion++;
                    }
                    else if (motions[i].reproductionOrder == currentPerformingIndex && motions[i].performing)
                    {
                        totalReproducingAnimotion++;
                    }
                }

                // Si todos los animotion de ese index estaban finalizados, entonces tenemos un match
                if (totalReproducingAnimotion == totalEndedReproducingAnimotion && totalEndedReproducingAnimotion > 0)
                {
                    hasMatch = true;

                    if (motions.Count == 0)
                    {
                        hasAllEnded = true;

                        if (configureAnimotion != null)
                        {
                            for (int i = configureAnimotion.Count; i >= 0; i--)
                            {
                                configureAnimotion[i].Configure();
                                configureAnimotion.RemoveAt(i);
                            }
                        }


                        yield break;
                    }
                }

                // si ya no quedan mas motions es que ya estan todas finalizadas
                if (motions.Count == 0)
                {
                    hasAllEnded = true;

                    if (configureAnimotion != null)
                    {
                        for (int i = configureAnimotion.Count; i >= 0; i--)
                        {
                            configureAnimotion[i].Configure();
                            configureAnimotion.RemoveAt(i);
                        }
                    }


                    yield break;
                }

                if (hasMatch == false)
                {
                    bool started = false;

                    currentPerformingIndex = actualMotionIndex;
                    actualMotionIndex++;

                    while (started == false)
                    {

                        if (configureAnimotion != null)
                        {
                            for (int i = configureAnimotion.Count; i >= 0; i--)
                            {
                                if (configureAnimotion[i].configureOrder == actualMotionIndex)
                                {
                                    configureAnimotion[i].Configure();
                                    configureAnimotion.RemoveAt(i);
                                }
                            }
                        }




                        for (int i = 0; i < motions.Count; i++)
                        {
                            if (motions[i].reproductionOrder == actualMotionIndex)
                            {
                                motions[i].CheckMotionBeforeStart();
                                started = true;
                            }
                        }

                        if (started == false)
                        {
                            currentPerformingIndex = actualMotionIndex;
                            actualMotionIndex++;
                        }

                        yield return null;
                    }
                }
                else
                {
                    yield return null;
                }
            }
        }

        public override void OnMotionSkip()
        {
            for (int i = 0; i < motions.Count; i++)
            {
                motions[i].OnMotionSkip();
            }
        }

        protected override void CheckMotionAfterEnd()
        {
            OnMotionSkip();
        }

        protected override void SpeedUpMotionOnMotion()
        {
            for (int i = 0; i < motions.Count; i++)
            {
                if (motions[i].reproductionOrder == actualMotionIndex)
                {
                    motions[i].SpeedUpMotion(speed);
                }
            }
        }

        protected override void SetNormalSpeedInMotion()
        {
            for (int i = 0; i < motions.Count; i++)
            {
                if (motions[i].reproductionOrder == actualMotionIndex)
                {
                    motions[i].SetNormalSpeedMotion();
                }
            }
        }
    }



    public interface Configurable
    {
        int configureOrder { get; }
        void Configure();
    }

    public abstract class ConfigureAnimotion<T,O> : Configurable
    {
        protected T firstConfigure;
        protected O secondConfigure;

        public int configureOrder { get; private set;}

        public ConfigureAnimotion(T firstConfigure, O secondConfigure, int configureOrder)
        {
            this.firstConfigure = firstConfigure;
            this.secondConfigure = secondConfigure;
            this.configureOrder = configureOrder;
        }

        public ConfigureAnimotion(T firstConfigure, int configureOrder)
        {
            this.firstConfigure = firstConfigure;
            this.configureOrder = configureOrder;
        }

        public virtual void Configure()
        {

        }

    }

    public class KimbokoPositioConfigureAnimotion<T,O> : ConfigureAnimotion<T, O> where T : Transform where O : Transform
    {
        public KimbokoPositioConfigureAnimotion(T firstConfigure, O secondConfigure, int configureOrder) : base(firstConfigure, secondConfigure, configureOrder)
        {
        }

        public override void Configure()
        {
            firstConfigure.position = secondConfigure.position;
            firstConfigure.gameObject.SetActive(true);
        }
    }

    public class CraneActiveConfigureAnimotion<T, O> : ConfigureAnimotion<T, O> where T : Transform where O : Transform
    {
        public CraneActiveConfigureAnimotion(T firstConfigure, int configureOrder) : base(firstConfigure, configureOrder)
        {
        }

        public override void Configure()
        {
            firstConfigure.gameObject.SetActive(false);
        }
    }



    public class TweenAnimotionParameter
    {
        TweenReproducer tweenReproducer;
        private Ease ease = Ease.Linear;
        private int tweenDuration;

        public TweenAnimotionParameter(TweenReproducer tweenReproducer, Ease ease, int tweenDuration)
        {
            this.tweenReproducer = tweenReproducer;
            this.ease = ease;
            this.tweenDuration = tweenDuration;
        }

        public Tween Reproduce(Transform transform)
        {
            return tweenReproducer.Apply(transform, ease, tweenDuration);
        }

        public bool isOver(Transform transform)
        {
            return tweenReproducer.isOver(transform);
        }

        public void End(Tween actualTween, Transform transform)
        {
            tweenReproducer.End(actualTween, transform);
        }

    }

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

    public class TweenDoMoveReproduce : TweenReproducer
    {
        Vector3 finalPosition;
        public TweenDoMoveReproduce(Transform transform, Vector3 finalPosition) : base(transform)
        {
            this.finalPosition = finalPosition;
        }

        public override Tweener Apply(Transform transform,Ease ease, int duration)
        {
            return transform.DOMove(finalPosition, duration).SetEase(ease);
        }

        public override bool isOver(Transform transform)
        {
            if (transform.position != finalPosition) return false;

            return true;
        }

        public override void End(Tween actualTween, Transform transform)
        {
            actualTween.Kill();
            transform.position = finalPosition;
        }
    }

    public class TweenDoPathReproducer : TweenReproducer
    {
        Vector3[] pathPositions;
        public TweenDoPathReproducer(Transform transform, Vector3[] pathPositions) : base(transform)
        {
            this.pathPositions = pathPositions;
        }

        public override Tweener Apply(Transform transform,Ease ease, int duration)
        {
            return transform.DOPath(pathPositions, duration).SetEase(ease);
        }

        public override bool isOver(Transform transform)
        {
            if (transform.position != pathPositions[pathPositions.Length - 1]) return false;

            return true;
        }

        public override void End(Tween actualTween, Transform transform)
        {
            actualTween.Kill();
            transform.position = pathPositions[pathPositions.Length - 1];
        }
    }



    public class AnimationAnimotionParameter
    {
        AnimationReproducer startReproducer;
        AnimationReproducer endReproducer;

        public AnimationAnimotionParameter(AnimationReproducer startReproducer, AnimationReproducer endReproducer)
        {
            this.startReproducer = startReproducer;
            this.endReproducer = endReproducer;
        }

        public void Reproduce(Animator animator)
        {
            startReproducer.Apply(animator);
        }

        public void End(Animator animator)
        {
            endReproducer.Apply(animator);
        }

    }

    public abstract class AnimationReproducer
    {
        // Nombre del parametro que va a activar esta animacion
        protected string activationParameterString;

        public AnimationReproducer(string activationParameterString)
        {

        }
        public virtual void Apply(Animator animator)
        {

        }
    }
   
    public class AnimationBoolReproducer : AnimationReproducer
    {
        public AnimationBoolReproducer(string activationParameterString) : base(activationParameterString)
        {
        }

        public override void Apply(Animator animator)
        {
            animator.SetBool(activationParameterString, true);
        }
    }

    public class AnimationTriggerReproducer : AnimationReproducer
    {
        public AnimationTriggerReproducer(string activationParameterString) : base(activationParameterString)
        {
        }

        public override void Apply(Animator animator)
        {
            animator.SetTrigger(activationParameterString);
        }
    }

    public class AnimationIntReproducer : AnimationReproducer
    {
        private int parameterValue;
        public AnimationIntReproducer(string activationParameterString, int parameterValue) : base(activationParameterString)
        {
            this.parameterValue = parameterValue;
        }

        public override void Apply(Animator animator)
        {
            animator.SetInteger(activationParameterString, parameterValue);
        }
    }

    public class AnimationFloatReproducer : AnimationReproducer
    {
        private float parameterValue;
        public AnimationFloatReproducer(string activationParameterString, float parameterValue) : base(activationParameterString)
        {
            this.parameterValue = parameterValue;
        }

        public override void Apply(Animator animator)
        {
            animator.SetFloat(activationParameterString, parameterValue);
        }
    }

}

