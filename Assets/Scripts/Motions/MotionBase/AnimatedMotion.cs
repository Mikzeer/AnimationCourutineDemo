using System.Collections;
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

        public bool isSpecialCheck;

        protected AnimationAnimotionParameter animotionParameter;

        int fullPathHash;

        public AnimatedMotion(MonoBehaviour coroutineMono, Animator animator, int reproductionOrder, bool isSpecialCheck = false) : base(coroutineMono, reproductionOrder)
        {
            //AnimatorControllerParameter[] animatorControllParameters = animator.parameters;
            //AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
            //int d = animator.GetInstanceID();
            //int j = animator.GetLayerIndex("Base Layer");
            //int pipo = Animator.StringToHash(layerName + ".Idlle");

            // ESTE ES EL NUEVO STATE DONDE QUEREMOS ESTAR
            //string animLayerName = "Base Layer";
            string animLayerName = animator.GetLayerName(0);
            string animationFileName = ".Spawn_Animation";

            fullPathHash = Animator.StringToHash(animLayerName + animationFileName);

            animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);

            this.animator = animator;
            this.isSpecialCheck = isSpecialCheck;
        }

        protected override void StartMotion()
        {
            animotionParameter.Reproduce(animator);
        }

        protected override IEnumerator CheckPendingRunningMotions()
        {
            // Wait until we enter the current state
            // VAMOS A ESPERAR PRIMERO A ESTAR EN EL NUEVO STATE, SIEMPRE VAMOS A ESPERAR ESTO, SINO NO TENDRIA MUCHO SENTIDO
            // CON ESTO EVITAMOS QUE EN EL PRIMER FRAME DE CHEQUEO O ANTES DE ESE FRAME SI LLEGARAMOS A CHEQUEAR
            // NOS VA A DAR QUE ESTAMOS EN OTRO ANIMATION STATE, ENTONCES COMO YA NO ES IGUAL AL NUEVO TERMINA LA MOTION
            //while (animator.GetCurrentAnimatorStateInfo(0).fullPathHash != fullPathHash)
            //{
            //    yield return null;
            //}



            if (isSpecialCheck == false)
            {
                while (animatorStateInfo.shortNameHash != animator.GetCurrentAnimatorStateInfo(0).shortNameHash)
                {
                    //Debug.Log("animateeeed");
                    yield return null;
                }
            }

            //if (isSpecialCheck == false)
            //{
            //    while (animatorStateInfo.fullPathHash != animator.GetCurrentAnimatorStateInfo(0).fullPathHash)
            //    {
            //        yield return null;
            //    }
            //}

            //while (!animator.GetCurrentAnimatorStateInfo(0).IsName(animParameter.skipParameterString))
            //{
            //    //Debug.Log("CheckPendingRunningMotions");
            //    yield return null;
            //}
        }

        public override void OnMotionSkip()
        {
            animotionParameter.End(animator);
            base.OnMotionSkip();
        }

        protected override void CheckMotionAfterEnd()
        {
            OnMotionSkip();
        }

        protected override void SpeedUpMotionOnMotion()
        {
            animationSpeedUpVelocity = Mathf.FloorToInt(speed);
            animator.SetFloat(animationSpeedParameterString, animationSpeedUpVelocity);
        }

        protected override void SetNormalSpeedInMotion()
        {
            animator.SetFloat(animationSpeedParameterString, animationNormalSpeed);
        }
    }
}