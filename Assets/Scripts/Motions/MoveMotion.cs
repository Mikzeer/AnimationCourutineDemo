using System.Collections;
using UnityEngine;
using DG.Tweening;
namespace PositionerDemo
{
    public class MoveMotion : AnimatedMotion
    {
        private const string moveTriggerString = "Move";

        private Tween movingTween;
        private Ease ease = Ease.Linear;

        private Vector2 startPosition;
        private Vector3 endPostion;
        private Vector3 finishPosition;

        bool va = true;

        public MoveMotion(MonoBehaviour coroutineMono, Animator animator) : base(coroutineMono, animator)
        {

        }

        public override bool CheckCorrectInput()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                return true;
            }

            return false;
        }

        protected override void StartMotion()
        {
            startPosition = animator.GetComponent<Transform>().position;
            Vector2 oldPosition = startPosition; // 5.96,-3.15
            if (va)
            {
                endPostion = startPosition + new Vector2(0, 8); // 5.96,11.85
                va = false;
            }
            else
            {
                endPostion = startPosition + new Vector2(0, -8); // 5.96,11.85
                va = true;
            }
            finishPosition = endPostion;



            animator.SetTrigger(moveTriggerString);


            movingTween = animator.transform.DOMove(endPostion, 20).SetEase(ease);

            movingTween.timeScale = tweenActualSpeed;


            startPosition = endPostion; 
            endPostion = oldPosition;
        }

        protected override IEnumerator CheckPendingRunningMotions()
        {
            while (animator.transform.position != finishPosition)
            {
                //Debug.Log("Wait for End Animation Time Own Animator Check");
                yield return null;
            }
        }

        protected override void OnMotionSkip()
        {
            animator.SetTrigger("Idlle");


            movingTween.Kill();
            animator.transform.position = finishPosition;

            base.OnMotionSkip();
        }

        protected override void CheckMotionAfterEnd()
        {
            animator.SetTrigger("Idlle");
        }

        protected override void SpeedUpMotionOnMotion()
        {
            if (movingTween == null) return;

            movingTween.timeScale = tweenSpeedUp;
            animator.SetFloat(animationSpeedParameter, animationSpeedUpVelocity);
        }

        protected override void SetNormalSpeedInMotion()
        {
            if (movingTween == null) return;

            movingTween.timeScale = tweenNormalSpeed;


            animator.SetFloat(animationSpeedParameter, animationNormalSpeed);
        }

    }

}

