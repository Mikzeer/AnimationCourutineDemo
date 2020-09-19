using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PositionerDemo
{
    public class AttackMotion : AnimatedMotion
    {
        private const string attackTriggerString = "Active";
        private const string SkipTriggerString = "Idlle";


        //List<Enemy> enemies;
        //private const string damageTriggerString = "Damage";
        //private AnimatorStateInfo[] damageAnimst;


        public AttackMotion(MonoBehaviour coroutineMono, Animator animator, int reproductionOrder) : base(coroutineMono, animator, reproductionOrder)
        {
            animotionParameter = new AnimationAnimotionParameter(new AnimationTriggerReproducer(attackTriggerString), new AnimationTriggerReproducer(SkipTriggerString));
        }

        public override bool CheckCorrectInput()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                return true;
            }

            return false;
        }

        //protected override void StartMotion()
        //{
        //    animator.SetTrigger(attackTriggerString);

        //    damageAnimst = new AnimatorStateInfo[enemies.Count];

        //    for (int i = 0; i < enemies.Count; i++)
        //    {
        //        damageAnimst[i] = enemies[i].animator.GetCurrentAnimatorStateInfo(0);
        //        enemies[i].animator.SetTrigger(damageTriggerString);
        //    }
        //}

        //protected override IEnumerator CheckPendingRunningMotions()
        //{
        //    while (animst.shortNameHash != animator.GetCurrentAnimatorStateInfo(0).shortNameHash)
        //    {
        //        //Debug.Log("Wait for End Animation Time Own Animator Check");
        //        yield return null;
        //    }


        //    for (int i = 0; i < enemies.Count; i++)
        //    {
        //        //// Tambien esperamos que termine la animacion de Damage de los enemigos
        //        //while (!enemies[i].animator.GetCurrentAnimatorStateInfo(0).IsName("Idlle"))
        //        //{
        //        //    //Debug.Log("Wait for End Animation Time Enemies List Check");
        //        //    yield return null;
        //        //}
        //        // Tambien esperamos que termine la animacion de Damage de los enemigos
        //        while (damageAnimst[i].shortNameHash != enemies[i].animator.GetCurrentAnimatorStateInfo(0).shortNameHash)
        //        {
        //            //Debug.Log("Somos los enemios Recibiendo dano");
        //            yield return null;
        //        }


        //    }
        //}

        //protected override void OnMotionSkip()
        //{
        //    animator.SetTrigger("Idlle");

        //    for (int i = 0; i < enemies.Count; i++)
        //    {
        //        enemies[i].animator.SetTrigger("Idlle");
        //    }
        //    base.OnMotionSkip();
        //}

        //protected override void CheckMotionAfterEnd()
        //{
        //    // ACA PODRIA VER SI ESTA EN ESTADO IDLLE Y SI NO PONERLO EN ESE ESTADO
        //}

        //protected override void SpeedUpMotionOnMotion()
        //{
        //    animator.SetFloat(animationSpeedParameter, animationSpeedUpVelocity);

        //    for (int i = 0; i < enemies.Count; i++)
        //    {
        //        enemies[i].animator.SetFloat(animationSpeedParameter, animationSpeedUpVelocity);
        //    }
        //}

        //protected override void SetNormalSpeedInMotion()
        //{
        //    animator.SetFloat(animationSpeedParameter, animationNormalSpeed);

        //    for (int i = 0; i < enemies.Count; i++)
        //    {
        //        enemies[i].animator.SetFloat(animationSpeedParameter, animationNormalSpeed);
        //    }
        //}

    }

    public class ShieldMotion : AnimatedMotion
    {
        private const string attackTriggerString = "Active";
        private const string SkipTriggerString = "Idlle";

        int fullPathHash;
        float waitTime;
        public ShieldMotion(MonoBehaviour coroutineMono, Animator animator, int reproductionOrder) : base(coroutineMono, animator, reproductionOrder, true)
        {
            animotionParameter = new AnimationAnimotionParameter(new AnimationTriggerReproducer(attackTriggerString), new AnimationTriggerReproducer(SkipTriggerString));

            AnimatorControllerParameter[] animatorControllParameters = animator.parameters;
            AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
            //int d = animator.GetInstanceID();
            //int j = animator.GetLayerIndex("Base Layer");
            string layerName = animator.GetLayerName(0);

            //int pipo = Animator.StringToHash(layerName + ".Idlle");

            fullPathHash = Animator.StringToHash(layerName + ".Active");
            waitTime = animator.GetCurrentAnimatorStateInfo(0).length;


        }

        protected override IEnumerator CheckPendingRunningMotions()
        {
            //while (fullPathHash != animator.GetCurrentAnimatorStateInfo(0).fullPathHash)
            //{
            //    yield return null;
            //}

            float counter = 0;
            
            //Now, Wait until the current state is done playing
            while (counter < (waitTime))
            {
                counter += Time.deltaTime;
                yield return null;
            }

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

