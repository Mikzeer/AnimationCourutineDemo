using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace PositionerDemo
{
    public class MoveCombineMotion : Motion
    {
        private const string moveTriggerString = "Move";
        private const string SkipTriggerString = "Idlle";

        private Tween[] movingTween;
        //private Ease ease = Ease.Linear;

        public List<Transform> enemies;
        private Vector3 endPostion;
        private Vector3 actualPosition;

        Dictionary<Transform, Vector3[]> enmiesAndPathToMove;
        UnitMovePositioner movePositioner;
        private float cellSize;
        private Vector3 finalPosition;
        private Vector3 startPosition;

        public MoveCombineMotion(MonoBehaviour coroutineMono, Vector3 startPosition, Vector3 finalPosition, List<Transform> enemies, float cellSize) : base(coroutineMono, 1)
        {
            this.cellSize = cellSize;
            movePositioner = new UnitMovePositioner(this.cellSize);
            this.startPosition = startPosition;
            this.finalPosition = finalPosition;
            this.enemies = enemies;
            //tweenActualSpeed = tweenNormalSpeed;

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
        //    endPostion = finalPosition;

        //    enmiesAndPathToMove = movePositioner.GetRoutePositions(enemies.ToArray(), movePositioner.GetPositionType(enemies.Count), finalPosition, startPosition);
        //    movingTween = new Tween[enemies.Count];


        //    int index = 0;
        //    foreach (KeyValuePair<Enemy, Vector3[]> entry in enmiesAndPathToMove)
        //    {

        //        entry.Key.animator.SetTrigger(moveTriggerString);

        //        movingTween[index] = entry.Key.transform.DOPath(entry.Value, 10).SetEase(ease);
        //        movingTween[index].timeScale = tweenActualSpeed;

        //        index++;
        //    }
        //}

        //protected override IEnumerator CheckPendingRunningMotions()
        //{
        //    foreach (KeyValuePair<Enemy, Vector3[]> entry in enmiesAndPathToMove)
        //    {
        //        while (entry.Key.transform.position != entry.Value[2])
        //        {
        //            //Debug.Log("Wait for End Animation Time Own Animator Check");
        //            yield return null;
        //        }
        //        //entry.Key.animator.SetTrigger("Idlle");
        //    }
        //}

        //public override void OnMotionSkip()
        //{
        //    actualPosition = endPostion;

        //    int index = 0;
        //    foreach (KeyValuePair<Enemy, Vector3[]> entry in enmiesAndPathToMove)
        //    {
        //        //entry.Key.animator.SetTrigger("Idlle");
        //        movingTween[index].Kill();
        //        index++;
        //        entry.Key.transform.position = entry.Value[2];
        //    }

        //    base.OnMotionSkip();
        //}

        //protected override void CheckMotionAfterEnd()
        //{
        //    foreach (KeyValuePair<Enemy, Vector3[]> entry in enmiesAndPathToMove)
        //    {
        //        entry.Key.animator.SetTrigger("Idlle");
        //    }

        //    actualPosition = finalPosition;
        //}

        //protected override void SpeedUpMotionOnMotion()
        //{
        //    int index = 0;
        //    foreach (KeyValuePair<Enemy, Vector3[]> entry in enmiesAndPathToMove)
        //    {
        //        movingTween[index].timeScale = tweenSpeedUp;
        //        entry.Key.animator.SetFloat(animationSpeedParameter, animationSpeedUpVelocity);
        //        index++;
        //    }
        //}

        //protected override void SetNormalSpeedInMotion()
        //{
        //    int index = 0;
        //    foreach (KeyValuePair<Enemy, Vector3[]> entry in enmiesAndPathToMove)
        //    {
        //        movingTween[index].timeScale = tweenNormalSpeed;
        //        entry.Key.animator.SetFloat(animationSpeedParameter, animationNormalSpeed);
        //        index++;
        //    }
        //}

    }

}

