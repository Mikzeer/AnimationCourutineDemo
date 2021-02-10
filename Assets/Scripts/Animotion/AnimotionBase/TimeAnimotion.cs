using UnityEngine;
using System.Collections;

namespace MikzeerGame.Animotion
{
    public class TimeAnimotion : Animotion
    {
        protected float timeNormalSpeed = 1;
        public float timeSpeedUp { get; protected set; }
        public float timeActualSpeed { get; protected set; }

        protected float duration;
        private float timeRemaining;

        public TimeAnimotion(MonoBehaviour coroutineMono, int reproductionOrder, float duration) : base(coroutineMono, reproductionOrder)
        {
            timeActualSpeed = timeNormalSpeed;
            this.duration = duration;
        }

        protected override void StartMotion()
        {
            timeRemaining = duration;
        }

        protected override IEnumerator CheckPendingRunningMotions()
        {
            while (timeRemaining >= 0)
            {
                timeRemaining -= Time.deltaTime * timeActualSpeed;
                float timeToShow = timeRemaining + 1;
                float minutes = Mathf.FloorToInt(timeToShow / 60);
                float seconds = Mathf.FloorToInt(timeToShow % 60);
                //string text = string.Format("{0:00}:{1:00}", minutes, seconds);
                //Debug.Log(text);
                yield return null;
            }
        }

        public override void OnMotionSkip()
        {
            base.OnMotionSkip();
            timeRemaining = 0;
        }

        protected override void OnMotionEnd()
        {
            timeRemaining = 0;
        }

        protected override void SpeedUpMotionOnMotion()
        {
            timeSpeedUp = 2;
            timeActualSpeed = timeSpeedUp;
        }

        protected override void SetNormalSpeedInMotion()
        {
            timeActualSpeed = timeNormalSpeed;
        }

    }
}