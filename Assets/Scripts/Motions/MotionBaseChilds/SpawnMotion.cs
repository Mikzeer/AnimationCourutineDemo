using DG.Tweening;
using System.Collections;
using UnityEngine;
namespace PositionerDemo
{
    public class SpawnMotion : AnimatedMotion
    {
        private const string SkipTriggerString = "Idlle";
        private const string spawnTriggerString = "Spawn";

        //private Animator craneAnimator;
        //private Animator kimbokoAnimator;
        ////private Ease ease = Ease.Linear;
        //private GameObject crane;
        //private GameObject spawnKimboko;
        //Vector3 spawnPosition;
        //private Vector3 craneStartPosition;
        //private Vector3 craneEndPostion;
        //private Tween craneTween;
        //private Tween spawnKimbokoTween;
        //private Transform craneEnd;

        public SpawnMotion(MonoBehaviour coroutineMono, Animator animator, int reproductionOrder) : base(coroutineMono, animator, reproductionOrder)
        {
            animotionParameter = new AnimationAnimotionParameter(new AnimationTriggerReproducer(spawnTriggerString), new AnimationTriggerReproducer(SkipTriggerString));
            //// crane transform / animator
            //this.crane = crane;
            //// kimboko transform / animator
            //this.spawnKimboko = spawnKimboko;
            //this.spawnPosition = spawnPosition;
            //this.craneEnd = craneEnd;
            //craneAnimator = crane.GetComponent<Animator>();
            //kimbokoAnimator = spawnKimboko.GetComponent<Animator>();
        }

        public override bool CheckCorrectInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                return true;
            }

            return false;
        }

        //protected override void StartMotion()
        //{
        //    //A CRANE//GRUA SET ACTIVE = TRUE
        //    crane.SetActive(true);
        //    //B TWEEN DESDE UNA POSICION ELEVADA SOBRE LA TILE DONDE SE INDICO SPAWNEAR HASTA MAS ABAJO ASI SE VE DESDE ARRIBA EN EL TABLERO SOBRE LA TILE
        //    craneStartPosition = new Vector3(spawnPosition.x, crane.transform.position.y, 0);
        //    crane.transform.position = craneStartPosition;
            

        //    craneEndPostion = new Vector3(spawnPosition.x, Helper.GetCameraTopBorderYWorldPostion().y);
        //    craneTween = crane.transform.DOMove(craneEndPostion, 1).SetEase(ease);
        //}

        //protected override IEnumerator CheckPendingRunningMotions()
        //{

        //    while (crane.transform.position != craneEndPostion)
        //    {
        //        yield return null;
        //    }

        //    ////C ANIMATION CRANESPAWNING

        //    AnimatorStateInfo animst = craneAnimator.GetCurrentAnimatorStateInfo(0);
        //    craneAnimator.SetTrigger(spawnTriggerString);
        //    yield return null;

        //    while (animst.shortNameHash != craneAnimator.GetCurrentAnimatorStateInfo(0).shortNameHash)
        //    {
        //        Debug.Log("Wait for End Animation Time Own Animator Check");
        //        yield return null;
        //    }


        //    ////D INSTANTIATE KIMBOKO DESDE LA PUNTA DEL CRANE DONDE DEBERIA CHORREAR LA GOTA
        //    spawnKimboko.transform.position = craneEndPostion;
        //    spawnKimboko.SetActive(true);
        //    ////E ANIMATION KIMBOKOSPAWNING
        //    ////E TWEEN DESDE LA PUNTA DEL CRANE HASTA EL PISO, DE LA MISMA DURACION QUE LA ANIMACION DE SPAWN
        //    ////F ANIMATION IDLLE... TAL VEZ SEA AUTOMATICO EL CAMBIO, PERO POR LAS DUDAS


        //    AnimatorStateInfo animstKimb = kimbokoAnimator.GetCurrentAnimatorStateInfo(0);
        //    kimbokoAnimator.SetTrigger(spawnTriggerString);

        //    spawnKimbokoTween = spawnKimboko.transform.DOMove(spawnPosition, 1).SetEase(ease);

        //    yield return null;

        //    while (animstKimb.shortNameHash != kimbokoAnimator.GetCurrentAnimatorStateInfo(0).shortNameHash)
        //    {
        //        Debug.Log("Wait for End Animation Time Own Animator Check");
        //        yield return null;
        //    }

        //    while (spawnKimboko.transform.position != spawnPosition)
        //    {
        //        yield return null;
        //    }


        //    //G TWEEN DE LA CRANE PARA QUE SALGA DEL MAPA
        //    craneTween = crane.transform.DOMove(craneStartPosition, 1).SetEase(ease);

        //    while (crane.transform.position != craneStartPosition)
        //    {
        //        //Debug.Log("WAIT FOR REACH END POSTION");
        //        yield return null;
        //    }

        //    //craneTween.Kill();


        //    //Debug.Log("Llego Crane Final");

        //    ////H CRANE//GRUA SET ACTIVE = false
        //    //crane.SetActive(false);

        //    yield return null;
        //}

        //protected override void CheckMotionAfterEnd()
        //{
        //    //animator.SetTrigger("Idlle");
        //}

        //public override void OnMotionSkip()
        //{
        //    craneTween.Kill();
        //    crane.transform.position = craneStartPosition;
        //    kimbokoAnimator.SetTrigger("Idlle");

        //    spawnKimbokoTween.Kill();
        //    spawnKimboko.transform.position = spawnPosition;
        //    craneAnimator.SetTrigger("Idlle");
        //    //animator.SetTrigger("Idlle");
        //    //spawnKimbokoTween.Kill();
        //    //animator.transform.position = finishPosition;

        //    base.OnMotionSkip();
        //}

        //protected override void SpeedUpMotionOnMotion()
        //{
        //    if (craneTween == null) return;

        //    craneTween.timeScale = tweenSpeedUp;
        //    craneAnimator.SetFloat(animationSpeedParameter, animationSpeedUpVelocity);

        //    if (spawnKimbokoTween == null) return;

        //    spawnKimbokoTween.timeScale = tweenSpeedUp;
        //    kimbokoAnimator.SetFloat(animationSpeedParameter, animationSpeedUpVelocity);
        //}

        //protected override void SetNormalSpeedInMotion()
        //{
        //    if (craneTween == null) return;

        //    craneTween.timeScale = tweenNormalSpeed;
        //    craneAnimator.SetFloat(animationSpeedParameter, animationNormalSpeed);

        //    if (spawnKimbokoTween == null) return;

        //    spawnKimbokoTween.timeScale = tweenNormalSpeed;
        //    kimbokoAnimator.SetFloat(animationSpeedParameter, animationNormalSpeed);
        //}

    }

}

